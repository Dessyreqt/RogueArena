@echo Executing psake build with default.ps1 configuration
@echo off
powershell -NoProfile -ExecutionPolicy Bypass -Command "& {Import-Module '.\tools\psake\psake.psm1'; invoke-psake psakefile.ps1 %1 -parameters @{"version"="'%2'"}; if ($psake.build_success -eq $false) { write-host "Build Failed!" -fore RED; exit 1 } else { exit 0 }}"
