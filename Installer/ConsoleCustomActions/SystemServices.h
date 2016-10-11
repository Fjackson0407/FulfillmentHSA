
#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 5.03.0280 */
/* at Thu Jul 23 12:46:48 2015
 */
/* Compiler settings for c:\nexgen\Console\SystemServices\SystemServices.idl:
    Oicf (OptLev=i2), W1, Zp8, env=Win32 (32b run), ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __SystemServices_h__
#define __SystemServices_h__

/* Forward Declarations */ 

#ifndef __ISystemServices_FWD_DEFINED__
#define __ISystemServices_FWD_DEFINED__
typedef interface ISystemServices ISystemServices;
#endif 	/* __ISystemServices_FWD_DEFINED__ */


#ifndef __SystemServices_FWD_DEFINED__
#define __SystemServices_FWD_DEFINED__

#ifdef __cplusplus
typedef class SystemServices SystemServices;
#else
typedef struct SystemServices SystemServices;
#endif /* __cplusplus */

#endif 	/* __SystemServices_FWD_DEFINED__ */


/* header files for imported files */
#include "unknwn.h"
#include "oaidl.h"

#ifdef __cplusplus
extern "C"{
#endif 

void __RPC_FAR * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void __RPC_FAR * ); 

#ifndef __ISystemServices_INTERFACE_DEFINED__
#define __ISystemServices_INTERFACE_DEFINED__

