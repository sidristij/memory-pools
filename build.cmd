dotnet pack MemoryPools\MemoryPools.csproj -o MemoryPools\bin\Release -c Release /p:Version=1.1.3.1 
dotnet pack MemoryPools.Collections\MemoryPools.Collections.csproj -o MemoryPools.Collections\bin\Release -c Release /p:Version=1.1.3.1

dotnet nuget push MemoryPools\bin\Release\MemoryPools.1.1.3.1.nupkg -k oy2cthywk3rsieisauhabiru7magfuue3nm4xr4illfuuq -s https://api.nuget.org/v3/index.json
dotnet nuget push MemoryPools.Collections\bin\Release\MemoryPools.Collections.1.1.3.1.nupkg -k oy2cthywk3rsieisauhabiru7magfuue3nm4xr4illfuuq -s https://api.nuget.org/v3/index.json