/// ===============================================================================================
/// <description>Implementation file for IsAuCriticalEventsJammed</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "IsAuCriticalEventsJammed.h"
#include "CustomActionCommon.h"
#include <exception>

/// ===============================================================================================
/// <summary>This method determines if the instrument has any unsent critical events.</summary>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning TRUE if there are jammed events, else return FALSE.</returns>
/// ===============================================================================================
BOOL AreAuCriticalEventsJammed(CString cstrIPAddress)
{
    BOOL bAuEventsJammed = FALSE;

    try
    {
        CString cstrPathToComCE;
        cstrPathToComCE.Format(CSTR_COM_CE_DAT, cstrIPAddress);

        HANDLE hCEFile = INVALID_HANDLE_VALUE;

        hCEFile = CreateFile(cstrPathToComCE,
            GENERIC_READ,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            NULL,
            OPEN_EXISTING,
            FILE_ATTRIBUTE_NORMAL,
            NULL);

        if (INVALID_HANDLE_VALUE != hCEFile)
        {
            HANDLE hCEFileMapping = INVALID_HANDLE_VALUE;

            hCEFileMapping = CreateFileMapping(hCEFile, NULL, PAGE_READONLY, 0, CE_STORAGE_SIZE, CSTR_COM_CE_STORAGE);

            if (INVALID_HANDLE_VALUE != hCEFileMapping)
            {
                BYTE* pData = NULL;
                pData = (BYTE *)MapViewOfFile(hCEFileMapping, FILE_MAP_READ, 0, 0, 0);

                if (NULL != pData)
                {
                    // Read the magin string
                    wchar_t szCEMagic[BUFFER_SIZE_256];
                    int nSize = wcslen(CSTR_CE_MAGIC_STR) * sizeof(wchar_t);
                    memcpy(szCEMagic, pData, nSize);

                    if (0 == memcmp(CSTR_CE_MAGIC_STR, szCEMagic, nSize))
                    {
                        long lLastSendCE = 0L;
                        long lHead = 0L;
                        long lTail = 0L;
                        long lUsage = 0L;
                        long lMargin = 0L;

                        // Read parameters
                        memcpy(&lLastSendCE, &pData[nSize], sizeof(long));
                        memcpy(&lHead, &pData[nSize + sizeof(long)], sizeof(long));
                        memcpy(&lTail, &pData[nSize + CONST_TWO * sizeof(long)], sizeof(long));
                        memcpy(&lUsage, &pData[nSize + CONST_THREE * sizeof(long)], sizeof(long));
                        memcpy(&lMargin, &pData[nSize + CONST_FOUR * sizeof(long)], sizeof(long));

                        if (lUsage > 0)
                        {
                            Log("CE Jammed!");
                            bAuEventsJammed = TRUE;
                        }
                        else
                        {
                            // Due to Release 1.0 Comm issue, there is a possibility that usage is negative even though no CEs jammed. 
                            // In this case, we use head and tail to determine
                            if (lHead == lTail)
                            {
                                Log("CE Not Jammed!");
                            }
                            else
                            {
                                Log("CE Jammed (Release 1.0 Comm)!");
                                bAuEventsJammed = TRUE;
                            }
                        }
                    }
                    else
                    {
                        // Can't not find the magic str, it may be a new CE file
                        Log("Can't find magic string in CE file.");
                    }

                    UnmapViewOfFile(pData);
                    pData = NULL;
                }

                CloseHandle(hCEFileMapping);
                hCEFileMapping = NULL;
            }

            CloseHandle(hCEFile);
            hCEFile = NULL;
        }
        else
        {
            Log("CE File not found.");
        }
    }
    catch (std::exception const& e)
    {
        Log("AreAuCriticalEventsJammed exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("AreAuCriticalEventsJammed generic exception thrown.");
    }

    return bAuEventsJammed;
}