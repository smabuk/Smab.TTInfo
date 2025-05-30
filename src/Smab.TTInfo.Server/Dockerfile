#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj", "src/Smab.TTInfo.Server/"]
COPY ["src/Smab.TTInfo.Shared/Smab.TTInfo.Shared.csproj", "src/Smab.TTInfo.Shared/"]
COPY ["src/Smab.TTInfo.TT365/Smab.TTInfo.TT365.csproj", "src/Smab.TTInfo.TT365/"]
COPY ["src/Smab.TTInfo.TTLeagues/Smab.TTInfo.TTLeagues.csproj", "src/Smab.TTInfo.TTLeagues/"]
RUN dotnet restore "src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj"
COPY . .
WORKDIR "/src/src/Smab.TTInfo.Server"
RUN dotnet build "Smab.TTInfo.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smab.TTInfo.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smab.TTInfo.Server.dll"]