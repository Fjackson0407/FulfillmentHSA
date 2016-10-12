c:

Path=%PATH%;c:\NexGen\Console\Bin;c:\NexGen\FastObjects\Bin;c:\NexGen\FastObjects\runtime\bin;

cd \
cd c:\NexGen\Console\Bin
PostOfficeUD.exe /RegServer
AUCommunicatorUD.exe /RegServer
InstrumentModelUD.exe /RegServer

cd \
cd c:\Windows\System32

regsvr32 /s C:\NexGen\Console\Bin\SfxBar.dll
regsvr32 /s C:\NexGen\Console\Bin\POEventUD.dll
regsvr32 /s C:\NexGen\Console\Bin\Cfx4032.ocx
regsvr32 /s C:\NexGen\Console\Bin\RackButtonsUD.ocx
regsvr32 /s C:\NexGen\Console\Bin\FrameworkBarsUD.ocx
regsvr32 /s C:\NexGen\Console\Bin\PostOfficeps.dll
regsvr32 /s C:\NexGen\Console\Bin\InstrumentModelps.dll
regsvr32 /s C:\NexGen\Console\Bin\AUCommunicatorps.dll
regsvr32 /s C:\NexGen\Console\Bin\AUConnection.dll
regsvr32 /s C:\NexGen\Console\Bin\AUObjps.dll
regsvr32 /s C:\NexGen\Console\Bin\AsynchronousInterface.dll
regsvr32 /s C:\NexGen\Console\Bin\BroadcastMonitorTest.dll

call del /Q "c:\Documents and Settings\All Users\Start Menu\Programs\Startup\NexGenUIUD*.*" 
call del /Q "c:\Documents and Settings\All Users\Desktop\AU Comm Test.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\DbTest UD.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\EventsTest UD.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\LIS Service UD.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\NexGenUI UD.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\NS Utility.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\POET Developer.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\POET Server.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\Reduce UD.lnk" 
call del /Q "c:\Documents and Settings\All Users\Desktop\Remote Debugger.lnk" 


regedit /s C:\TempInstallFiles\RealVNC-vncserver-registry-values-32bit.reg
