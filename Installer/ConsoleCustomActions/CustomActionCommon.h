/// ===============================================================================================
/// <description>Header file for CustomActionCommon</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "Unknwnbase.h"

#if !defined(CUSTOMCOMMONACTIONS__INCLUDED_)
#define CUSTOMCOMMONACTIONS__INCLUDED_

#define MAXIMUM_LINE_LENGTH 256

// Methods
BOOL CreateProcess(CString cstrCmd);
void GetAuIPAddress(CString& szAuIPAddress, MSIHANDLE handle);
BOOL GetAuIPAddressFromInstallerProperty(MSIHANDLE handle, CString &cstrIP);
CString GetErrorString(int nError, int nLanguage);
CString GetErrorString(MSIHANDLE handle, int nError);
int GetInstallerLanguage(MSIHANDLE handle);
IUnknown* GetRemoteInterface(MSIHANDLE handle);
void LoadIPAddresses(CAtlArray<CString> &cstrIPAddresses);
void Log(CString cstrMessage);
BOOL Ping(CString cstrIPAddress, int &nError);
BOOL PingForAuIPAddress(CString &cstrIP);
void SearchForIPAddress(CString &cstrIPAddress, MSIHANDLE handle);
BOOL SendRebootCommand(CString cstrIPAddress);

// Constants
const int		BUFFER_SIZE_20 = 20;
const int		BUFFER_SIZE_100 = 100;
const int       BUFFER_SIZE_256 = 256;
const int		BUFFER_SIZE_1000 = 1000;
const int       BUFFER_SIZE_1024 = 1024;
const int       CE_STORAGE_SIZE = 10485760;
const int       TWENTY_THOUSAND = 20000;
const int       ONE_THOUSAND = 1000;
const int       MAX_ITERATIONS = 600;
const int       MAGIC_VERSION_NUM = 1103;
const int       INT_COMPARE_LENGTH = 6;
const int       TERMINATE_APP_TIMEOUT = 2000;
const int       EXIT_CODE = 5;
const HRESULT   CA_FAIL = -1;
const int       ICMP_ECHO = 8;
const int       MAX_PACKET = 1024;
const int       PING_TIMEOUT = 4000;

const char	    CHAR_NULL = '\0';
const TCHAR     TCHAR_NULL = _T('\0');
const char *    const   SZ_SUCCESS = "success";
const char *    const   SZ_FAIL = "fail";
const wchar_t * const   SZ_DXI_INSTALL = _T("DxI Install");
const wchar_t * const   SZ_MSI_INSTALL = _T("msiexec /i c:\\AUInstaller.msi /l*v c:\\AUInstallLog.txt");
const wchar_t * const   SZ_NEW_CONSOLE_IP_ADDRESS = _T("192.168.2.3");

const CString  CSTR_CONSOLE_PC_NAME = _T("CONSOLE_PC");
const CString  CSTR_LOG_PATH = _T("\\\\%s\\c$\\au\\log\\");
const CString  CSTR_AUINSTALL_TXT = _T("\\\\%s\\C$\\AuInstall.txt");
const CString  CSTR_AU_IP = _T("AuIp");
const CString  CSTR_AUSYSTEMVERSION_TXT = _T("\\C$\\Au\\SystemVersion.txt");
const CString  CSTR_AU_SYSTEMVERSION_TXT = _T("\\\\%s\\c$\\au\\systemversion.txt");
const CString  CSTR_CE_MAGIC_STR = _T("Last CE ID:");
const CString  CSTR_COM_CE_STORAGE = _T("COMM.CEStorage");
const CString  CSTR_COM_CE_DAT = _T("\\\\%s\\C$\\AU\\CommCE.dat");
const CString  CSTR_CONSOLE_SYSTEMVERSION_TXT = _T("C:\\NexGen\\Console\\SystemVersion.txt");
const CString  CSTR_PRODUCT_VERSION = _T("ProductVersion");
const CString  CSTR_PRODUCT_LANGUAGE = _T("ASSISTANCE_LANGUAGE_NAME");
const CString  CSTR_DOUBLE_BACKSLASH = _T("\\\\");
const CString  CSTR_INSTALL_VERSION_TXT = _T("C:\\SystemVersion.txt");
const CString  CSTR_STOP_CIA_SERVICE = _T("%s\\net.exe stop CIAService");
const CString  CSTR_UNREGISTER_CIA_SERVICE = _T("C:\\Program Files\\Beckman Coulter Inc\\RMS CIA\\bin\\ciaservice.exe -unregserver");
const CString  CSTR_STOP_UNICEL_SERVICE = _T("%s\\net.exe stop UnicelService");
const CString  CSTR_WIN_INI = _T("\\\\%s\\c$\\Windows\\win.ini");
const CString  CSTR_WORKGROUP_KEY = _T("SOFTWARE\\Beckman\\NexStep\\Workgroup");
const CString  CSTR_RUN_ONCE = _T("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce");
const CString  CSTR_REMOTE_MACHINE_NAME = _T("\\\\%s");
const CString  CSTR_COPY_MSI = _T("copy c:\\ImageVersionTest.txt \\\\%s\\C$\\");
const CString  CSTR_PSEXEC = _T("%s\\shutdown -m \\\\%s -r -f -t 02");
const CString  CSTR_ERROR_CODE = _T("ERROR_CODE");
const CString  CSTR_DISPLAY_TXT = _T("Display*.txt");
const CString  CSTR_PATH_AND_FILE_NAME = _T("%s%s");
const CString  CSTR_NEXGEN_UI = _T("NexGenUIUD.exe");
const CString  CSTR_NOT_READY = _T("Not Ready");
const CString  CSTR_READY = _T("Ready");
const CString  CSTR_INITIALIZING = _T("Initializing");
const CString  CSTR_DIAGNOSTICS = _T("Diagnostics");
const CString  CSTR_AU_IP_ADDRESS = _T("AU_IP_ADDRESS");

