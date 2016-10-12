/// ===============================================================================================
/// <description>Implementation file for SignalStatusToConsole</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "SignalStatusToConsole.h"
#include <exception>

/// ===============================================================================================
/// <summary>Writes a file to the hard drive that the console installer will poll for.</summary>
/// <param name="hInstall">handle to the installer.</param>
/// <exception cref="pE">Exceptions are handled by the called function.</exception>
/// ===============================================================================================
UINT __stdcall SignalStatusToConsole(MSIHANDLE hInstall)
{
    UINT errorCode = ERROR_SUCCESS;

    HRESULT hr = WcaInitialize(hInstall, "SignalStatusToConsole");
    ExitOnFailure(hr, "Failed to initialize");

    WcaLog(LOGMSG_STANDARD, "Initialized.");

    try
    {
        DropFile();
    }
    catch (std::exception const& e)
    {
        WcaLog(LOGMSG_STANDARD, "SignalStatusToConsole exception thrown.");
        WcaLog(LOGMSG_STANDARD, e.what());
    }
    catch (...)
    {
        WcaLog(LOGMSG_STANDARD, "SignalStatusToConsole generic exception thrown.");
    }

LExit:
    errorCode = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
    return WcaFinalize(errorCode);
}

/// ===============================================================================================
/// <summary>Creates the file the console installer will poll for.</summary>
/// <param name="cstrIPAddress">Returned IP Address of the AU.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// ===============================================================================================
void DropFile(void)
{
    HANDLE hFile = INVALID_HANDLE_VALUE;
    DWORD byteswritten = 0;

    hFile = CreateFile(CSTR_DROPFILE_PATH, GENERIC_ALL, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

    if (hFile != INVALID_HANDLE_VALUE)
    {
        WcaLog(LOGMSG_STANDARD, "DropFile Created.");

        if (WriteFile(hFile, SZ_SUCCESS, strlen(SZ_SUCCESS), &byteswritten, NULL) == TRUE)
        {
            WcaLog(LOGMSG_STANDARD, "WriteFile successful.");
        }
        else
        {
            WcaLog(LOGMSG_STANDARD, "WriteFile failed.");
        }

        CloseHandle(hFile);
    }
    else
    {
        WcaLog(LOGMSG_STANDARD, "Unable to create DropFile.");
    }
}