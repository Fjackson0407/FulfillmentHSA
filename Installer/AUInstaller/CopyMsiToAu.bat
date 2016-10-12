cd\
cd Windows
cd system32
net use * /delete /y
net use "\\%1\c$" tech /USER:user 
call del /Q "\\%1\c$\AUInstaller.msi"
call del /Q "\\%1\c$\AUInstall.txt"
call del /Q "\\%1\c$\Documents and Settings\All Users\Start Menu\Programs\Startup\AuObj*.*"
call copy /Y "c:\TempInstallFiles\AUInstaller.msi" "\\%1\c$\"