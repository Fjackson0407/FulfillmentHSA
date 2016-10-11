/// ===============================================================================================
/// <description>Implementation file for KillConsoleProcesses</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "CustomActionCommon.h"
#include "KillConsoleProcesses.h"
#include "psapi.h"
#include <winsvc.h>
#include <exception>

/// ===============================================================================================
/// <summary>This function is responsible for stopping all console processes that may interfere
/// with proper installation. We want to avoid the Windows Installer program from popping up a 
/// dialog that says installation can't continue since a program is in use.</summary>
/// <remarks>The list of executables is fixed at compile time.</remarks>
/// <exception cref="pE">Catch all exceptions and log. Return false if exception thrown.</exception>
/// <returns>Returning TRUE indicates installation can continue. FALSE indicates that we were not
/// able to stop all running processes.</returns>
/// ===============================================================================================
BOOL KillConsoleProcesses()
{
    BOOL bConsoleProcessesKilled = FALSE;
    CString cstrMsg;
    try
    {
        if (StopUnicelService() == FALSE)
        {
            Log("UnicelService either not running or couldn't be stopped.");
        }

        if (StopCIAService() == FALSE)
        {
            Log("CIAService either not running or couldn't be stopped.");
        }

        if (KillProcesses() == TRUE)
        {
            bConsoleProcessesKilled = TRUE;
        }
        else
        {
            Log("Not all processes could be stopped.");
        }
    }
    catch (std::exception const& e)
    {
        Log("KillConsoleProcesses exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("KillConsoleProcesses generic exception thrown.");
    }

    return bConsoleProcessesKilled;
}

/// ===============================================================================================
/// <summary>This method loops through a list of process names and determines if that process is
/// running. If it is, the method copies that name to the list of running processes. </summary>
/// <remarks>N/A</remarks>
/// <param name="cstrProcessToFindNames">A hard coded list of processes that
/// may or may not be running. pCSTRRunningProcs is the (output) subset list of processes actually
/// running.</param>
/// <param name="pCSTRRunningProcs">Array of running processes.</param>
/// <exception cref="pE">We have implemented no exception handling.</exception>
/// <returns>Returns the total number of running processes found.</returns>
/// ===============================================================================================
int FindAnyProcesses(CAtlArray<CString> &cstrProcessToFindNames, CAtlArray<CString> &pCSTRRunningProcs)
{
    int nResult = 0;
    DWORD lProcIDs[BUFFER_SIZE_1024];
    DWORD lNeeded = 0;
    DWORD lProcesses = 0;

    Assert(EnumProcesses(lProcIDs, sizeof(lProcIDs), &lNeeded));

    // Get how many procs are running on the system
    lProcesses = lNeeded / sizeof(DWORD);
    // Find the name if each process identifier.
    for (DWORD i = 0; i < lProcesses; i++)
    {
        TCHAR szProcessName[MAX_PATH];    // Get a handle to each process.

        HANDLE hProcess = NULL;

        hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, lProcIDs[i]);

        if (hProcess)
        {
            HMODULE hMod;				// Get the process name.    
            if (EnumProcessModules(hProcess, &hMod, sizeof(hMod), &lNeeded))
            {
                GetModuleBaseName(hProcess, hMod, szProcessName, sizeof(szProcessName));

                for (UINT j = 0; j < cstrProcessToFindNames.GetCount(); j++)
                {
                    const int nLen = INT_COMPARE_LENGTH;
                    if (_wcsnicmp(szProcessName, cstrProcessToFindNames[j], nLen) == 0)
                    {
                        // Fill actual running procs name array
                        pCSTRRunningProcs.Add(szProcessName);
                        nResult++;
                        break;
                    }

                }
            }
            CloseHandle(hProcess);
        }
    }

    return nResult;
}

