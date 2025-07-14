FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Server/Viewer.Server.Presentation/Viewer.Server.Presentation.csproj", "src/Server/Viewer.Server.Presentation/"]
COPY ["src/Viewer.Shared/Viewer.Shared.csproj", "src/Viewer.Shared/"]
COPY ["src/Server/Viewer.Server.Domain/Viewer.Server.Domain.csproj", "src/Server/Viewer.Server.Domain/"]
COPY ["src/Server/Viewer.Server.Infrastructure/Viewer.Server.Infrastructure.csproj", "src/Server/Viewer.Server.Infrastructure/"]
COPY ["src/Server/Viewer.Server.Application/Viewer.Server.Application.csproj", "src/Server/Viewer.Server.Application/"]
RUN dotnet restore "src/Server/Viewer.Server.Presentation/Viewer.Server.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Server/Viewer.Server.Presentation"
RUN dotnet build "Viewer.Server.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Viewer.Server.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Viewer.Server.Presentation.dll"]
