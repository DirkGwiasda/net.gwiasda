#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["net.gwiasda.local.ui/net.gwiasda.local.ui.csproj", "net.gwiasda.local.ui/"]
RUN dotnet restore "net.gwiasda.local.ui/net.gwiasda.local.ui.csproj"
COPY . .
WORKDIR "/src/net.gwiasda.local.ui"
RUN dotnet build "net.gwiasda.local.ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "net.gwiasda.local.ui.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "net.gwiasda.local.ui.dll"]