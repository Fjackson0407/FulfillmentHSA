/// ===============================================================================================
/// <description>Implementation file for CustomActionCommon</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "CustomActionCommon.h"
#include "SystemServices_i.c"
#include <exception>

/// ===============================================================================================
/// <summary>Returns the AuIp address from the Beckman registry key. If the data does not exist, 
/// return default.</summary>
/// <param name="cstrIPAddress">Returned IP Address of the AU.</param>
/// <param name="handle">handle to the installer process. Needed to put address in memory.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// ===============================================================================================
void GetAuIPAddress(CString& cstrIPAddress, MSIHANDLE handle)
{
    CString cstrMsg;

    // First see if AU IP Address has been discovered and put in memory.
    if (GetAuIPAddressFromInstallerProperty(handle, cstrIPAddress) == FALSE)
    {
		cstrMsg.Format(_T("AuIPAddress Installer Property is not set, need to search for it manually. IP Address: %s"), cstrIPAddress);
		Log(cstrMsg);
		SearchForIPAddress(cstrIPAddress, handle);
	}
}

/// ===============================================================================================
/// <summary>Searches for an IP address and if found, returns it and puts it in memory.</summary>
/// <param name="cstrIPAddress">Returned IP Address of the AU.</param>
/// <param name="handle">handle to the installer process. Needed to put address in memory.</param>
/// ===============================================================================================
void SearchForIPAddress(CString &cstrIPAddress, MSIHANDLE handle)
{
    CString cstrMsg;

    if (PingForAuIPAddress(cstrIPAddress) == TRUE)
    {
        cstrMsg.Format(_T("Setting Msi Property for IPAddress from pinging. IP Address: %s"), cstrIPAddress);
        Log(cstrMsg);
        // We were able to find it by pinging. Put it in memory so subsequent requests can find it easily.
        MsiSetProperty(handle, CSTR_AU_IP_ADDRESS, cstrIPAddress);
    }
    else
    {
        Log("Failed to obtain IP Address by pinging.");
    }
}


