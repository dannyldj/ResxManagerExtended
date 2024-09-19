FROM nginx:stable AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ResxManagerExtended.Web/ResxManagerExtended.Web.csproj", "ResxManagerExtended.Web/"]
COPY ["ResxManagerExtended.Shared/ResxManagerExtended.Shared.csproj", "ResxManagerExtended.Shared/"]
RUN dotnet restore "ResxManagerExtended.Web/ResxManagerExtended.Web.csproj"
COPY . .
WORKDIR "/src/ResxManagerExtended.Web"
RUN dotnet build "ResxManagerExtended.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ResxManagerExtended.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ResxManagerExtended.Web/nginx.conf /etc/nginx/nginx.conf