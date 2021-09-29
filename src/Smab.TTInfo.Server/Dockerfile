#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN ls -a
COPY ["src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj", "src/Smab.TTInfo.Server/"]
COPY ["src/Smab.TTInfo/Smab.TTInfo.csproj", "src/Smab.TTInfo/"]
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