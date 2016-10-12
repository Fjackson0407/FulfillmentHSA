/// ===============================================================================================
/// <description>Implementation file for IsVersionNumbersOk</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "IsVersionNumbersOk.h"
#include "CustomActionCommon.h"
#include "winnls.h"
#include <exception>

/// ===============================================================================================
/// <summary>This method determines if the current version of the software found in the 
/// SystemVersion.txt file is less than the version we are trying to install.</summary>
/// <param name="szInstallVersion">The version of software we are attempting to install.</param>
/// <param name="cstrVersionFilePath">The file path for the current SystemVersion.txt file on the system.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning TRUE if version numbers are OK, else return FALSE.</returns>
/// ===============================================================================================
BOOL AreVersionNumbersOk(char *szInstallVersion, CString cstrVersionFilePath)
{
	CString cstrMsg;
	BOOL bAreVersionNumbersOk = FALSE;
	CString InstallerSoftwareVersion = szInstallVersion;
	CString CurrentSoftwareVersion = "";

	try
	{
		cstrMsg.Format(_T("AreVersionNumbersOk: InstallerSoftwareVersion %s"), InstallerSoftwareVersion);
		Log(cstrMsg);

		//Get Software Version
		CurrentSoftwareVersion = GetVersionNumber(cstrVersionFilePath);


		//InstallerSoftwareVersion must be higher than the Current Software Versions.
		if (IsVersionLess(InstallerSoftwareVersion, CurrentSoftwareVersion) == FALSE)
		{
			bAreVersionNumbersOk = TRUE;
			cstrMsg.Format(_T("%s Version Numbers are ok (InstallerSoftwareVersion: %s >= Current Software Version: %s)"), cstrVersionFilePath, InstallerSoftwareVersion, CurrentSoftwareVersion);
			Log(cstrMsg);
		}
		else
		{
			cstrMsg.Format(_T("%s Version Numbers are NOT ok (InstallerSoftwareVersion: %s < Current Software Version: %s)"), cstrVersionFilePath, InstallerSoftwareVersion, CurrentSoftwareVersion);
			Log(cstrMsg);
		}
	}
	catch (std::exception const& e)
	{
		Log("AreVersionNumbersOk exception thrown.");
		Log(e.what());
	}
	catch (...)
	{
		Log("AreVersionNumbersOk generic exception thrown.");
	}

	return bAreVersionNumbersOk;
}

/// ===============================================================================================
/// <summary> Opens a file at a given path and extracts the current software version.</summary>
/// <param name="szPath">Path to systemversion.txt on Console or AU.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>CString of the extracted version number</returns>
/// ===============================================================================================
CString GetVersionNumber(CString szPath)
{
	CString cstrVersionNumber = _T("unknown");
	
	try
	{
		// Open the file.
		char            fileData[MAXIMUM_LINE_LENGTH];
		FILE*           fp;
		char            szFilePath[MAXIMUM_LINE_LENGTH];
		CString         cstrTemp(szPath);
		CString         cstrRevision;

#ifdef _UNICODE
		BUnicodeToAnsi(szFilePath, cstrTemp.GetBuffer(cstrTemp.GetLength()), MAXIMUM_LINE_LENGTH);
#else
		strcpy(szFilePath, cstrTemp.GetBuffer(cstrTemp.GetLength()));
#endif

		fp = fopen(szFilePath, "r");

		if (fp)
		{
			// Read the revision from the first line. (It's ANSI text.)
			if (fgets(fileData, MAXIMUM_LINE_LENGTH, fp) != NULL)
			{
#ifdef _UNICODE
				TCHAR   szRevision[MAXIMUM_LINE_LENGTH + 1];
				BAnsiToUnicode(szRevision, fileData, MAXIMUM_LINE_LENGTH);
				cstrRevision = szRevision;
#else
				cstrRevision = fileData;
#endif
				// Strip the trailing '$'.
				int i = cstrRevision.GetLength() - 1;
				i = cstrRevision.ReverseFind(TCHAR('$'));
				if (-1 != i)
				{
					cstrRevision = cstrRevision.Left(i);
				}
				// Strip the PVCS keyword off the front.
				i = cstrRevision.Find(TCHAR(':'));
				if (i != -1)
				{
					cstrRevision = cstrRevision.Mid(++i);
				}
				// Remove any spaces and end of line markers.
				cstrRevision.TrimLeft();
				cstrRevision.TrimRight();
				cstrVersionNumber = cstrRevision;
			}
			fclose(fp);
		}
	}
	catch (std::exception const& e)
	{
		Log("GetVersionNumber exception thrown.");
		Log(e.what());
	}
	catch (...)
	{
		Log("GetVersionNumber generic exception thrown.");
	}

	return cstrVersionNumber;
}

