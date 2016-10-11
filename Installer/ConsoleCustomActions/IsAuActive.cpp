/// ===============================================================================================
/// <description>Implementation file for IsAuActive</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "CustomActionCommon.h"
#include "SystemServices.h"
#include <exception>

/// ===============================================================================================
/// <summary>We ask the AU operating system if auobj.exe is a running process.</summary>
/// <param name="hInstall">Handle to the install process. Given to us by installer.</param>
/// <exception cref="pE">Catch all exceptions and log. Return error if exception thrown</exception>
/// <returns>Returning SUCCESS if we could perform all operations, else return ERROR.</returns>
/// ===============================================================================================
UINT _stdcall IsAuActive(MSIHANDLE hInstall)
{
    HRESULT hr = S_OK;
    UINT er = ERROR_SUCCESS;

    hr = WcaInitialize(hInstall, "IsAuActive");
    ExitOnFailure(hr, "Failed to initialize");

    Log("Start");

    try
    {
        CoInitialize(NULL);

        hr = S_FALSE;
        IUnknown *pUnk = NULL;
        pUnk = GetRemoteInterface(hInstall);

        if (NULL != pUnk)
        {
            ISystemServices* pUnicelService = NULL;
            Log("IsAuActive calling pUnk->QueryInterface for UnicelService.");
            HRESULT hrUnicel = pUnk->QueryInterface(IID_ISystemServices, (void**)&pUnicelService);

            if ((hrUnicel == S_OK) && (pUnicelService != NULL))
            {
                // Check whether AU is running
                BOOL bAlive = TRUE;
                Log("Calling pUnicelService->IsAuObjAlive.");
                hrUnicel = pUnicelService->IsAuObjAlive(&bAlive);

				if (hrUnicel !=  S_OK)
                {
                    Log("Call to IsAuObjAlive failed.");
                }
                else
                {
                    Log("Call to pUnicelService->IsAuObjAlive worked somehow.");
                    if (TRUE == bAlive)
                    {
                        hr = S_OK;
                        Log("Au is active.");
                    }
                    else
                    {
                        Log("Au is not active.");
                    }

                    pUnicelService->Release();
                }
            }
            else
            {
                Log("Unable obtain interface to UnicelService.");
            }

            pUnk->Release();
        }

        CoUninitialize();
    }
    catch (std::exception const& e)
    {
        Log("IsAuActive exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("IsAuActive generic exception thrown.");
    }

    Log("End");
LExit:
    er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
    return WcaFinalize(er);
}

