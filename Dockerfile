FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Takerman.MailService/Takerman.MailService.csproj", "Takerman.MailService/"]
RUN dotnet restore "Takerman.MailService/Takerman.MailService.csproj"
COPY . .

COPY ["Takerman.MailService.Tests/Takerman.MailService.Tests.csproj", "Takerman.MailService.Tests/"]
RUN dotnet restore "Takerman.MailService.Tests/Takerman.MailService.Tests.csproj"
COPY . .

WORKDIR "/src/Takerman.MailService"
RUN dotnet build "Takerman.MailService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Takerman.MailService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Takerman.MailService.dll"]