/// ===============================================================================================
/// <summary>This method attempts to stop a running process of the given name.</summary>
/// <remarks>N/A</remarks>
/// <param name="szProcToKillName">The name of the running process.</param>
/// <param name="bForce">Controls how forcefully to stop the process.</param>
/// <exception cref="pE">We have implemented no exception handling.</exception>
/// <returns>Returns the state of the process after the attempt to stop it was performed.</returns>
/// ===============================================================================================
eTA_State KillProcess(LPCTSTR szProcToKillName, BOOL bForce)
{
    eTA_State eReturn = TA_NOT_FOUND;
    DWORD lProcIDs[1024];
    DWORD lNeeded = 0;
    DWORD lProcesses = 0;

    Assert(EnumProcesses(lProcIDs, sizeof(lProcIDs), &lNeeded));

    // Get how many procs are running
    lProcesses = lNeeded / sizeof(DWORD);
    // Find the name if each process identifier.

    for (DWORD i = 0; i < lProcesses; i++)
    {
        TCHAR szProcessName[MAX_PATH];    // Get a handle to each process.

        HANDLE hProcess = NULL;

        hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, lProcIDs[i]);

        if (hProcess)
        {
            HMODULE hMod;				// Get the process name.    
            if (EnumProcessModules(hProcess, &hMod, sizeof(hMod), &lNeeded))
            {
                GetModuleBaseName(hProcess, hMod, szProcessName, sizeof(szProcessName));
                const int nLen = 6;

                if (_tcsnicmp(szProcessName, szProcToKillName, nLen) == 0)
                {
                    eReturn = TerminateApp(lProcIDs[i], hProcess, TERMINATE_APP_TIMEOUT, bForce);
                    break;
                }
            }
            CloseHandle(hProcess);
        }
    }

    // Logging
    CString cstrTemp;
    cstrTemp.Format(_T("Terminating  %s."), szProcToKillName);
    switch (eReturn)
    {
    case TA_FAILED:
        cstrTemp += _T(" Failed");
        break;
    case TA_SUCCESS_CLEAN:
        cstrTemp += _T(" Cleanly");
        break;
    case TA_SUCCESS_KILL:
        cstrTemp += _T(" Forcibly");
        break;
    case TA_SUCCESS_MULTI_KILL:
        cstrTemp += _T(" Forcibly by multiple attempts");
        break;
    }

    Log(cstrTemp);

    return eReturn;
}

/// ===============================================================================================
/// <summary>This method loads a hard coded list of processes to stop. It then determines which of
/// those processes are actually running. It any are running, they are stopped.</summary>
/// <exception cref="pE">We have implemented no exception handling.</exception>
/// <returns>Returns whether or not the function was able to successfully stop all procs.</returns>
/// ===============================================================================================
BOOL KillProcesses(void)
{
    BOOL bSuccess = FALSE;
    int nProcs = 0;
    int nProcsKilled = 0;
    CString cstrTemp;
    CAtlArray<CString> cstrUIProcNames;
    CAtlArray<CString> cstrActualRunningUIProcs;

    KillProcess(CSTR_NEXGEN_UI, FALSE);
    Sleep(1000);

    LoadUIProcesses(cstrUIProcNames);

    nProcs = FindAnyProcesses(cstrUIProcNames, cstrActualRunningUIProcs);

    while (nProcs > 0)
    {
        cstrTemp.Format(_T("Now looping to kill remaining %d procs."), nProcs);
        Log(cstrTemp);

        // Then, try the brutal way, use the first 6 characters of possible running apps
        for (UINT i = 0; i < cstrUIProcNames.GetCount(); i++)
        {
			if (TA_NOT_FOUND != KillProcess(cstrUIProcNames[i], TRUE))
			{
				nProcsKilled++;
			}     
        }

        nProcs = FindAnyProcesses(cstrUIProcNames, cstrActualRunningUIProcs);
    }

    if (nProcs == 0)
    {
        cstrTemp.Format(_T("All %d processes killed!"), nProcsKilled);
        Log(cstrTemp);
        bSuccess = TRUE;
    }

    cstrTemp.Format(_T("Exiting KillProcesses(Total Killed:%d)..."), nProcsKilled);
    Log(cstrTemp);

    return bSuccess;
}

