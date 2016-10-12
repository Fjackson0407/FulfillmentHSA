/// ===============================================================================================
/// <description>Implementation file for RestartConsoleComputer</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "CustomActionCommon.h"
#include <exception>

/// ===============================================================================================
/// <summary>This method reboots the console computer upon install completion.</summary>
/// <param name="hInstall">Handle to the install process. Given to us by installer.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning SUCCESS if we could perform all operations, else return FAILURE.</returns>
/// ===============================================================================================
UINT _stdcall RestartConsoleComputer(MSIHANDLE hInstall)
{
    HRESULT hr = S_OK;
    UINT er = ERROR_SUCCESS;

    hr = WcaInitialize(hInstall, "RestartConsoleComputer");
    ExitOnFailure(hr, "Failed to initialize");

    Log("Start: RestartConsoleComputer");

    try
    {
        if (ExitWindowsEx(EWX_REBOOT, SHTDN_REASON_MAJOR_OTHER) == FALSE)
        {
            CString cstrMsg;
            cstrMsg.Format(_T("RestartTheConsoleComputer Error: %d"), GetLastError());
            Log(cstrMsg);
        }
    }
    catch (std::exception const& e)
    {
        Log("RestartConsoleComputer exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("RestartConsoleComputer generic exception thrown.");
    }

    Log("End: RestartConsoleComputer");
LExit:
    er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
    return WcaFinalize(er);
}
