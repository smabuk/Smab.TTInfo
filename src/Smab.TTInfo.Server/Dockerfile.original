#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN dotnet --info
COPY ["src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj", "src/Smab.TTInfo.Server/"]
COPY ["src/Smab.TTInfo.Shared/Smab.TTInfo.Shared.csproj", "src/Smab.TTInfo.Shared/"]
COPY ["src/Smab.TTInfo.TT365/Smab.TTInfo.TT365.csproj", "src/Smab.TTInfo.TT365/"]
COPY ["src/Smab.TTInfo.TTLeagues/Smab.TTInfo.TTLeagues.csproj", "src/Smab.TTInfo.TTLeagues/"]
RUN dotnet restore "src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj"
COPY . .
WORKDIR "/src/src/Smab.TTInfo.Server"
RUN dotnet build "Smab.TTInfo.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smab.TTInfo.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /cache
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smab.TTInfo.Server.dll"]