/// ===============================================================================================
/// <description>Implementation file for CanSeeAuFromConsole</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include <fstream>
#include "CustomActionCommon.h"
#include "CanSeeAuFromConsole.h"
#include <exception>
using namespace std;

/// ===============================================================================================
/// <summary>This method determines if the Console computer can communicate with the AU.</summary>
/// <param name="hInstall">Handle to the install process. Given to us by installer.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning SUCCESS if we could perform all operations, else return ERROR.</returns>
/// ===============================================================================================
UINT _stdcall CanSeeAuFromConsole(MSIHANDLE hInstall)
{
    HRESULT hr = S_OK;
    UINT er = ERROR_SUCCESS;

    hr = WcaInitialize(hInstall, "CanSeeAuFromConsole");
    ExitOnFailure(hr, "Failed to initialize");

    Log("Start: CanSeeAuFromConsole");

    try
    {
        if (CanOpenAuFile(hInstall) == FALSE)
        {
            MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_CANT_SEE_AU));
            hr = CA_FAIL;
        }
    }
    catch (std::exception const& e)
    {
        Log("CanSeeAuFromConsole exception thrown.");
        Log(e.what());
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_CANT_SEE_AU));
    }
    catch (...)
    {
        Log("CanSeeAuFromConsole generic exception thrown.");
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_CANT_SEE_AU));
    }
        
    Log("End: CanSeeAuFromConsole");
LExit:
    er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
    return WcaFinalize(er);
}

/// ===============================================================================================
/// <summary>This method gets the IP Address of the AU and looks for a known windows file.
/// If the file exists, we know we can see the instrument. We also look to see </summary>
/// <param name="handle">Handle to the install process. Given to us by installer.</param>
/// <exception cref="pE">Exception handling occurs in the calling method.</exception>
/// <returns>Returning TRUE if we opened the file, else return FALSE.</returns>
/// ===============================================================================================
BOOL CanOpenAuFile(MSIHANDLE handle)
{
    BOOL bCanSeeAuFromConsole = FALSE;
    CString cstrMsg;
    CString cstrIPAddress;

    // First see if we are on the Console PC. 
    TCHAR szNetBiosName[BUFFER_SIZE_256] = TEXT("");
    DWORD dwSize = sizeof(szNetBiosName);

    if (!GetComputerNameEx(ComputerNameNetBIOS, szNetBiosName, &dwSize))
    {
        cstrMsg.Format(_T("GetComputerNameEx failed (%d)\n"), GetLastError());
        Log(cstrMsg);
    }
    else
    {
        cstrMsg.Format(_T("GetComputerNameEx passed. Computer name: %s\n"), szNetBiosName);
        Log(cstrMsg);
        // Look for console computer name
        CString cstrNetBiosName = szNetBiosName;

        if (cstrNetBiosName == CSTR_CONSOLE_PC_NAME)
        {
            GetAuIPAddress(cstrIPAddress, handle);

            CString cstrWindowsFilePath;
            cstrWindowsFilePath.Format(CSTR_WIN_INI, cstrIPAddress);

            ifstream fileWindowsIni;

            fileWindowsIni.open(cstrWindowsFilePath, ifstream::in);

            if (fileWindowsIni.is_open())
            {
                bCanSeeAuFromConsole = TRUE;
                cstrMsg.Format(_T("%s found!"), cstrWindowsFilePath);
                fileWindowsIni.close();
            }
            else
            {
                cstrMsg.Format(_T("%s not found! Either the IP Address is wrong or Win.ini isn't there."), cstrWindowsFilePath);
            }

            Log(cstrMsg);
        }
    }
        
    return bCanSeeAuFromConsole;
}