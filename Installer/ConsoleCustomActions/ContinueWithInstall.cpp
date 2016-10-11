/// ===============================================================================================
/// <description>Implementation file for ContinueWithInstall</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "ContinueWithInstall.h"
#include "CustomActionCommon.h"
#include "IsAuCriticalEventsJammed.h"
#include "IsAuInActiveMode.h"
#include "IsVersionNumbersOk.h"
#include "KillConsoleProcesses.h"
#include <exception>

/// ===============================================================================================
/// <summary>This method combines a few pre-condition methods required for installation.</summary>
/// <param name="hInstall">Handle to the install process. Used for debugging purposes.</param>
/// <exception cref="pE">Catch all exceptions and log. Return false if exception thrown.</exception>
/// <returns>Returning SUCCESS indicates installation can continue. FALSE indicates that we were 
/// are blocked in our ability to successfully install.</returns>
/// ===============================================================================================
UINT __stdcall ContinueWithInstall(MSIHANDLE hInstall)
{
	HRESULT hr = S_OK;
	UINT er = ERROR_SUCCESS;

	hr = WcaInitialize(hInstall, "ContinueWithInstall");
	ExitOnFailure(hr, "Failed to initialize");

	Log("Start: ContinueWithInstall");

	try
	{
		if (ContinueWithTheInstall(hInstall) == FALSE)
		{
			hr = CA_FAIL;
		}
	}
	catch (std::exception const& e)
	{
		Log("ContinueWithInstall exception thrown.");
		Log(e.what());
	}
	catch (...)
	{
		Log("ContinueWithInstall generic exception thrown.");
	}

	Log("End: ContinueWithInstall");
LExit:
	er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;

	return WcaFinalize(er);
}

/// ===============================================================================================
/// <summary>Determines if the installation should continue. Controlling method.</summary>
/// <param name="hInstall">handle to the install process. Used for debugging purposes.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// <returns>Returns true if the conditions are ok to continue with the install.  FALSE indicates 
/// the system is not ready to install and an error code property will be set. </returns>
/// ===============================================================================================
BOOL ContinueWithTheInstall(MSIHANDLE hInstall)
{
	BOOL bContinue = TRUE;
	char szInstallVersion[BUFFER_SIZE_100];
	memset(szInstallVersion, CHAR_NULL, BUFFER_SIZE_100);

	// First check if we can get the installation version 
	if (GetInstallVersion(hInstall, szInstallVersion))
	{
		bContinue = CheckAuInstallStatus(hInstall, szInstallVersion);

		if (bContinue == TRUE)
		{
			bContinue = CheckConsoleInstallStatus(hInstall, szInstallVersion);
		}
	}
	else
	{
		MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_VERSION));
	}

	return bContinue;
}

/// ===============================================================================================
/// <summary>Checks the install conditions to determine if the AU is in a state where installation 
/// of system software can continue.</summary>
/// <param name="hInstall">handle to the install process. Used for debugging purposes.</param>
/// <param name="szInstallVersion">The version of software we are attempting to install.</param>
/// <returns>Returns true if the AU conditions are ok to continue with the install.  FALSE indicates 
/// the system is not ready to install and an error code property will be set. </returns>
/// ===============================================================================================
BOOL CheckAuInstallStatus(MSIHANDLE hInstall, char *szInstallVersion)
{
	BOOL bContinue = FALSE;
	BOOL bAuExists = FALSE;
	CString cstrMsg;

	CString cstrIPAddress;
	GetAuIPAddress(cstrIPAddress, hInstall);

	CString cstrAuFileName;
	cstrAuFileName.Format(CSTR_AU_SYSTEMVERSION_TXT, cstrIPAddress);

	// See if SystemVersion.txt exists on the AU. This really just checks to see if we need to stop the Au.
	if (InstallFileExists(cstrAuFileName) == FALSE)
	{
		cstrMsg.Format(_T("%s does not exist. New installation."), cstrAuFileName);
		Log(cstrMsg);
	}
	else
	{
		cstrMsg.Format(_T("%s exists. Upgrade installation."), cstrAuFileName);
		Log(cstrMsg);
		bAuExists = TRUE;
	}

	if (bAuExists)
	{
		if (AreVersionNumbersOk(szInstallVersion, cstrAuFileName) == TRUE)
		{
			if (IsAuObjectInActiveMode(cstrIPAddress) == FALSE)
			{
				if (StopAuFromConsole(hInstall) == TRUE)
				{
					if (AreAuCriticalEventsJammed(cstrIPAddress) == FALSE)
					{
						//Au is in a state where the installation can continue.
						bContinue = TRUE;
					}
					else
					{
						MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_JAMMED_CRITICAL_EVENTS));
					}
				}
				else
				{
					MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_STOP_AU_FROM_CONSOLE));
				}
			}
			else
			{
				MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_AU_ACTIVE));
			}
		}
		else
		{
			MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_VERSION));
		}
	}
	else
	{
		//Installation can continue even though the AU may not have previous softwarw installed (it is a new install).
		bContinue = TRUE;
	}

	return bContinue;
}

