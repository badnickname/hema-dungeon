FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
#WORKDIR /app
#COPY HemaDungeon/HemaDungeon.csproj ./HemaDungeon/HemaDungeon.csproj
#COPY HemaDungeon.Calculator/HemaDungeon.Calculator.csproj ./HemaDungeon.Calculator/HemaDungeon.Calculator.csproj
#WORKDIR /app/HemaDungeon
#RUN dotnet restore
WORKDIR /app
COPY . .
WORKDIR /app/HemaDungeon
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "HemaDungeon.dll"]