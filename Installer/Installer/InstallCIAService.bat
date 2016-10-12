c:

Path=%PATH%;c:\NexGen\Console\Bin;c:\NexGen\FastObjects\Bin;c:\NexGen\FastObjects\runtime\bin;

cd c:\TempInstallFiles
call CIASetup.exe /s /sms /f1c:\TempInstallFiles\CIAInstall.iss /f2c:\TempInstallFiles\CIAInstall.log
call C:\Program Files\Beckman Coulter Inc\RMS CIA\bin\StartService.bat