/// ===============================================================================================
/// <summary>This method copies a hard coded list of process names to the array given.</summary>
/// <remarks>N/A</remarks>
/// <param name="cstrUIProcNames">The list of process we know we must stop</param>
/// ===============================================================================================
void LoadUIProcesses(CAtlArray<CString> &cstrUIProcNames)
{
    cstrUIProcNames.Add(_T("NexGenUIUD.exe"));
    cstrUIProcNames.Add(_T("PostOfficeUD.exe"));
    cstrUIProcNames.Add(_T("AUCommunicatorUD.exe"));
    cstrUIProcNames.Add(_T("AUCommTest.exe"));
    cstrUIProcNames.Add(_T("DBMaintenanceUD.exe"));
    cstrUIProcNames.Add(_T("InstrumentModelUD.exe"));
    cstrUIProcNames.Add(_T("LISServiceUD.exe"));
    cstrUIProcNames.Add(_T("NSWGSVUD.exe"));
    cstrUIProcNames.Add(_T("ReduceUD.exe"));
    cstrUIProcNames.Add(_T("PoetDev.exe"));
    cstrUIProcNames.Add(_T("PoetAdm.exe"));
    cstrUIProcNames.Add(_T("FastObjectsAdministrator.exe"));
    cstrUIProcNames.Add(_T("FastObjectsDeveloper.exe"));
    cstrUIProcNames.Add(_T("UnicelService.exe"));
    cstrUIProcNames.Add(_T("CIAService.exe"));
    cstrUIProcNames.Add(_T("NexGenDBGen.exe"));
    cstrUIProcNames.Add(_T("NSPRNTUD.exe"));
    cstrUIProcNames.Add(_T("WinHlp32.exe"));	//for our online help
    cstrUIProcNames.Add(_T("DBTestUD.exe"));
    cstrUIProcNames.Add(_T("EventsTestUD.exe"));
    cstrUIProcNames.Add(_T("RestoreUtilUD.exe"));
    cstrUIProcNames.Add(_T("NSUtilUD.exe"));
    cstrUIProcNames.Add(_T("WorkgroupCommTestUD.exe"));
    cstrUIProcNames.Add(_T("LASUtilities.exe"));
    cstrUIProcNames.Add(_T("PtServ32.exe"));
    cstrUIProcNames.Add(_T("IaToMpu.exe"));
    cstrUIProcNames.Add(_T("IaFromMpu.exe"));
    cstrUIProcNames.Add(_T("winvnc.exe"));
    cstrUIProcNames.Add(_T("DXCSimulator.exe"));
	cstrUIProcNames.Add(_T("ResourceMonitorUD.exe"));
	cstrUIProcNames.Add(_T("ScreenTransitionToolUD.exe"));
}

/// ===============================================================================================
/// <summary>This method stops the CIA Service using a created process (command prompt).</summary>
/// <remarks>If it isn't running, that is fine. We do our best to stop it.</remarks>
/// <returns>Returns whether or not we were successful in stopping the CIA Service.</returns>
/// ===============================================================================================
BOOL StopCIAService(void)
{
    // Try to stop the running service
    BOOL bStatus = TRUE;
    TCHAR szPath[BUFFER_SIZE_256];
    GetSystemDirectory(szPath, BUFFER_SIZE_256);
    CString cstrCmdLine;

    cstrCmdLine.Format(CSTR_STOP_CIA_SERVICE, szPath);

    bStatus = CreateProcess(cstrCmdLine);

    if (bStatus == TRUE)
    {
        cstrCmdLine = CSTR_UNREGISTER_CIA_SERVICE;

        bStatus = CreateProcess(cstrCmdLine);
    }

    return bStatus;
}

