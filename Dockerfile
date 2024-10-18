FROM mcr.microsoft.com/dotnet/aspnet:8.0.10-alpine3.20-arm64v8 as runtime
WORKDIR /publish
COPY publish/ .
ENTRYPOINT ["dotnet", "HemaDungeon.dll"]