@echo off

echo Clear...
set currentPath=%cd%

set dirPath=%currentPath%\.vs
if exist %dirPath% (
	rmdir /s /q %dirPath%
)

set dirPath=%currentPath%\Assets\StreamingAssets
if exist %dirPath% (
	rmdir /s /q %dirPath%
)
set filePath=%currentPath%\Assets\StreamingAssets.meta
if exist %filePath% (
	del %filePath%
)

set dirPath=%currentPath%\Library
if exist %dirPath% (
	rmdir /s /q %dirPath%
)

set dirPath=%currentPath%\Packages
if exist %dirPath% (
	rmdir /s /q %dirPath%
)

set dirPath=%currentPath%\obj
if exist %dirPath% (
	rmdir /s /q %dirPath%
)

set dirPath=%currentPath%\StreamingAssets
if exist %dirPath% (
	rmdir /s /q %dirPath%
)

set dirPath=%currentPath%\Version
if exist %dirPath% (
	rmdir /s /q %dirPath%
)

set filePath=%currentPath%\Assembly-CSharp.csproj
if exist %filePath% (
	del %filePath%
)

set filePath=%currentPath%\Assembly-CSharp-Editor.csproj
if exist %filePath% (
	del %filePath%
)
exit 0