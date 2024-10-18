FROM mcr.microsoft.com/dotnet/sdk:8.0.403-alpine3.20-arm64v8 as build
WORKDIR /app
COPY . .
WORKDIR /app/HemaDungeon
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0.10-alpine3.20-arm64v8 as runtime
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "HemaDungeon.dll"]