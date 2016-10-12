/// ===============================================================================================
/// <description>Implementation file for IsAuInstallSuccessful</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "IsAuInstallSuccessful.h"
#include "CustomActionCommon.h"
#include <exception>
#include <iostream>
#include <fstream>
#include <string>
using namespace std;
/// ===============================================================================================
/// <summary>This function determines if the AU installation was successful.</summary>
/// <param name="hInstall">handle to the installer process. Needed to put address in memory.</param>
/// <exception cref="pE">Catch all exceptions and log. Return false if exception thrown.</exception>
/// <returns>Returning TRUE indicates installation can continue. FALSE indicates that we were not
/// able to stop all running processes.</returns>
/// ===============================================================================================
UINT __stdcall IsAuInstallSuccessful(MSIHANDLE hInstall)
{
    HRESULT hr = S_OK;
    UINT er = ERROR_SUCCESS;
    CString cstrErrorCode;

    hr = WcaInitialize(hInstall, "IsAuInstallSuccessful");
    ExitOnFailure(hr, "Failed to initialize");

    Log("Start: IsAuInstallSuccessful");

    try
    {
        if (WasAuInstallSuccessful(cstrErrorCode, hInstall) == TRUE)
        {
            MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_NO_ERROR));
        }
        else
        {
			MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_CANT_SEE_AU));
			hr = CA_FAIL;
        }
    }
    catch (std::exception const& e)
    {
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, cstrErrorCode);
        Log("IsAuInstallSuccessful exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, cstrErrorCode);
        Log("IsAuInstallSuccessful generic exception thrown.");
    }

    Log("End: IsAuInstallSuccessful");
LExit:
    er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
    return WcaFinalize(er);
}

/// ===============================================================================================
/// <summary>This function reads the file the AU created at the end of installation.</summary>
/// <param name="fullPath">Path to the file the AU created at install completion.</param>
/// <param name="failureInformation">Failure code in AU file if a failure occured.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// <returns>Return True if the file that was found contained a coherent message. False if not.</returns>
/// ===============================================================================================
BOOL IsInstallFinished(CString fullPath, char *failureInformation)
{
    BOOL bIsFinished = FALSE;
    HANDLE hFile = INVALID_HANDLE_VALUE;
    OVERLAPPED ol = { 0 };
    char fileBuffer[BUFFER_SIZE_100] = { 0 };
    CString cstrMsg;

    try
    {
        hFile = CreateFile(fullPath, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

		if (INVALID_HANDLE_VALUE != hFile)
        {
            if (TRUE == ReadFileEx(hFile, fileBuffer, BUFFER_SIZE_100 - 1, &ol, NULL))
            {
                CloseHandle(hFile);

                CString cstrFileContents(fileBuffer);
                cstrMsg.Format(_T("File %s contents: %s."), fullPath, cstrFileContents);
                Log(cstrMsg);

                // If we do have a file with contents, distinguish between success and failure.
                if (0 == strncmp(SZ_SUCCESS, fileBuffer, strlen(SZ_SUCCESS)) || 0 == strncmp(SZ_FAIL, fileBuffer, strlen(SZ_FAIL)))
                {
                    bIsFinished = TRUE;

                    // If the AU failed to install, capture the failure code.
                    if (0 == strncmp(SZ_FAIL, fileBuffer, strlen(SZ_FAIL)))
                    {
                        char *p = fileBuffer + strlen(SZ_FAIL) + 1; // point to beginning of code
                        strncpy_s(failureInformation, BUFFER_SIZE_20, p, CONST_THREE); // code is three digits
                        failureInformation[CONST_THREE] = CHAR_NULL; // NULL terminate the string
                    }
                }
            }

            CloseHandle(hFile);
        }
        else
        {
            cstrMsg.Format(_T("CreateFile failed with error code: %d"), GetLastError());
            Log(cstrMsg);
        }
    }
    catch (std::exception const& e)
    {
        Log("IsInstallFinished exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("IsInstallFinished generic exception thrown.");
    }

    return bIsFinished;
}

/// ===============================================================================================
/// <summary>This function polls for the file that the installer is supposed to create.</summary>
/// <param name="cstrErrorCode">Returned error code on failure.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// <returns>Return True if file was found within polling period and false if not.</returns>
/// ===============================================================================================
BOOL WasAuInstallSuccessful(CString &cstrErrorCode, MSIHANDLE handle)
{
    BOOL bWasInstallSuccessful = FALSE;
    CString cstrMsg;
    char szErrorText[BUFFER_SIZE_20];
    memset(szErrorText, CHAR_NULL, BUFFER_SIZE_20);

	// Since the Au IPAddress is set for every install, we can assume this is the IP address.
	CString cstrIPAddress = "192.168.2.4";
	MsiSetProperty(handle, CSTR_AU_IP_ADDRESS, cstrIPAddress);

    CString cstrInstallFile;
    cstrInstallFile.Format(CSTR_AUINSTALL_TXT, cstrIPAddress);

    cstrMsg.Format(_T("Calling IsInstallFinished for file %s"), cstrInstallFile);
    Log(cstrMsg);
    // Set up a loop to look for success or failure of the install
    for (int i = 0; i < MAX_ITERATIONS; i++)
    {
        if (IsInstallFinished(cstrInstallFile, szErrorText) == TRUE)
        {
			if (szErrorText[0] == CHAR_NULL)
            {
                bWasInstallSuccessful = TRUE;
            }
            else
            {
                cstrErrorCode = szErrorText;
            }

            break;
        }

        Sleep(ONE_THOUSAND);
    }

    return bWasInstallSuccessful;
}