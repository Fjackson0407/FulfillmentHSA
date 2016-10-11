/// ===============================================================================================
/// <description>Implementation file for StopAuFromConsole</description>
/// <legal>Copyright (C) 2016 Beckman Coulter, Inc., Chaska, MN, USA All Rights Reserved</legal>
/// ===============================================================================================
#include "stdafx.h"
#include "CustomActionCommon.h"
#include "SystemServices.h"
#include <exception>

/// ===============================================================================================
/// <summary>This method is used to stop the instrument from the console PC on install.</summary>
/// <remarks>This method uses a COM service, this the import of SystemServices.h</remarks>
/// <remarks>If we cannot obtain the interface to System Services, it may be because we have a 
/// good AU, but a brand new console with no software installed. We will not stop the install if we
/// do not get the DCOM or UnicelService interfaces. They are used to stop the Au. At this point in
/// the installation, we know that the AU is in Ready or Not Ready. We stop the AU to prevent it 
/// starting before we install. </remarks>
/// <remarks>This method uses a COM service, this the import of SystemServices.h</remarks>
/// <param name="handle">handle to the install process. Used to put IP address in memory.</param>
/// <exception cref="pE">Catch all exceptions, log event and return false.</exception>
/// <returns>Returns True on successful termination. False otherwise.</returns>
/// ===============================================================================================
BOOL StopAuFromConsole(MSIHANDLE handle)
{
    BOOL bStopAuFromConsole = TRUE;

    try
    {
        CoInitialize(NULL);

        IUnknown *pUnk = GetRemoteInterface(handle);

        if (NULL != pUnk)
        {
            ISystemServices* pUnicelService = NULL;
            Log("Calling pUnk->QueryInterface for UnicelService.");
            HRESULT hrUnicel = pUnk->QueryInterface(IID_ISystemServices, (void**)&pUnicelService);

            if ((hrUnicel == S_OK) && (pUnicelService != NULL))
            {
                Log("Terminating Au.");
                hrUnicel = pUnicelService->TerminateAu();

                if (S_OK == hrUnicel)
                {
                    Log("Au successfully terminated.");
                }
                else
                {
                    bStopAuFromConsole = FALSE;
                    Log("Au was not terminated!");
                }

                pUnicelService->Release();
                pUnicelService = NULL;
            }
            else
            {
                Log("Unable to obtain interface to UnicelService.");
            }

            pUnk->Release();
            pUnk = NULL;
        }
        else
        {
            Log("Unable to obtain DCOM interface.");
        }

        CoUninitialize();
    }
    catch (std::exception const& e)
    {
        Log("StopAuFromConsole exception thrown.");
        Log(e.what());
    }
    catch (...)
    {
        Log("StopAuFromConsole generic exception thrown.");
    }
    
    return bStopAuFromConsole;
}
