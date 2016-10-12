/// ===============================================================================================
/// <description>Header file for KillConsoleProcesses</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
BOOL KillConsoleProcesses(void);
BOOL KillProcesses(void);
void LoadUIProcesses(CAtlArray<CString> &cstrUIProcNames);
BOOL StopUnicelService(void);
BOOL StopCIAService(void);
eTA_State TerminateApp(DWORD dwProcID, HANDLE hProcIn, DWORD dwTimeout, BOOL bForce);
BOOL CALLBACK TerminateAppEnum(HWND hwnd, LPARAM lParam);