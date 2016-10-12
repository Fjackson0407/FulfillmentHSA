// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
#define _CRT_SECURE_NO_WARNINGS
// Windows Header Files:
#include <windows.h>

#include <msiquery.h>
#include "atlstr.h"
#include "atlcoll.h"
#include <strsafe.h>
// WiX Header Files:
#include <wcautil.h>

//After the Au sucessfully installs, AuInstall.txt is dropped into the CSTR_DROPFILE_PATH
//The console will look for this file to know the AU install is complete.
const CString  CSTR_DROPFILE_PATH = _T("c:\\AuInstall.txt");
const char *    const   SZ_SUCCESS = "success";