/* interface ISystemServices */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ISystemServices;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("FEA3258C-38BE-11D5-A221-00105A226C2E")
    ISystemServices : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Reboot( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PowerDown( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StartAu( 
            /* [in] */ BSTR bstrCmdLine) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TerminateAu( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE UpdateSoftware( 
            BSTR path) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetVersion( 
            /* [retval][out] */ BSTR __RPC_FAR *bstrVersion) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetIPAddress( 
            /* [in] */ BSTR szNewIPAddress) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TerminateSysTemperature( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TerminateSysCompressor( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IsAuObjAlive( 
            /* [out] */ BOOL __RPC_FAR *bAlive) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearAUCriticalMessages( 
            /* [in] */ BOOL bMakeACopy,
            /* [in] */ BSTR bstrName) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TerminateSystemServices( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemoteMessageBox( 
            /* [in] */ BSTR bstrContent,
            /* [in] */ BSTR bstrCaption,
            /* [in] */ UINT nType,
            /* [out] */ int __RPC_FAR *pnResponse) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AUCleanUp( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DeleteAUFiles( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IsNonHardware( 
            /* [out] */ BOOL __RPC_FAR *bNhw) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetAuSerialNumber( 
            /* [out] */ BSTR __RPC_FAR *pBSTRSerialNum) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetAuSerialNumber( 
            /* [in] */ BSTR bstrSerialNum) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AcquireINIFileMutex( 
            /* [in] */ BSTR bstrINIFileName) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ReleaseINIFileMutex( 
            /* [in] */ BSTR bstrINIFileName) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetSupplyRestoreInSpecific( 
            /* [in] */ BOOL bRestore) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetDxIConfigurationFromSpecificINI( 
            /* [out] */ BOOL __RPC_FAR *pBDxI600,
            /* [out] */ BOOL __RPC_FAR *pBDualGantry,
            /* [out] */ BOOL __RPC_FAR *pBUCTA) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetUCTAinSpecificINI( 
            /* [in] */ BOOL bUCTA) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetIPAddressForNIC( 
            /* [in] */ BSTR szNewIP,
            /* [in] */ BSTR szOldIP) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DecommissionDualGantry( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetDoubleFromIniFile( 
            /* [in] */ BSTR bstrFileName,
            /* [in] */ BSTR bstrCategory,
            /* [in] */ BSTR bstrKey,
            /* [out] */ double __RPC_FAR *d) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISystemServicesVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *QueryInterface )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *AddRef )( 
            ISystemServices __RPC_FAR * This);
        
        ULONG ( STDMETHODCALLTYPE __RPC_FAR *Release )( 
            ISystemServices __RPC_FAR * This);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetTypeInfoCount )( 
            ISystemServices __RPC_FAR * This,
            /* [out] */ UINT __RPC_FAR *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetTypeInfo )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo __RPC_FAR *__RPC_FAR *ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetIDsOfNames )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR __RPC_FAR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID __RPC_FAR *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Invoke )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS __RPC_FAR *pDispParams,
            /* [out] */ VARIANT __RPC_FAR *pVarResult,
            /* [out] */ EXCEPINFO __RPC_FAR *pExcepInfo,
            /* [out] */ UINT __RPC_FAR *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *Reboot )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *PowerDown )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *StartAu )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR bstrCmdLine);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *TerminateAu )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *UpdateSoftware )( 
            ISystemServices __RPC_FAR * This,
            BSTR path);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetVersion )( 
            ISystemServices __RPC_FAR * This,
            /* [retval][out] */ BSTR __RPC_FAR *bstrVersion);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetIPAddress )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR szNewIPAddress);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *TerminateSysTemperature )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *TerminateSysCompressor )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *IsAuObjAlive )( 
            ISystemServices __RPC_FAR * This,
            /* [out] */ BOOL __RPC_FAR *bAlive);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ClearAUCriticalMessages )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BOOL bMakeACopy,
            /* [in] */ BSTR bstrName);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *TerminateSystemServices )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *RemoteMessageBox )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR bstrContent,
            /* [in] */ BSTR bstrCaption,
            /* [in] */ UINT nType,
            /* [out] */ int __RPC_FAR *pnResponse);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AUCleanUp )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *DeleteAUFiles )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *IsNonHardware )( 
            ISystemServices __RPC_FAR * This,
            /* [out] */ BOOL __RPC_FAR *bNhw);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetAuSerialNumber )( 
            ISystemServices __RPC_FAR * This,
            /* [out] */ BSTR __RPC_FAR *pBSTRSerialNum);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetAuSerialNumber )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR bstrSerialNum);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *AcquireINIFileMutex )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR bstrINIFileName);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *ReleaseINIFileMutex )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR bstrINIFileName);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetSupplyRestoreInSpecific )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BOOL bRestore);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetDxIConfigurationFromSpecificINI )( 
            ISystemServices __RPC_FAR * This,
            /* [out] */ BOOL __RPC_FAR *pBDxI600,
            /* [out] */ BOOL __RPC_FAR *pBDualGantry,
            /* [out] */ BOOL __RPC_FAR *pBUCTA);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetUCTAinSpecificINI )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BOOL bUCTA);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *SetIPAddressForNIC )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR szNewIP,
            /* [in] */ BSTR szOldIP);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *DecommissionDualGantry )( 
            ISystemServices __RPC_FAR * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE __RPC_FAR *GetDoubleFromIniFile )( 
            ISystemServices __RPC_FAR * This,
            /* [in] */ BSTR bstrFileName,
            /* [in] */ BSTR bstrCategory,
            /* [in] */ BSTR bstrKey,
            /* [out] */ double __RPC_FAR *d);
        
        END_INTERFACE
    } ISystemServicesVtbl;

    interface ISystemServices
    {
        CONST_VTBL struct ISystemServicesVtbl __RPC_FAR *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISystemServices_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISystemServices_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISystemServices_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISystemServices_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define ISystemServices_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define ISystemServices_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define ISystemServices_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define ISystemServices_Reboot(This)	\
    (This)->lpVtbl -> Reboot(This)

#define ISystemServices_PowerDown(This)	\
    (This)->lpVtbl -> PowerDown(This)

#define ISystemServices_StartAu(This,bstrCmdLine)	\
    (This)->lpVtbl -> StartAu(This,bstrCmdLine)

#define ISystemServices_TerminateAu(This)	\
    (This)->lpVtbl -> TerminateAu(This)

#define ISystemServices_UpdateSoftware(This,path)	\
    (This)->lpVtbl -> UpdateSoftware(This,path)

#define ISystemServices_GetVersion(This,bstrVersion)	\
    (This)->lpVtbl -> GetVersion(This,bstrVersion)

#define ISystemServices_SetIPAddress(This,szNewIPAddress)	\
    (This)->lpVtbl -> SetIPAddress(This,szNewIPAddress)

#define ISystemServices_TerminateSysTemperature(This)	\
    (This)->lpVtbl -> TerminateSysTemperature(This)

#define ISystemServices_TerminateSysCompressor(This)	\
    (This)->lpVtbl -> TerminateSysCompressor(This)

#define ISystemServices_IsAuObjAlive(This,bAlive)	\
    (This)->lpVtbl -> IsAuObjAlive(This,bAlive)

#define ISystemServices_ClearAUCriticalMessages(This,bMakeACopy,bstrName)	\
    (This)->lpVtbl -> ClearAUCriticalMessages(This,bMakeACopy,bstrName)

#define ISystemServices_TerminateSystemServices(This)	\
    (This)->lpVtbl -> TerminateSystemServices(This)

#define ISystemServices_RemoteMessageBox(This,bstrContent,bstrCaption,nType,pnResponse)	\
    (This)->lpVtbl -> RemoteMessageBox(This,bstrContent,bstrCaption,nType,pnResponse)

#define ISystemServices_AUCleanUp(This)	\
    (This)->lpVtbl -> AUCleanUp(This)

#define ISystemServices_DeleteAUFiles(This)	\
    (This)->lpVtbl -> DeleteAUFiles(This)

#define ISystemServices_IsNonHardware(This,bNhw)	\
    (This)->lpVtbl -> IsNonHardware(This,bNhw)

#define ISystemServices_GetAuSerialNumber(This,pBSTRSerialNum)	\
    (This)->lpVtbl -> GetAuSerialNumber(This,pBSTRSerialNum)

#define ISystemServices_SetAuSerialNumber(This,bstrSerialNum)	\
    (This)->lpVtbl -> SetAuSerialNumber(This,bstrSerialNum)

#define ISystemServices_AcquireINIFileMutex(This,bstrINIFileName)	\
    (This)->lpVtbl -> AcquireINIFileMutex(This,bstrINIFileName)

#define ISystemServices_ReleaseINIFileMutex(This,bstrINIFileName)	\
    (This)->lpVtbl -> ReleaseINIFileMutex(This,bstrINIFileName)

#define ISystemServices_SetSupplyRestoreInSpecific(This,bRestore)	\
    (This)->lpVtbl -> SetSupplyRestoreInSpecific(This,bRestore)

#define ISystemServices_GetDxIConfigurationFromSpecificINI(This,pBDxI600,pBDualGantry,pBUCTA)	\
    (This)->lpVtbl -> GetDxIConfigurationFromSpecificINI(This,pBDxI600,pBDualGantry,pBUCTA)

#define ISystemServices_SetUCTAinSpecificINI(This,bUCTA)	\
    (This)->lpVtbl -> SetUCTAinSpecificINI(This,bUCTA)

#define ISystemServices_SetIPAddressForNIC(This,szNewIP,szOldIP)	\
    (This)->lpVtbl -> SetIPAddressForNIC(This,szNewIP,szOldIP)

#define ISystemServices_DecommissionDualGantry(This)	\
    (This)->lpVtbl -> DecommissionDualGantry(This)

#define ISystemServices_GetDoubleFromIniFile(This,bstrFileName,bstrCategory,bstrKey,d)	\
    (This)->lpVtbl -> GetDoubleFromIniFile(This,bstrFileName,bstrCategory,bstrKey,d)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_Reboot_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_Reboot_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_PowerDown_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_PowerDown_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_StartAu_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR bstrCmdLine);


