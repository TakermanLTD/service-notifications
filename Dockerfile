FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /src
RUN apt update && apt install -y curl libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -fsSL https://deb.nodesource.com/nsolid_setup_deb.sh | sh -s 20
ARG BUILD_CONFIGURATION=Release
ARG NUGET_PASSWORD

COPY . .
COPY ["Takerman.Notifications/nuget.config", "./"]

RUN sed -i "s|</configuration>|<packageSourceCredentials><github><add key=\"Username\" value=\"takerman\"/><add key=\"ClearTextPassword\" value=\"${NUGET_PASSWORD}\"/></github></packageSourceCredentials></configuration>|" nuget.config
RUN dotnet nuget add source https://nuget.pkg.github.com/takermanltd/index.json --name github

# WORKDIR "/src/Takerman.Notifications.Tests"
# RUN dotnet test

WORKDIR "/src/Takerman.Notifications"
RUN dotnet build "Takerman.Notifications.csproj" -c Release -o /app/build
RUN dotnet test "Takerman.Notifications.csproj"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Takerman.Notifications.dll"]