/// ===============================================================================================
/// <summary>Checks the install conditions to determine if the Console is in a state where installation 
/// of system software can continue.</summary>
/// <param name="hInstall">handle to the install process. Used for debugging purposes.</param>
/// <param name="szInstallVersion">The version of software we are attempting to install.</param>
/// <returns>Returns true if the Console conditions are ok to continue with the install.  FALSE indicates 
/// the system is not ready to install and an error code property will be set. </returns>
/// ===============================================================================================
BOOL CheckConsoleInstallStatus(MSIHANDLE hInstall, char *szInstallVersion)
{
	BOOL bContinue = FALSE;
	BOOL bConsoleExists = FALSE;
	CString cstrMsg;

	// See if SystemVersion.txt exists on the Console.
	if (InstallFileExists(CSTR_CONSOLE_SYSTEMVERSION_TXT) == FALSE)
	{
		cstrMsg.Format(_T("%s does not exist. New installation."), CSTR_CONSOLE_SYSTEMVERSION_TXT);
		Log(cstrMsg);
	}
	else
	{
		cstrMsg.Format(_T("%s exists. Upgrade installation."), CSTR_CONSOLE_SYSTEMVERSION_TXT);
		Log(cstrMsg);
		bConsoleExists = TRUE;
	}

	if (bConsoleExists == TRUE)
	{
		if (AreVersionNumbersOk(szInstallVersion, CSTR_CONSOLE_SYSTEMVERSION_TXT) == TRUE)
		{
			if (KillConsoleProcesses() == TRUE)
			{
				// We are good for upgrade install
				bContinue = TRUE;
			}
			else
			{
				MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_KILL_PROCESSES));
			}
		}
		else
		{
			MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_VERSION));
		}
	}
	else
	{
		//Installation can continue even though the Console may not have previous softwarw installed (it is a new install).
		bContinue = TRUE;
	}
	return bContinue;
}

/// ===============================================================================================
/// <summary>Reads the version of the product to be installed.</summary>
/// <param name="handle">handle to the install process. Used for debugging purposes.</param>
/// <param name="szInstallVersion">The version of software to be installed.</param>
/// <exception cref="pE">This method should catch exception and return FALSE.</exception>
/// ===============================================================================================
BOOL GetInstallVersion(MSIHANDLE handle, char *szInstallVersion)
{
	BOOL bInstallVersionExists = FALSE;
	CString cstrLog;
	DWORD dwBufSize = BUFFER_SIZE_100;
	wchar_t szProductVersionValueBuf[BUFFER_SIZE_100];

	try
	{
		memset(szProductVersionValueBuf, CHAR_NULL, BUFFER_SIZE_100);
		Assert(szInstallVersion != NULL);

		// Get version to be installed
		UINT uiStat = MsiGetProperty(handle, CSTR_PRODUCT_VERSION, szProductVersionValueBuf, &dwBufSize);

		if (uiStat == ERROR_SUCCESS)
		{
			if (0 == WideCharToMultiByte(CP_ACP, WC_COMPOSITECHECK | WC_DEFAULTCHAR, szProductVersionValueBuf, -1, szInstallVersion, BUFFER_SIZE_100 - 1, NULL, NULL))
			{
				Log("WideCharToMultiByte failed.");
			}
			else
			{
				CString cstrVersion = szInstallVersion;
				cstrLog.Format(_T("InstallVersionNumber: %s"), cstrVersion);
				Log(cstrLog);
				bInstallVersionExists = TRUE;
			}
		}
		else
		{
			cstrLog.Format(_T("MsiGetProperty failed. Error: %d."), uiStat);
			Log(cstrLog);
		}
	}
	catch (std::exception const& e)
	{
		Log("GetInstallVersion exception thrown.");
		Log(e.what());
	}
	catch (...)
	{
		Log("GetInstallVersion generic exception thrown.");
	}

	return bInstallVersionExists;
}

/// ===============================================================================================
/// <summary>Determines if the installation file exists. Looks for it by name.</summary>
/// <param name="fullPath">Full path and name of the file.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// <returns>Returns True if the file exists, false if not.</returns>
/// ===============================================================================================
BOOL InstallFileExists(CString fullPath)
{
	BOOL bFileExits = FALSE;
	HANDLE hFile = INVALID_HANDLE_VALUE;

	try
	{
		if (!fullPath.IsEmpty())
		{
			// 'CreateFile' should be read as 'OpenFile' since we're using the OPEN_EXISTING param
			hFile = CreateFile(fullPath, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

			if (hFile != INVALID_HANDLE_VALUE)
			{
				bFileExits = TRUE;
				CloseHandle(hFile);
			}
		}
		else
		{
			Log("InstallFileExists: fullPath is empty!");
		}
	}
	catch (std::exception const& e)
	{
		Log("InstallFileExists exception thrown.");
		Log(e.what());
	}
	catch (...)
	{
		Log("InstallFileExists generic exception thrown.");
	}

	return bFileExits;
}
