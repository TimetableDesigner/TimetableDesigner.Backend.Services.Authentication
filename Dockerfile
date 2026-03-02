FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./TimetableDesigner.Backend.Services.Authentication", "./TimetableDesigner.Backend.Services.Authentication"]
COPY ["./TimetableDesigner.Backend.Services.Authentication.Core", "./TimetableDesigner.Backend.Services.Authentication.Core"]
COPY ["./TimetableDesigner.Backend.Services.Authentication.Database", "./TimetableDesigner.Backend.Services.Authentication.Database"]
COPY ["./TimetableDesigner.Backend.Services.Authentication.DTO.Events", "./TimetableDesigner.Backend.Services.Authentication.DTO.Events"]
COPY ["./TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI", "./TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI"]
RUN dotnet nuget add source --name gitea --username TimetableDesigner --password --mount=type=secret,id=nuget_registry_token --store-password-in-clear-text https://repos.mateuszskoczek.com/api/packages/TimetableDesigner/nuget/index.json
RUN dotnet restore "./TimetableDesigner.Backend.Services.Authentication/TimetableDesigner.Backend.Services.Authentication.csproj"
WORKDIR "/src/"
COPY . .
RUN dotnet build "./TimetableDesigner.Backend.Services.Authentication/TimetableDesigner.Backend.Services.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TimetableDesigner.Backend.Services.Authentication/TimetableDesigner.Backend.Services.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimetableDesigner.Backend.Services.Authentication.dll"]