/// ===============================================================================================
/// <description>Implementation file for IsAuInActiveMode</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "IsAuInActiveMode.h"
#include "CustomActionCommon.h"
#include <fstream>
#include <exception>

/// ===============================================================================================
/// <summary>Determines if the instrument is or was last in an installable state. This is done
/// by looking through all the Display.txt files on the AU for a known string.</summary>
/// <param name="cstrAuIPAddress">The IP address of the AU.No changes will be made at this time.
/// <exception cref="pE">Catch and Log exceptions.</exception>
/// <returns>Return True if au is or last was in an active mode and false if not.</returns>
/// ===============================================================================================
BOOL IsAuObjectInActiveMode(CString cstrAuIPAddress)
{
    BOOL bIsActive = FALSE;
    CString cstrLog;

    try
    {
        CString cstrVersionFile;
        cstrVersionFile.Format(CSTR_AU_SYSTEMVERSION_TXT, cstrAuIPAddress);

        std::ifstream fileSysVersion;
        
        fileSysVersion.open(cstrVersionFile, std::ifstream::in);
        CString cstrFileToSearch = CSTR_DISPLAY_TXT;

        if (fileSysVersion.is_open())
        {
            char szLine[BUFFER_SIZE_1000];
            memset(szLine, CHAR_NULL, BUFFER_SIZE_1000);
            fileSysVersion.getline(szLine, BUFFER_SIZE_1000);
            CString cstrVersionLine(szLine);
            fileSysVersion.close();

            int nVersion = 0;
            int nVersionMinor = 0;
            int nVersionBranch = 0;
            int nBuild = 0;
            TCHAR cDollarSign = TCHAR_NULL;	// Due to the PVCS automatic replace revision function, we need to customize the way we read the revision num
            if (_stscanf(cstrVersionLine, _T("%cRevision:%d.%d.%d.%d$"), &cDollarSign, &nVersion, &nVersionMinor, &nVersionBranch, &nBuild) < 5)
            {
                cstrLog.Format(_T("DxI Software version (%s) is NOT recognized, IsAUObjInActiveMode return FALSE.... "), cstrVersionLine);
                Log(cstrLog);
                return FALSE;
            }
            cstrLog.Format(_T("DxI Software version is %d.%d.%d.%d.... "), nVersion, nVersionMinor, nVersionBranch, nBuild);
            Log(cstrLog);
        }

        CString cstrPath;
        cstrPath.Format(CSTR_LOG_PATH, cstrAuIPAddress);
        CAtlList<WIN32_FIND_DATA> lstFileNames;

        // Get file name list sorted by create time, ascending
        GetTimeSortedFileNames(cstrPath, cstrFileToSearch, lstFileNames);

        CString cstrFileFullName;
        if (lstFileNames.GetCount())
        {
            POSITION p = lstFileNames.GetTailPosition();	// Latest created log
            BOOL bFound = FALSE;
            while (p && !bFound)
            {
                WIN32_FIND_DATA *pfi = &lstFileNames.GetPrev(p);
                cstrFileFullName.Format(CSTR_PATH_AND_FILE_NAME, cstrPath, pfi->cFileName);

                // File exists
                std::ifstream fileToOpen;
                fileToOpen.open(cstrFileFullName, std::ifstream::in);

                if (fileToOpen.is_open())
                {
                    cstrLog.Format(_T("Searching instrument mode in %s.... "), cstrFileFullName);
                    Log(cstrLog);
                    CAtlArray<CString> cstrLines;

                    // Reverse the order
                    char szLine[BUFFER_SIZE_1000];
                    memset(szLine, CHAR_NULL, BUFFER_SIZE_1000);
                    while (fileToOpen.getline(szLine, BUFFER_SIZE_1000))
                    {
                        CString cstrLine(szLine);
                        cstrLines.InsertAt(0, cstrLine);
                    }

                    fileToOpen.close();

                    for (UINT i = 0; i < cstrLines.GetCount(); i++)
                    {
                        CString cstrLine;
                        cstrLine = cstrLines[i];
                        int nPos = cstrLine.Find(DISPLAY_TO_FIND1);
                        CString cstrMode = _T("");

                        if (nPos != -1)
                        {
                            // Found it
                            int nStartPos = cstrLine.Find(_T(")"), nPos);
                            int nEndPos = cstrLine.Find(_T("("), nStartPos);
                            Assert(nStartPos != -1 && nEndPos != -1);
                            cstrMode = cstrLine.Mid(nStartPos + 1, nEndPos - nStartPos - 1);
                            cstrMode.TrimLeft();
                            cstrMode.TrimRight();
                            cstrLog.Format(_T("Found instrument mode(%s) in %s.... "), cstrMode, cstrFileFullName);
                            Log(cstrLog);
                            bFound = TRUE;
                        }
                        else
                        {
                            // Try another string.
                            nPos = cstrLine.Find(DISPLAY_TO_FIND2);
                            if (nPos != -1)
                            {
                                // Found it
                                int nOffset = _tcslen(DISPLAY_TO_FIND2);
                                int nStartPos = nPos + nOffset;
                                int nEndPos = cstrLine.Find(_T("by"), nStartPos);
                                Assert(nStartPos != -1 && nEndPos != -1);
                                cstrMode = cstrLine.Mid(nStartPos + 1, nEndPos - nStartPos - 1);
                                cstrMode.TrimLeft();
                                cstrMode.TrimRight();
                                cstrLog.Format(_T("Found instrument mode(%s) in %s.... "), cstrMode, cstrFileFullName);
                                Log(cstrLog);
                                bFound = TRUE;
                            }
                        }

                        if (bFound)
                        {
                            if (cstrMode == CSTR_NOT_READY ||
                                cstrMode == CSTR_READY ||
                                cstrMode == CSTR_INITIALIZING ||
                                cstrMode == CSTR_DIAGNOSTICS)
                            {
                                // The instrument is not in an Active mode.
                                bIsActive = FALSE;
                            }
                            else
                            {
                                // The instrument IS in an Active mode.
                                bIsActive = TRUE;
                            }

                            cstrLog.Format(_T("Instrument mode is %s. IsAUObjInActiveMode return %d"), cstrMode, bIsActive);
                            Log(cstrLog);
                            break;
                        }
                    }
                }
                else
                {
                    // Can't open file
                    cstrLog.Format(_T("Can't open(Err:%d) %s, maybe a new build. IsAuObjInActiveMode return FALSE.... "),
                        GetLastError(), cstrFileFullName);
                    Log(cstrLog);
                }
            }	// end of while(p&&!bFound)
        }	// End of lstFileNames.GetCount()
    }
    catch (std::exception const& e)
    {
        Log("IsAuObjectInActiveMode exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("IsAuObjectInActiveMode generic exception thrown.");
    }

    return bIsActive;
}

/// ===============================================================================================
/// <summary>Returns a time sorted list of file names from the AU.</summary>
/// <param name="szPath">File path</param>
/// <param name="szFileName">File name</param>
/// <param name="lstNames">A time sorted list of names is passed by reference</param>
/// ===============================================================================================
void GetTimeSortedFileNames(LPCTSTR szPath, LPCTSTR szFileName, CAtlList<WIN32_FIND_DATA>& lstNames)
{
    WIN32_FIND_DATA FileData;
    BOOL bFinished = FALSE;
    CString cstrMsg;

    // Find first file
    CString str;
    str.Format(CSTR_PATH_AND_FILE_NAME, szPath, szFileName);
    HANDLE hSearch = FindFirstFile(str, &FileData);

    if (hSearch != INVALID_HANDLE_VALUE)
    {
        while (!bFinished && hSearch != INVALID_HANDLE_VALUE)
        {
            BOOL bAdded = FALSE;
            if (lstNames.GetCount())
            {
                POSITION p = lstNames.GetHeadPosition();
                while (p)
                {
                    POSITION lastp = p;
                    WIN32_FIND_DATA *pfi = &lstNames.GetNext(p);
                    if (CompareFileTime(&FileData.ftLastWriteTime, &pfi->ftLastWriteTime) < 0)
                    {
                        lstNames.InsertBefore(lastp, FileData);
                        bAdded = TRUE;
                        break;
                    }
                }
            }
            if (!bAdded)
            {
                lstNames.AddTail(FileData);
            }

            if (!FindNextFile(hSearch, &FileData))
            {
                if (GetLastError() == ERROR_NO_MORE_FILES)
                {
                    bFinished = TRUE;
                }
                else
                {
                    cstrMsg.Format(_T("FindNextFile return value: %d"), GetLastError());
                    Log(cstrMsg);
                    Assert(FALSE);
                }
            }
        } // end of while

        FindClose(hSearch);
        hSearch = INVALID_HANDLE_VALUE;
    }
}