/// ===============================================================================================
/// <summary>COM call to return an interface to the AU from the DCOM server.</summary>
/// <param name="handle">handle to the installer process. Needed to put address in memory.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// ===============================================================================================
IUnknown *GetRemoteInterface(MSIHANDLE handle)
{
    IUnknown* pRemoteInterface = NULL;
    CString cstrMsg;
    CString cstrAuIPAddress;
    HRESULT hr = E_FAIL;

    try
    {
        // Get the AU Ip address
        GetAuIPAddress(cstrAuIPAddress, handle);

        // Get the remote interface using DCOM, the return interface will always be IUnknown
        MULTI_QI mqi;
        const IID iID = IID_IUnknown;
        mqi.pIID = &iID;
        mqi.hr = 0;
        mqi.pItf = NULL;
        COSERVERINFO serverName = { 0, cstrAuIPAddress.GetBuffer(), NULL, 0 };
        hr = CoCreateInstanceEx(CLSID_SystemServices, NULL, CLSCTX_REMOTE_SERVER, &serverName, 1, &mqi);

        if (SUCCEEDED(hr))
        {
            pRemoteInterface = mqi.pItf;
        }
        else
        {
            cstrMsg.Format(_T("CoCreateInstanceEx(CLSID_SystemServices... : Unable to retrieve interface to SystemServices. Error Code: %d"), GetLastError());
            Log(cstrMsg);
        }
    }
    catch (std::exception const& e)
    {
        Log("GetRemoteInterface exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("GetRemoteInterface generic exception thrown.");
    }

    return pRemoteInterface;
}

/// ===============================================================================================
/// <summary>Helper function that allows the user to provide a formatted string.</summary>
/// <param name="cstrMessage">Message to go into the log.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// ===============================================================================================
void Log(CString cstrMessage)
{
    CStringA cstrLoggableMsg(cstrMessage);
    WcaLog(LOGMSG_STANDARD, cstrLoggableMsg);
}

/// ===============================================================================================
/// <summary>This method opens a command prompt window with the provided command line.</summary>
/// <param name="cstrCmd">Command line message for generic process.</param>
/// <exception cref="pE">Exceptions are handled by the calling function.</exception>
/// ===============================================================================================
BOOL CreateProcess(CString cstrCmd)
{
    BOOL bStatus = FALSE;
    PROCESS_INFORMATION sProc;
    STARTUPINFO sStartup;

    try
    {
        memset(&sStartup, NULL, sizeof(STARTUPINFO));
        sStartup.cb = sizeof(STARTUPINFO);
        sStartup.dwFlags = STARTF_USESHOWWINDOW;
        sStartup.wShowWindow = SW_HIDE;	// Hide the cmd win
        CString cstrLogMsg;

        // On success, CreateProcess returns non zero value
        bStatus = CreateProcess(NULL, cstrCmd.GetBuffer(), NULL, NULL,
            FALSE, CREATE_DEFAULT_ERROR_MODE | CREATE_UNICODE_ENVIRONMENT | NORMAL_PRIORITY_CLASS,
            NULL, NULL, &sStartup, &sProc);

        if (WaitForSingleObject(sProc.hProcess, TWENTY_THOUSAND) != WAIT_OBJECT_0)
        {
            cstrLogMsg.Format(_T("CreateProcess command: %s, timed out...Error!."), cstrCmd);
            Log(cstrLogMsg);
        }
        else
        {
            DWORD dwExitCode = 19720429;
            GetExitCodeProcess(sProc.hProcess, &dwExitCode);
            cstrLogMsg.Format(_T("CreateProcess command: %s, (Exit Code: %d)"), cstrCmd, dwExitCode);
            Log(cstrLogMsg);
        }
    }
    catch (std::exception const& e)
    {
        Log("CreateProcess exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("CreateProcess generic exception thrown.");
    }

    return bStatus;
}

/// ===============================================================================================
/// <summary>Returns a string to be displayed in the UI based on the current language and the error
/// code.</summary>
/// <param name="hInstall">Handle to the installer. Used to set a property used by the UI.</param>
/// <exception cref="pE">No exception handling in this method.</exception>
/// <returns>Return an error string translated in the user's chosen language.</returns>
/// ===============================================================================================
CString GetErrorString(MSIHANDLE hInstall, int nError)
{
    return GetErrorString(nError, GetInstallerLanguage(hInstall));
}

/// ===============================================================================================
/// <summary>Returns a integer which represents the user's chosen installation language.</summary>
/// <param name="hInstall">The handle to the install process. Needed to get a property.</param>
/// <exception cref="pE">Exceptions are caught and logged.</exception>
/// <returns>Returns the installation language.</returns>
/// ===============================================================================================
int GetInstallerLanguage(MSIHANDLE hInstall)
{
    int nLanguage = LANGUAGE_ENGLISH;
    CString cstrLog;
    DWORD dwBufSize = BUFFER_SIZE_100;
    wchar_t szLanguageValueBuf[BUFFER_SIZE_100];
    char szLanguage[BUFFER_SIZE_100];

    try
    {
        memset(szLanguageValueBuf, CHAR_NULL, BUFFER_SIZE_100);

        // Get version to be installed
        UINT uiStat = MsiGetProperty(hInstall, CSTR_PRODUCT_LANGUAGE, szLanguageValueBuf, &dwBufSize);

		if (uiStat == ERROR_SUCCESS)
        {
            if (0 == WideCharToMultiByte(CP_ACP, WC_COMPOSITECHECK | WC_DEFAULTCHAR, szLanguageValueBuf, -1, szLanguage, BUFFER_SIZE_100 - 1, NULL, NULL))
            {
                Log("WideCharToMultiByte failed.");
            }
            else
            {
                CString cstrLanguage = szLanguageValueBuf;
                nLanguage = atoi(szLanguage);
                cstrLog.Format(_T("UI Language: %s"), cstrLanguage);
                Log(cstrLog);
            }
        }
        else
        {
            cstrLog.Format(_T("MsiGetProperty failed. Error: %d."), uiStat);
            Log(cstrLog);
        }
    }
    catch (std::exception const& e)
    {
        Log("GetInstallerLanguage exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("GetInstallerLanguage generic exception thrown.");
    }

    return nLanguage;
}

/// ===============================================================================================
/// <summary>Finds the IP Address stored in an installation property (in memory).</summary>
/// <param name="hInstall">The handle to the install process. Needed to get the property.</param>
/// <param name="cstrAuIPAddress">The IP address of the AU.</param>
/// <exception cref="pE">Exceptions are caught and logged.</exception>
/// <returns>Returns whether the address was found in memory.</returns>
/// ===============================================================================================
BOOL GetAuIPAddressFromInstallerProperty(MSIHANDLE handle, CString &cstrAuIPAddress)
{
    BOOL bIPAddressInMemory = FALSE;
    CString cstrLog;
    DWORD dwBufSize = BUFFER_SIZE_100;
    wchar_t szAuIPAddressBuf[BUFFER_SIZE_20];
    char szAuIPAddress[BUFFER_SIZE_20];

    try
    {
        memset(szAuIPAddressBuf, CHAR_NULL, BUFFER_SIZE_20);

        // Get version to be installed
        UINT uiStat = MsiGetProperty(handle, CSTR_AU_IP_ADDRESS, szAuIPAddressBuf, &dwBufSize);

		if (uiStat == ERROR_SUCCESS)
        {
            if (0 == WideCharToMultiByte(CP_ACP, WC_COMPOSITECHECK | WC_DEFAULTCHAR, szAuIPAddressBuf, -1, szAuIPAddress, BUFFER_SIZE_100 - 1, NULL, NULL))
            {
                Log("WideCharToMultiByte failed.");
            }
            else
            {
                CString cstrTempIP = szAuIPAddress;
                cstrLog.Format(_T("IPAddressFrom MsiGetProperty: %s"), cstrTempIP);
                Log(cstrLog);

                if (cstrTempIP.GetLength() > 1)
                {
                    bIPAddressInMemory = TRUE;
                    cstrAuIPAddress = cstrTempIP;
                }
            }
        }
        else
        {
            cstrLog.Format(_T("MsiGetProperty failed. Error: %d."), uiStat);
            Log(cstrLog);
        }
    }
    catch (std::exception const& e)
    {
        Log("GetAuIPAddressFromInstallerProperty exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("GetAuIPAddressFromInstallerProperty generic exception thrown.");
    }

    return bIPAddressInMemory;
}

/// ===============================================================================================
/// <summary>Returns a translated error string to be shown in the UI.</summary>
/// <param name="nError">The error code encountered during installation.</param>
/// <param name="nLanguage">The user selected installation language.</param>
/// <exception cref="pE">No exception handling.</exception>
/// <returns>Returns the translated error string.</returns>
/// ===============================================================================================
CString GetErrorString(int nError, int nLanguage)
{
    CString cstrError;

    switch (nError)
    {
    case ERROR_CANT_SEE_AU:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Communication failed between the PC and instrument.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("Error de comunicación entre el PC y el instrumento.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Échec de communication entre l'ordinateur et l'instrument.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Errore di comunicazione tra PC e strumento.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Kommunikation zwischen dem PC dem und Gerät fehlgeschlagen.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("PC 和仪器通信失败。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("PC と装置間で通信エラーです。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("Communication failed between the PC and instrument.\nContact your local service representative for assistance.");
			break;
		}
        break;
    case ERROR_OS_MATCH_FAIL:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("PC and Instrument operating systems do not match.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("Los sistemas operativos del PC y del instrumento no coinciden.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("L'ordinateur et les systèmes d'exploitation de l'instrument ne correspondent pas.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("I sistemi operativi di PC e strumento non corrispondono.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("PC und Gerätebetriebssystem passen nicht zusammen.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("PC 和仪器操作系统不匹配。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("PC と装置のオペレーティング システムが一致しません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("PC and Instrument operating systems do not match.\nContact your local service representative for assistance.");
			break;
        }
        break;
    case ERROR_VERSION:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("System cannot install an older version of software.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("El sistema no puede instalar una versión anterior del software.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Le système ne peut pas installé une version plus ancienne du logiciel.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Il sistema non è in grado di installare una versione precedente del software.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Das System kann keine ältere Version der Software installieren.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("系统不能安装旧版本的软件。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("システムは古いバージョンのソフトウェアをインストールできません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("System cannot install an older version of software.\nContact your local service representative for assistance.");
			break;
        }
		break;
    case ERROR_AU_ACTIVE:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Tests are in progress. Installation cannot proceed.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("Pruebas en curso. La instalación no puede continuar.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Des tests sont en cours. Impossible de procéder à l'installation.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Test in corso. L’installazione non può procedere.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Tests werden verarbeitet. Installation kann nicht fortgesetzt werden.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("检测正在进行中。 无法继续安装。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("測定項目は処理中です。インストレーションは続行できません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("Tests are in progress. Installation cannot proceed.\nContact your local service representative for assistance.");
			break;
        }
        break;
    case ERROR_STOP_AU_FROM_CONSOLE:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Installation software is unable to stop the instrument.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("El software de instalación no puede detener el instrumento.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Il est impossible pour le logiciel d'installation d'arrêter l'instrument.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Il software di installazione non riesce ad arrestare lo strumento.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Gerät kann nicht von der Installationssoftware gestoppt werden.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("安装软件无法停止本仪器。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("インストレーション ソフトウェアは装置を停止できません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("Installation software is unable to stop the instrument.\nContact your local service representative for assistance.");
			break;
        }
		break;
    case ERROR_JAMMED_CRITICAL_EVENTS:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Critical Events are jammed. Installation cannot proceed.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("Eventos críticos bloqueados. La instalación no puede continuar.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Les événements critiques sont bloqués. Impossible de procéder à l'installation.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Eventi critici bloccati. L’installazione non può procedere.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Kritische Ereignisse sind blockiert. Installation kann nicht fortgesetzt werden.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("关键事件被堵塞。 无法继续安装。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("クリティカルイベントが詰まっています。 インストレーションは続行できません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("Critical Events are jammed. Installation cannot proceed.\nContact your local service representative for assistance.");
			break;
        }
        break;
    case ERROR_REBOOT_AU_FAILED:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Installation software is unable to restart the instrument.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("El software de instalación no puede reiniciar el instrumento.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Il est impossible pour le logiciel d'installation de redémarrer l'instrument.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Il software di installazione non riesce a riavviare lo strumento.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Gerät kann nicht von der Installationssoftware neu gestartet werden.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("安装软件无法重新启动本仪器。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("インストレーション ソフトウェアは装置を再起動できません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("Installation software is unable to restart the instrument.\nContact your local service representative for assistance.");
			break;
        }
        break;
    case ERROR_KILL_PROCESSES:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Installation software is unable to stop PC processes.\nContact your local service representative for assistance.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("El software de instalación no puede detener los procesos del PC.\nPóngase en contacto con su representante de servicio local para obtener asistencia.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Il est impossible pour le logiciel d'installation d'interrompre les processus de l'ordinateur.\nContactez votre représentant local pour obtenir de l'aide.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Il software di installazione non riesce ad arrestare i processi del PC.\nContattare il rappresentante di assistenza di zona.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("PC-Prozesse können nicht von der Installationssoftware gestoppt werden.\nWenden Sie sich an Ihren lokalen Kundendienstmitarbeiter, um Unterstützung zu erhalten.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("安装软件无法停止 PC 进程。\n请联系您当地的服务代表寻求帮助。");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("インストレーション ソフトウェアは PC プロセスを停止できません。\nサポートは最寄りのサービス代理店にお問合わせください。");
            break;
		default :
			cstrError = _T("Installation software is unable to stop PC processes.\nContact your local service representative for assistance.");
			break;
        }
        break;
    case ERROR_NO_ERROR:
        switch (nLanguage)
        {
        case LANGUAGE_ENGLISH:
            cstrError = _T("Installation successful.\nPress the ‘Ok’ button. The PC will reboot.");
            break;
        case LANGUAGE_SPANISH:
            cstrError = _T("Installation successful (Spanish).\nPress the ‘Ok’ button. The PC will reboot.");
            break;
        case LANGUAGE_FRENCH:
            cstrError = _T("Installation successful (French).\nPress the ‘Ok’ button. The PC will reboot.");
            break;
        case LANGUAGE_ITALIAN:
            cstrError = _T("Installation successful (Italian).\nPress the ‘Ok’ button. The PC will reboot.");
            break;
        case LANGUAGE_GERMAN:
            cstrError = _T("Installation successful (German).\nPress the ‘Ok’ button. The PC will reboot.");
            break;
        case LANGUAGE_CHINESE:
            cstrError = _T("Installation successful (Chinese).\nPress the ‘Ok’ button. The PC will reboot.");
            break;
        case LANGUAGE_JAPANESE:
            cstrError = _T("Installation successful (Japanese).\nPress the ‘Ok’ button. The PC will reboot.");
            break;
		default :
			cstrError = _T("Installation successful.\nPress the ‘Ok’ button. The PC will reboot.");
			break;
        }
        break;
    default:
        cstrError.Format(_T("Unknown Error: %d"), nError);
    }

    return cstrError;
}

/// ===============================================================================================
/// <summary>Returns whether the given IP Address was pingable.</summary>
/// <param name="cstrIPAddress">The IP Address to ping.</param>
/// <param name="nError">A returned error code.</param>
/// <exception cref="pE">Exceptions are caught and logged.</exception>
/// <returns>Returns the translated error string.</returns>
/// ===============================================================================================
BOOL Ping(CString cstrIPAddress, int &nError)
{
    BOOL bPingable = FALSE;
    char szIpAddress[BUFFER_SIZE_20];
    int nMsTimeout = PING_TIMEOUT;
    WSADATA wsa; 

    try
    {
        CStringA cstrLoggableMsg(cstrIPAddress);

        strcpy_s(szIpAddress, cstrLoggableMsg);

        if (WSAStartup(MAKEWORD(2, 2), &wsa) == 0)
        {
            SOCKET	sockRaw = WSASocket(AF_INET, SOCK_RAW, IPPROTO_ICMP, NULL, 0, 0);
            if (INVALID_SOCKET == sockRaw)
            {
                nError = 2;
                return(FALSE);
            }
            int rc;
            rc = setsockopt(sockRaw, SOL_SOCKET, SO_RCVTIMEO, (char *)&nMsTimeout,
                sizeof(int));
            if (SOCKET_ERROR == rc)
            {
                nError = 3;
                return(FALSE);
            }
            rc = setsockopt(sockRaw, SOL_SOCKET, SO_SNDTIMEO, (char *)&nMsTimeout,
                sizeof(int));
            if (SOCKET_ERROR == rc)
            {
                nError = 4;
                return(FALSE);
            }
            struct sockaddr_in dest;
            memset(&dest, 0, sizeof(sockaddr_in));
            int nAddress = 0;
            nAddress = inet_addr(szIpAddress);
            if (INADDR_NONE == nAddress)
            {
                nError = 5;
                return(FALSE);
            }
            dest.sin_addr.s_addr = nAddress;
            dest.sin_family = AF_INET;

            //
            // Create a PING packet to send
            //
            char		icmpData[MAX_PACKET];

            memset((void *)icmpData, 0, MAX_PACKET);
            ICMPHDR *	pHdr = (ICMPHDR *)&icmpData;
            pHdr->i_cksum = 0;
            pHdr->timestamp = GetTickCount();
            pHdr->i_seq = 0;
            pHdr->i_type = ICMP_ECHO;
            pHdr->i_code = 0;
            pHdr->i_id = (USHORT)GetCurrentProcessId();
            char * pData = (char *)icmpData + sizeof(ICMPHDR);
            memset(pData, 'E', MAX_PACKET - sizeof(ICMPHDR));
            //
            // calculate the checksum
            //
            USHORT usSum = 0;
            USHORT * pUS = (USHORT *)&icmpData;
            int size = 2 * sizeof(ICMPHDR);
            { // scope cksum...
                unsigned long cksum = 0;
#pragma warning( disable : 4244 ) 
                while (size > 1)
                {
                    cksum += *pUS++;
                    size -= sizeof(USHORT);
                }
#pragma warning( default : 4244 ) 
                cksum = (cksum >> 16) + (cksum & 0xffff);
                cksum += (cksum >> 16);
                usSum = (USHORT)(~cksum);
            }

            pHdr->i_cksum = usSum;
            size = 2 * sizeof(ICMPHDR);
            //
            // Send it and wait for reply or timeout
            //
            rc = sendto(sockRaw, (char *)icmpData, size, 0, (struct sockaddr *)&dest, sizeof(dest));

            if (SOCKET_ERROR == rc)
            {
                if (WSAETIMEDOUT == WSAGetLastError())
                {
                    nError = 6;
                    return(FALSE);
                }
                else
                {
                    nError = 7;
                    return(FALSE);
                }
            }

            int nTTWait = nMsTimeout;
            nError = 8;	// TIMEOUT
            while ((nError != 0) && (nTTWait > 0))
            {
                long nBytes = 0;
                if (ioctlsocket(sockRaw, FIONREAD, (u_long *)&nBytes) == 0)
                {
                    nError = 0;
                    if (nBytes == 0)
                    {
                        nError = 8;
                        Sleep(500);
                        nTTWait -= 500;
                    }
                }
            }

            closesocket(sockRaw);
            WSACleanup();

            if (nError == 0)
                bPingable = TRUE;
        }
        else
        {
            nError = 9;
        }
    }
    catch (std::exception const& e)
    {
        Log("Ping exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("Ping generic exception thrown.");
    }

    return bPingable;
}

/// ===============================================================================================
/// <summary>Searches a list of possible AU IP Addresses for the correct one.</summary>
/// <param name="cstrAuIPAddress">The IP address of the AU.</param>
/// <exception cref="pE">Catches and logs exceptions.</exception>
/// <returns>Returns TRUE if the ping to AU is sucessful. Else it returns FALSE.</returns>
/// ===============================================================================================
BOOL PingForAuIPAddress(CString &cstrAuIPAddress)
{
    BOOL bIPFound = FALSE;
    int nError = 0;
    CString cstrMsg;
    CAtlArray<CString> cstrPossibleAuIPAddresses;

    try
    {
        // Load IP Addresses
        LoadIPAddresses(cstrPossibleAuIPAddresses);

        // Iterate through them until we get a hit.
        for (UINT i = 0; i < cstrPossibleAuIPAddresses.GetCount() - 1; i++)
        {
            if (Ping(cstrPossibleAuIPAddresses[i], nError) == TRUE)
            {
                bIPFound = TRUE;
                cstrAuIPAddress = cstrPossibleAuIPAddresses[i];
                break;
            }
            else
            {
                cstrMsg.Format(_T("Ping failed for IP Address %s. Error code %d"), cstrPossibleAuIPAddresses[i], nError);
                Log(cstrMsg);
            }
        }        
    }
    catch (std::exception const& e)
    {
        Log("Ping exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("Ping generic exception thrown.");
    }

    return bIPFound;
}

/// ===============================================================================================
/// <summary>This method copies a hard coded list of process names to the array given.</summary>
/// <remarks>N/A</remarks>
/// <param name="cstrUIProcNames">The list of possible IP Addresses</param>
/// <exception cref="pE">No exception handling.</exception>
/// <returns>No return value. The list of IP Addresses is used by the caller.</returns>
/// ===============================================================================================
void LoadIPAddresses(CAtlArray<CString> &cstrIPAddresses)
{
    cstrIPAddresses.Add(_T("192.168.2.4"));
    cstrIPAddresses.Add(_T("192.168.2.2"));
    cstrIPAddresses.Add(_T("192.168.2.5"));
    cstrIPAddresses.Add(_T("192.168.2.6"));
    cstrIPAddresses.Add(_T("192.168.2.7"));
    cstrIPAddresses.Add(_T("192.168.2.8"));
    cstrIPAddresses.Add(_T("192.168.2.9"));
}

/// ===============================================================================================
/// <summary>This method creates a process that runs "PSExec with a command line param.</summary>
/// <param name="cstrIPAddress">Path to the instrument.</param>
/// <exception cref="pE">If exception thrown, catch, log and release.</exception>
/// <returns>Returning TRUE if we could run the process, else return FALSE.</returns>
/// ===============================================================================================
BOOL SendRebootCommand(CString cstrIPAddress)
{
	BOOL bReboot = FALSE;
	TCHAR szPath[MAX_PATH];

	try
	{
		memset(szPath, CHAR_NULL, MAX_PATH);
		CString cstrCommandLine;

		GetSystemDirectory(szPath, MAX_PATH);
		cstrCommandLine.Format(CSTR_PSEXEC, szPath, cstrIPAddress);

		bReboot = CreateProcess(cstrCommandLine);
	}
	catch (std::exception const& e)
	{
		Log("SendRebootCommand exception thrown.");
		Log(e.what());
	}
	catch (...)
	{
		Log("SendRebootCommand generic exception thrown.");
	}

	return bReboot;
}