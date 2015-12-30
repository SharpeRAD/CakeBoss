@ECHO OFF

@setlocal enableextensions
@cd /d "%~dp0"

if exist "%cd%\CakeBoss.Agent.exe" (
    SET file=%cd%\CakeBoss.Agent.exe
) else (
    SET file=%cd%\CakeBoss.Server.exe
)
ECHO %file%

CALL "%file%" uninstall
PAUSE