void __RPC_STUB ISystemServices_StartAu_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_TerminateAu_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_TerminateAu_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_UpdateSoftware_Proxy( 
    ISystemServices __RPC_FAR * This,
    BSTR path);


void __RPC_STUB ISystemServices_UpdateSoftware_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_GetVersion_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [retval][out] */ BSTR __RPC_FAR *bstrVersion);


void __RPC_STUB ISystemServices_GetVersion_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_SetIPAddress_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR szNewIPAddress);


void __RPC_STUB ISystemServices_SetIPAddress_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_TerminateSysTemperature_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_TerminateSysTemperature_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_TerminateSysCompressor_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_TerminateSysCompressor_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_IsAuObjAlive_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [out] */ BOOL __RPC_FAR *bAlive);


void __RPC_STUB ISystemServices_IsAuObjAlive_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_ClearAUCriticalMessages_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BOOL bMakeACopy,
    /* [in] */ BSTR bstrName);


void __RPC_STUB ISystemServices_ClearAUCriticalMessages_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_TerminateSystemServices_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_TerminateSystemServices_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_RemoteMessageBox_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR bstrContent,
    /* [in] */ BSTR bstrCaption,
    /* [in] */ UINT nType,
    /* [out] */ int __RPC_FAR *pnResponse);


void __RPC_STUB ISystemServices_RemoteMessageBox_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_AUCleanUp_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_AUCleanUp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_DeleteAUFiles_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_DeleteAUFiles_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_IsNonHardware_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [out] */ BOOL __RPC_FAR *bNhw);


