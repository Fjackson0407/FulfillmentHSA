/// ===============================================================================================
/// <description>Implementation file for SetRunOnceThenRebootAu</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "SetRunOnceThenRebootAu.h"
#include "CustomActionCommon.h"
#include <exception>

/// ===============================================================================================
/// <summary>This function is responsible for setting the "RunOnce" registry key on the
/// instrument from the console. This requires that the instrument be set to share its registry.
/// Once the "RunOnce" is set to run the installer on startup, the instrument is rebooted using
/// the PSExec tool.</summary>
/// <param name="hInstall">Handle to the install process. Given to us by installer.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning SUCCESS if we could perform all operations, else return ERROR.</returns>
/// ===============================================================================================
UINT __stdcall SetRunOnceThenRebootAu(MSIHANDLE hInstall)
{
	HRESULT hr = S_OK;
	UINT er = ERROR_SUCCESS;

	hr = WcaInitialize(hInstall, "SetRunOnceThenRebootAu");
	ExitOnFailure(hr, "Failed to initialize");

	Log("Start: SetRunOnceThenRebootAu");

	try
	{
		if (SetRunOnceThenRebootTheAu(hInstall) == FALSE)
		{
			hr = MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_REBOOT_AU_FAILED));
			hr = CA_FAIL;
		}
	}
	catch (std::exception const& e)
	{
		Log("SetRunOnceThenRebootAu exception thrown.");
		Log(e.what());
		MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_REBOOT_AU_FAILED));
	}
	catch (...)
	{
		MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_REBOOT_AU_FAILED));
		Log("SetRunOnceThenRebootAu generic exception thrown.");
	}

	Log("End: SetRunOnceThenRebootAu");
LExit:
	er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;

	return WcaFinalize(er);
}

/// ===============================================================================================
/// <summary>This method simply gets the insturment IP address and continues the process.</summary>
/// <exception cref="pE">Exception handling occurs in the calling method.</exception>
/// <returns>Returning TRUE if we could perform all operations, else return FALSE.</returns>
/// ===============================================================================================
BOOL SetRunOnceThenRebootTheAu(MSIHANDLE hInstall)
{
	BOOL bRebootSuccessful = FALSE;
	CString cstrAuIPAddress;
	GetAuIPAddress(cstrAuIPAddress, hInstall);

	CString cstrAuName;
	cstrAuName.Format(CSTR_REMOTE_MACHINE_NAME, cstrAuIPAddress);

	if (SetRunOnce(cstrAuName) == TRUE)
	{
		if (SendRebootCommand(cstrAuIPAddress) == TRUE)
		{
			bRebootSuccessful = TRUE;
		}
	}

	return bRebootSuccessful;
}

/// ===============================================================================================
/// <summary>This method opens the remote registry and sets the RunOnce key.</summary>
/// <param name="cstrAuName">Path to the remote registry.</param>
/// <exception cref="pE">Exception handling occurs in the calling method.</exception>
/// <returns>Returning TRUE if we could set the key, else return FALSE.</returns>
/// ===============================================================================================
BOOL SetRunOnce(CString cstrAuName)
{
	BOOL bRunOnce = FALSE;
	CString cstrLog;
	HKEY   hKey;

	// Set the RunOnce registry setting on the AU
	if (RegConnectRegistry(cstrAuName, HKEY_LOCAL_MACHINE, &hKey) == ERROR_SUCCESS)
	{
		HKEY hResultKey;
		if (RegOpenKeyEx(hKey, CSTR_RUN_ONCE, 0, KEY_SET_VALUE, &hResultKey) == ERROR_SUCCESS)
		{
			if (RegSetValueEx(hResultKey, SZ_DXI_INSTALL, 0, REG_SZ, (LPBYTE)SZ_MSI_INSTALL, wcslen(SZ_MSI_INSTALL) * sizeof(TCHAR)) == ERROR_SUCCESS)
			{
				bRunOnce = TRUE;
			}
			else
			{
				cstrLog.Format(_T("RegSetValueEx for %s failed. Error Code: %d"), SZ_DXI_INSTALL, GetLastError());
				Log(cstrLog);
			}

			RegCloseKey(hResultKey);
		}
		else
		{
			cstrLog.Format(_T("RegOpenKeyEx for %s failed. Error Code: %d"), CSTR_RUN_ONCE, GetLastError());
			Log(cstrLog);
		}

		RegCloseKey(hKey);
	}
	else
	{
		cstrLog.Format(_T("RegConnectRegistry for %s failed. Error Code: %d"), cstrAuName, GetLastError());
		Log(cstrLog);
	}

	return bRunOnce;
}