#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Protyo.DatabaseRefresh/Protyo.DatabaseRefresh.csproj", "Protyo.DatabaseRefresh/"]
RUN dotnet restore "Protyo.DatabaseRefresh/Protyo.DatabaseRefresh.csproj"
COPY . .
WORKDIR "/src/Protyo.DatabaseRefresh"
RUN dotnet build "Protyo.DatabaseRefresh.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Protyo.DatabaseRefresh.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Protyo.DatabaseRefresh.dll"]