/// ===============================================================================================
/// <summary>Compares two software version strings by evaluating their major, minor, branch, 
/// and build numbers.</summary>
/// <param name="szInstallVersion">The version of software we are attempting to install.</param>
/// <param name="szExistingVersion">The version of software that is currently installed.</param>
/// <returns>Returning TRUE if the version to be installed is less than the current version of 
/// both the console and the AU, else return FALSE.</returns>
/// ===============================================================================================
BOOL IsVersionLess(CString szInstallVersion, CString szExistingVersion)
{
    BOOL bIsVersionLess = FALSE;
	
	// Extract the Install Version (IV) of the string
	int MajorIV = 0;
	int MinorIV = 0;
	int BranchIV = 0; 
	int BuildIV = 0;

	_stscanf(szInstallVersion, _T("%d.%d.%d.%d"), &MajorIV, &MinorIV, &BranchIV, &BuildIV);

	// Extract the Existing Version (EV) of the string
	int MajorEV = 0;
	int MinorEV = 0;
	int BranchEV = 0;
	int BuildEV = 0;

	_stscanf(szExistingVersion, _T("%d.%d.%d.%d"), &MajorEV, &MinorEV, &BranchEV, &BuildEV);
	
	bIsVersionLess = InstallerVersionIsGreaterThanExistingVersion(MajorIV, MinorIV, BranchIV, BuildIV,
		MajorEV, MinorEV, BranchEV, BuildEV);

    return bIsVersionLess;
}

/// ===============================================================================================
/// <summary>Determines if the version to be installed is greater than the version that is 
/// currently installed.</summary>
/// <param name="MajorIV">Left most integer of the version to be installed.</param>
/// <param name="MinorIV">Second integer from the left of the version to be installed.</param>
/// <param name="BranchIV">Third integer from the left of the version to be installed.</param>
/// <param name="BuildIV">Right most integer of the version to be installed.</param>
/// <param name="MajorEV">Left most integer of the version currently installed.</param>
/// <param name="MinorEV">Second integer from the left of the version currently installed.</param>
/// <param name="BranchEV">Third integer from the left of the version currently installed.</param>
/// <param name="BuildEV">Right most integer of the version currently installed.</param>
/// <returns>Returning TRUE if the version to be installed is less than the current version, 
/// else return FALSE.</returns>
/// ===============================================================================================
BOOL InstallerVersionIsGreaterThanExistingVersion(int MajorIV, int MinorIV, int BranchIV, int BuildIV, 
	int MajorEV, int MinorEV, int BranchEV, int BuildEV)
{
	if (MajorIV < MajorEV)
	{
		return true;
	}
	if (MajorIV > MajorEV)
	{
		return false;
	}
	if (MinorIV < MinorEV)
	{
		return true;
	}
	if (MinorIV > MinorEV)
	{
		return false;
	}
	if (BranchIV < BranchEV)
	{
		return true;
	}
	if (BranchIV > BranchEV)
	{
		return false;
	}
	if (BuildIV < BuildEV)
	{
		return true;
	}
	if (BuildIV > BuildEV)
	{
		return false;
	}

	// default value.  Version numbers are the same.
	return false;
}

/// ===============================================================================================
/// <summary>BUnicodeToAnsi(LPSTR szAnsiDest, LPCWSTR szUnicodeSource, const short nChars)
/// Converts a Unicode string to an ANSI string.Include the NULL
/// terminator in the count of characters.< /summary>
/// <param name="szAnsiDest">LPSTR source path of the SystemVersion.txt file</param>
/// <param name="szUnicodeSource">LPCWSTR source path of the SystemVersion.txt file</param>
/// <param name="nChars">The maximum line length.</param>
/// ===============================================================================================
void BUnicodeToAnsi(LPSTR szAnsiDest, LPCWSTR szUnicodeSource, const short nChars)
{
	int     iUnicodeConversion;

	Assert(szUnicodeSource != NULL);

	iUnicodeConversion = ::WideCharToMultiByte(CP_ACP, WC_COMPOSITECHECK | WC_DEFAULTCHAR,
		szUnicodeSource, -1, szAnsiDest, nChars,
		NULL, NULL);

	if (0 == iUnicodeConversion)
	{
		// The error code can be one of the following:
		// ERROR_INSUFFICIENT_BUFFER
		// ERROR_INVALID_FLAGS
		// ERROR_INVALID_PARAMETER
		DWORD dwErr;
		dwErr = GetLastError();
	}
	Assert(iUnicodeConversion > 0);
}

/// ===============================================================================================
/// <summary>BAnsiToUnicode(LPWSTR szUnicodeDest, LPCSTR szAnsiSource, const short nChars)
/// Converts an ANSI string to a Unicode string.Include the NULL
/// terminator in the count of characters.< / summary>
/// <param name="szUnicodeDest">LPWSTR full version number</param>
/// <param name="szAnsiSource">LPCSTR full version number</param>
/// <param name="nChars">The maximum line length.</param>
/// ===============================================================================================
void BAnsiToUnicode(LPWSTR szUnicodeDest, LPCSTR szAnsiSource, const short nChars)
{
	int     iUnicodeConversion;

	Assert(szAnsiSource != NULL);

	iUnicodeConversion = ::MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED,
		szAnsiSource, -1, szUnicodeDest, nChars);

	if (0 == iUnicodeConversion)
	{
		// The error code can be one of the following:
		// ERROR_INSUFFICIENT_BUFFER
		// ERROR_INVALID_FLAGS
		// ERROR_INVALID_PARAMETER
		// ERROR_NO_UNICODE_TRANSLATION
		DWORD dwErr;
		dwErr = GetLastError();
	}
	Assert(iUnicodeConversion > 0);

}