// These adapter registry keys are for a silver face PC with Windows XP.
const CString  CSTR_XP_CONSOLE_LEGACY_NETWORK_CARD_KEY = _T("SYSTEM\\CurrentControlSet\\Services\\{218982DF-7A13-4344-9424-57064D86FD6B}\\Parameters\\Tcpip");
const CString  CSTR_XP_CONSOLE_REAL_NETWORK_CARD_KEY = _T("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\{218982DF-7A13-4344-9424-57064D86FD6B}");

// VM Values
//const CString  CSTR_XP_CONSOLE_LEGACY_NETWORK_CARD_KEY = _T("SYSTEM\\CurrentControlSet\\Services\\{1208EACC-CB65-45CB-A153-87DFC07C129B}\\Parameters\\Tcpip");
//const CString  CSTR_XP_CONSOLE_REAL_NETWORK_CARD_KEY = _T("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\{1208EACC-CB65-45CB-A153-87DFC07C129B}");

const CString  CSTR_IP_ADDRESS = _T("IPAddress");
const CString  CSTR_BASE_IP_ADDRESS = _T("192.168.2.1");

const int ERROR_NO_ERROR = 900;
const int ERROR_KILL_PROCESSES = 800;
const int ERROR_REBOOT_AU_FAILED = 700;
const int ERROR_JAMMED_CRITICAL_EVENTS = 600;
const int ERROR_STOP_AU_FROM_CONSOLE = 500;
const int ERROR_AU_ACTIVE = 400;
const int ERROR_VERSION = 300;
const int ERROR_OS_MATCH_FAIL = 200;
const int ERROR_CANT_SEE_AU = 100;

const DWORD XP_MAJOR = 5;
const DWORD XP_MINOR = 1;
const DWORD WIN8_MAJOR = 6;
const DWORD WIN8_MINOR = 2;
const int CONST_TWO = 2;
const int CONST_THREE = 3;
const int CONST_FOUR = 4;

const DWORD WORKSTATION_INFO = 102;

const int LANGUAGE_ENGLISH = 1033;
const int LANGUAGE_SPANISH = 3082;
const int LANGUAGE_FRENCH = 1036;
const int LANGUAGE_ITALIAN = 1040;
const int LANGUAGE_GERMAN = 1031;
const int LANGUAGE_CHINESE = 2052;
const int LANGUAGE_JAPANESE = 1041;

// Enumerations
enum	eTA_State{
    TA_NOT_FOUND,
    TA_FAILED,
    TA_SUCCESS_CLEAN,
    TA_SUCCESS_KILL,
    TA_SUCCESS_MULTI_KILL
};

typedef struct _tagIcmpHdr
{
    BYTE			i_type;
    BYTE			i_code;
    unsigned short	i_cksum;
    unsigned short	i_id;
    unsigned short	i_seq;
    unsigned long	timestamp;
} ICMPHDR;

#endif