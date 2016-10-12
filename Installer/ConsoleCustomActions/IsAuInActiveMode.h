/// ===============================================================================================
/// <description>Header file for IsAuInActiveMode</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"

#define DISPLAY_TO_FIND1	_T("ChangeInstrumentMode")
#define DISPLAY_TO_FIND2	_T("Mode was changed to")

BOOL IsAuObjectInActiveMode(CString cstrAuIPAddress);
void GetTimeSortedFileNames(LPCTSTR szPath, LPCTSTR szFileName, CAtlList<WIN32_FIND_DATA>& lstNames);