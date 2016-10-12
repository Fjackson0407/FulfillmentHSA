/// ===============================================================================================
/// <description>Header file for IsVersionNumbersOk</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"

#if !defined(ISVERSIONNUMBERS__INCLUDED_)
#define ISVERSIONNUMBERS__INCLUDED_

BOOL AreVersionNumbersOk(char *szInstallVersion, CString cstrIPAddress);
CString GetVersionNumber(CString szPath);
BOOL IsVersionLess(CString szInstallVersion, CString szExistingVersion);
BOOL InstallerVersionIsGreaterThanExistingVersion(int MajorIV, int MinorIV, int BranchIV, int BuildIV, int MajorEV, int MinorEV, int BranchEV, int BuildEV);
void BUnicodeToAnsi(LPSTR szAnsiDest, LPCWSTR szUnicodeSource, const short nChars);
void BAnsiToUnicode(LPWSTR szUnicodeDest, LPCSTR szAnsiSource, const short nChars);
#endif