void __RPC_STUB ISystemServices_IsNonHardware_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_GetAuSerialNumber_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [out] */ BSTR __RPC_FAR *pBSTRSerialNum);


void __RPC_STUB ISystemServices_GetAuSerialNumber_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_SetAuSerialNumber_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR bstrSerialNum);


void __RPC_STUB ISystemServices_SetAuSerialNumber_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_AcquireINIFileMutex_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR bstrINIFileName);


void __RPC_STUB ISystemServices_AcquireINIFileMutex_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_ReleaseINIFileMutex_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR bstrINIFileName);


void __RPC_STUB ISystemServices_ReleaseINIFileMutex_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_SetSupplyRestoreInSpecific_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BOOL bRestore);


void __RPC_STUB ISystemServices_SetSupplyRestoreInSpecific_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_GetDxIConfigurationFromSpecificINI_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [out] */ BOOL __RPC_FAR *pBDxI600,
    /* [out] */ BOOL __RPC_FAR *pBDualGantry,
    /* [out] */ BOOL __RPC_FAR *pBUCTA);


void __RPC_STUB ISystemServices_GetDxIConfigurationFromSpecificINI_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_SetUCTAinSpecificINI_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BOOL bUCTA);


void __RPC_STUB ISystemServices_SetUCTAinSpecificINI_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_SetIPAddressForNIC_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR szNewIP,
    /* [in] */ BSTR szOldIP);


void __RPC_STUB ISystemServices_SetIPAddressForNIC_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_DecommissionDualGantry_Proxy( 
    ISystemServices __RPC_FAR * This);


void __RPC_STUB ISystemServices_DecommissionDualGantry_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISystemServices_GetDoubleFromIniFile_Proxy( 
    ISystemServices __RPC_FAR * This,
    /* [in] */ BSTR bstrFileName,
    /* [in] */ BSTR bstrCategory,
    /* [in] */ BSTR bstrKey,
    /* [out] */ double __RPC_FAR *d);


void __RPC_STUB ISystemServices_GetDoubleFromIniFile_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISystemServices_INTERFACE_DEFINED__ */



#ifndef __SYSTEMSERVICESLib_LIBRARY_DEFINED__
#define __SYSTEMSERVICESLib_LIBRARY_DEFINED__

/* library SYSTEMSERVICESLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_SYSTEMSERVICESLib;

EXTERN_C const CLSID CLSID_SystemServices;

#ifdef __cplusplus

class DECLSPEC_UUID("FEA3258D-38BE-11D5-A221-00105A226C2E")
SystemServices;
#endif
#endif /* __SYSTEMSERVICESLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long __RPC_FAR *, unsigned long            , BSTR __RPC_FAR * ); 
unsigned char __RPC_FAR * __RPC_USER  BSTR_UserMarshal(  unsigned long __RPC_FAR *, unsigned char __RPC_FAR *, BSTR __RPC_FAR * ); 
unsigned char __RPC_FAR * __RPC_USER  BSTR_UserUnmarshal(unsigned long __RPC_FAR *, unsigned char __RPC_FAR *, BSTR __RPC_FAR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long __RPC_FAR *, BSTR __RPC_FAR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


