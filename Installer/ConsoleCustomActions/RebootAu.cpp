/// ===============================================================================================
/// <description>Implementation file for RebootAu</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "RebootAu.h"
#include "CustomActionCommon.h"
#include <exception>

/// ===============================================================================================
/// <summary>This function is responsible for rebooting the instrument using the PSExec tool.</summary>
/// <param name="hInstall">Handle to the install process. Given to us by installer.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning SUCCESS if we could perform all operations, else return ERROR.</returns>
/// ===============================================================================================
UINT __stdcall RebootAu(MSIHANDLE hInstall)
{
    HRESULT hr = S_OK;
    UINT er = ERROR_SUCCESS;

    hr = WcaInitialize(hInstall, "RebootAu");
    ExitOnFailure(hr, "Failed to initialize");

    Log("Start: RebootAu");

    try
    {
        if (RebootTheAu(hInstall) == FALSE)
        {
            hr = MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_REBOOT_AU_FAILED));
            hr = CA_FAIL;
        }
    }
    catch (std::exception const& e)
    {
        Log("RebootAu exception thrown.");
        Log(e.what());
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_REBOOT_AU_FAILED));
    }
    catch (...)
    {
        MsiSetProperty(hInstall, CSTR_ERROR_CODE, GetErrorString(hInstall, ERROR_REBOOT_AU_FAILED));
        Log("RebootAu generic exception thrown.");
    }

    Log("End: RebootAu");
LExit:
    er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;

    return WcaFinalize(er);
}

/// ===============================================================================================
/// <summary>This method simply gets the insturment IP address and continues the process.</summary>
/// <exception cref="pE">Exception handling occurs in the calling method.</exception>
/// <returns>Returning TRUE if we could perform all operations, else return FALSE.</returns>
/// ===============================================================================================
BOOL RebootTheAu(MSIHANDLE hInstall)
{
    BOOL bRebootSuccessful = FALSE;
    CString cstrAuIPAddress;
    GetAuIPAddress(cstrAuIPAddress, hInstall);

    CString cstrAuName;
    cstrAuName.Format(CSTR_REMOTE_MACHINE_NAME, cstrAuIPAddress);

	if (SendRebootCommand(cstrAuIPAddress) == TRUE)
    {
		bRebootSuccessful = TRUE;
    }

    return bRebootSuccessful;
}