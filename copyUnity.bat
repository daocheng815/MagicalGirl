@echo off
set /p choice="Do you want to copy the folder? (Y/N): "

if /i "%choice%"=="Y" (
    xcopy "E:\gameproject\2D ACT\Assets\Scripts" "C:\Git\Unity\Scripts" /E /Y
    xcopy "E:\gameproject\2D ACT\Assets\Resources" "C:\Git\Unity\Resources" /E /Y
    xcopy "E:\gameproject\2D ACT\Assets\Inventory" "C:\Git\Unity\Inventory" /E /Y
    echo Copy completed.
) else (
    echo Copy canceled.
)

pause


