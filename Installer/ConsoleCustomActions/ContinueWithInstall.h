/// ===============================================================================================
/// <description>Header file for ContinueWithInstall</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"

BOOL ContinueWithTheInstall(MSIHANDLE hInstall);
BOOL CheckAuInstallStatus(MSIHANDLE hInstall, char *szInstallVersion);
BOOL CheckConsoleInstallStatus(MSIHANDLE hInstall, char *szInstallVersion);
BOOL GetInstallVersion(MSIHANDLE handle, char *szInstallVersion);
BOOL InstallFileExists(CString fullPath);
BOOL StopAuFromConsole(MSIHANDLE handle);