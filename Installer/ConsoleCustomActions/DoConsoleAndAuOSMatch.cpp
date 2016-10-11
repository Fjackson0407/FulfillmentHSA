/// ===============================================================================================
/// <description>Implementation file for DoConsoleAndAuOSMatch</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "CustomActionCommon.h"
#include "DoConsoleAndAuOSMatch.h"
#include "lm.h"
#include <exception>

/// ===============================================================================================
/// <summary>This function determines if the AU and Console operating systems are of the correct
/// version and if they match.</summary>
/// <param name="hInstall">handle to the installer process. Needed to put address in memory.</param>
/// <exception cref="pE">Catch all exceptions and log. Return false if exception thrown.</exception>
/// <returns>Returning TRUE indicates installation can continue. FALSE indicates that we were not
/// able to stop all running processes.</returns>
/// ===============================================================================================
UINT __stdcall DoConsoleAndAuOSMatch(MSIHANDLE hInstall)
{
    HRESULT hr = S_OK;
    UINT er = ERROR_SUCCESS;

    hr = WcaInitialize(hInstall, "DoConsoleAndAuOSMatch");
    ExitOnFailure(hr, "Failed to initialize");

    Log("Start: DoConsoleAndAuOSMatch");

    try
    {
        if (DoTheConsoleAndAuOSMatch(hInstall) == FALSE)
        {
            MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_OS_MATCH_FAIL));
            hr = CA_FAIL;
        }
    }
    catch (std::exception const& e)
    {
        Log("DoConsoleAndAuOSMatch exception thrown.");
        Log(e.what());
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_OS_MATCH_FAIL));
    }
    catch (...)
    {
        Log("DoConsoleAndAuOSMatch generic exception thrown.");
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_OS_MATCH_FAIL));
    }
    
    Log("End: DoConsoleAndAuOSMatch");
LExit:
    er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;

    return WcaFinalize(er);
}

/// ===============================================================================================
/// <summary>This function determines if the AU and Console operating systems are of the correct
/// version and if they match.</summary>
/// <param name="handle">handle to the installer process. Needed to put address in memory.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// <returns>Return True if versions are good for install and false if not.</returns>
/// ===============================================================================================
BOOL DoTheConsoleAndAuOSMatch(MSIHANDLE handle)
{
    BOOL bMatch = FALSE;
    CString cstrMsg;
    DWORD dwConsoleMajor = 0;
    DWORD dwConsoleMinor = 0;
    DWORD dwAUMajor = 0;
    DWORD dwAUMinor = 0;

    CString localIP;
    CString cstrAUIPAddress;
    GetAuIPAddress(cstrAUIPAddress, handle);

    // Empty IP address will cause NetWkstaGetInfo to look at local machine
    GetVersionInfo(localIP, dwConsoleMajor, dwConsoleMinor);

    GetVersionInfo(cstrAUIPAddress, dwAUMajor, dwAUMinor);

    if ((dwConsoleMajor != 0 && dwAUMajor != 0) && ((dwConsoleMajor == XP_MAJOR && dwConsoleMinor == XP_MINOR) || (dwConsoleMajor == WIN8_MAJOR && dwConsoleMinor == WIN8_MINOR)))
    {
        if (dwConsoleMajor == dwAUMajor && dwConsoleMinor == dwAUMinor)
        {
            bMatch = TRUE;
        }
        else
        {
            cstrMsg.Format(_T("OS issue: Console Major: %d, Console Minor: %d, AU Major: %d, AU Minor %d."), dwConsoleMajor, dwConsoleMinor, dwAUMajor, dwAUMinor);
            Log(cstrMsg);
        }
    }
    else
    {
        cstrMsg.Format(_T("OS issue: Console Major: %d, Console Minor: %d, AU Major: %d, AU Minor %d."), dwConsoleMajor, dwConsoleMinor, dwAUMajor, dwAUMinor);
        Log(cstrMsg);
    }

    return bMatch;
}

/// ===============================================================================================
/// <summary>Get the OS version of the machine indicated by the IP Address</summary>
/// <param name="cstrIPAddress">IP Address of the machine in question.</param>
/// <param name="dwMajorVersion">Returned major OS version.</param>
/// <param name="dwMinorVersion">Returned minor OS version.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// ===============================================================================================
void GetVersionInfo(CString cstrIPAddress, DWORD &dwMajorVersion, DWORD &dwMinorVersion)
{
    CString cstrMsg;
    DWORD dwLevel = WORKSTATION_INFO;
    LPWKSTA_INFO_102 pBuf = NULL;
    NET_API_STATUS nStatus;

    try
    {
        // Now use network call to get AU OS version
        nStatus = NetWkstaGetInfo(cstrIPAddress.GetBuffer(), dwLevel, (LPBYTE *)&pBuf);

        if (nStatus == NERR_Success)
        {
            dwMajorVersion = pBuf->wki102_ver_major;
            dwMinorVersion = pBuf->wki102_ver_minor;

            cstrMsg.Format(_T("NetWkstaGetInfo: IP: %s, Major: %d, Console Minor: %d."), cstrIPAddress, dwMajorVersion, dwMinorVersion);
            Log(cstrMsg);
        }
        else
        {
            cstrMsg.Format(_T("NetWkstaGetInfo failed. ErrorCode: %d"), GetLastError());
            Log(cstrMsg);
        }

        if (pBuf != NULL)
        {
            NetApiBufferFree(pBuf);
        }
    }
    catch (std::exception const& e)
    {
        Log("GetVersionInfo exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("GetVersionInfo generic exception thrown.");
    }
}




