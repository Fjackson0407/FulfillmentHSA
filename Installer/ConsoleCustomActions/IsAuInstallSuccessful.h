/// ===============================================================================================
/// <description>Header file for IsAuInstallSuccessful</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"

BOOL WasAuInstallSuccessful(CString &cstrErrorCode, MSIHANDLE handle);
BOOL IsInstallFinished(CString szPath, char *failureInformation);

