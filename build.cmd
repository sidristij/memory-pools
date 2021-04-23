SET PACKAGE_VERSION=1.1.3.5

dotnet pack MemoryPools\MemoryPools.csproj -o MemoryPools\bin\Release -c Release /p:Version=%PACKAGE_VERSION% 
dotnet pack MemoryPools.Collections\MemoryPools.Collections.csproj -o MemoryPools.Collections\bin\Release -c Release /p:Version=%PACKAGE_VERSION%

dotnet nuget push MemoryPools\bin\Release\MemoryPools.%PACKAGE_VERSION%.nupkg -k oy2cthywk3rsieisauhabiru7magfuue3nm4xr4illfuuq -s https://api.nuget.org/v3/index.json
dotnet nuget push MemoryPools.Collections\bin\Release\MemoryPools.Collections.%PACKAGE_VERSION%.nupkg -k oy2cthywk3rsieisauhabiru7magfuue3nm4xr4illfuuq -s https://api.nuget.org/v3/index.json