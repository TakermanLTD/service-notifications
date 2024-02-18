FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Takerman.Notifications/Takerman.Notifications.csproj", "Takerman.Notifications/"]
RUN dotnet clean "Takerman.Notifications/Takerman.Notifications.csproj"
RUN dotnet restore "Takerman.Notifications/Takerman.Notifications.csproj"
COPY . .

COPY ["Takerman.Notifications.Tests/Takerman.Notifications.Tests.csproj", "Takerman.Notifications.Tests/"]
RUN dotnet clean "Takerman.Notifications.Tests/Takerman.Notifications.Tests.csproj"
RUN dotnet restore "Takerman.Notifications.Tests/Takerman.Notifications.Tests.csproj"
COPY . .

WORKDIR "/src/Takerman.Notifications"
RUN dotnet build "Takerman.Notifications.csproj" -c Release -o /app/build
RUN dotnet test "Takerman.Notifications.csproj"

FROM build AS publish
RUN dotnet publish "Takerman.Notifications.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Takerman.Notifications.dll"]