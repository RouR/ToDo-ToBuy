@echo off
cls && dotnet run -launch-profile localProfile --project ./src/AccountService/AccountService.csproj --port=55551

