@echo off
Setlocal enabledelayedexpansion
::CODER BY Ambition POWERD BY iBAT

cd %~dp0

set Config_Root=E:\\Application\\Mushrooms\\Config\\Out
echo %Config_Root%

set Work_Root=%cd%\\..\\Assets
echo %Work_Root%

set StreamingAssets=%cd%\\..\\Assets\\StreamingAssets\\Json
echo %StreamingAssets%

set Code=%cd%\\..\\Assets\\Script\\config
echo %Code%

del %Code%\\*.cs
del %StreamingAssets%\\*.json

xcopy /s /y %Config_Root%\\config %Code%
xcopy /s /y %Config_Root%\\Json %StreamingAssets%


pause