/// ===============================================================================================
/// <summary>This method stops the UnicelService using a command prompt.</summary>
/// <remarks>If the service isn't running, that is fine. We are doing our best to stop it.</remarks>
/// <returns>Returns whether or not we were successful in stopping the Unicel Service.</returns>
/// ===============================================================================================
BOOL StopUnicelService(void)
{
    BOOL bStatus;
    TCHAR szPath[255];
    GetSystemDirectory(szPath, 255);
    CString cstrCmdLine;

    // Stop Unicel Service
    cstrCmdLine.Format(CSTR_STOP_UNICEL_SERVICE, szPath);

    bStatus = CreateProcess(cstrCmdLine);

    return bStatus;
}

/// ===============================================================================================
/// <summary>This method stops the process associated with the given process ID.</summary>
/// <param name="dwProcID">The process id of the process that will be terminated.</param>
/// <param name="hProcIn">Windows handle of the process that will be terminated.</param>
/// <param name="dwTimeout">Longest period in milliseconds we will wait for termination.</param>
/// <param name="bForce">Determines if we should use extraordinary measures to terminate.</param>
/// <returns>Returns whether or not we were successful in stopping the Unicel Service.</returns>
/// ===============================================================================================
eTA_State TerminateApp(DWORD dwProcID, HANDLE hProcIn, DWORD dwTimeout, BOOL bForce)
{
    HANDLE   hProc = NULL;
    eTA_State   eState;	// return value

    if (hProcIn == NULL)		// we have only Proc ID, need to obtain proc handle
    {
        // If we can't open the process with PROCESS_TERMINATE rights,
        // then we give up immediately.
        hProc = OpenProcess(SYNCHRONIZE | PROCESS_TERMINATE, FALSE, dwProcID);
    }
    else					// use the pass in handle.
    {
        hProc = hProcIn;
    }

    if (hProc == NULL)
    {
        return TA_FAILED;
    }

    // TerminateAppEnum() posts WM_CLOSE to all windows whose PID
    // matches your process's.
    if (!bForce)
    {
        EnumWindows((WNDENUMPROC)TerminateAppEnum, (LPARAM)dwProcID);
    }
    else
    {
        TerminateProcess(hProc, EXIT_CODE);
    }

    // Wait on the handle. If it signals, great. If it times out,
    // then you kill it.
    BOOL bExitLoop = FALSE;
    int nTryCount = 0;

    while (WaitForSingleObject(hProc, dwTimeout) != WAIT_OBJECT_0 && bExitLoop == FALSE)
    {
        // Termiate with exit code 5.
        TerminateProcess(hProc, EXIT_CODE);
        nTryCount++;

        if (nTryCount >= 3)
        {
            bExitLoop = TRUE;
        }
    }

    if (bExitLoop == TRUE)
    {
        eState = TA_FAILED;					// Failed with multi try
    }
    else
    {
        switch (nTryCount)
        {
        case 0:
            if (!bForce)
                eState = TA_SUCCESS_CLEAN;	// WM_CLOSE succeeded 
            else
                eState = TA_SUCCESS_KILL;	// With one shot
            break;
        case 1:
            eState = TA_SUCCESS_KILL;		// Terminate with one try
            break;
        default:
            eState = TA_SUCCESS_MULTI_KILL;	// Terminate with multi try
            break;
        }
    }

    if (hProcIn == NULL)
    {
        // Only if this func own the handle
        CloseHandle(hProc);
    }

    return eState;
}

/// ===============================================================================================
/// <summary>This is a function called by Windows for the EnumWindows function.</summary>
/// <remarks>This is the nice way to stop an application</remarks>
/// <param name="hwnd">This parameter is empty for our purposes.</param>
/// <param name="lParam">This parameter is the process id of the process to end.</param>
/// <returns>Always returns TRUE.</returns>
/// ===============================================================================================
BOOL CALLBACK TerminateAppEnum(HWND hwnd, LPARAM lParam)
{
    DWORD dwID;

    GetWindowThreadProcessId(hwnd, &dwID);

    if (dwID == (DWORD)lParam)
    {
        PostMessage(hwnd, WM_CLOSE, 0, 0);
    }
    return TRUE;
}