FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["ActivTrades.MailService/ActivTrades.MailService.csproj", "ActivTrades.MailService/"]
RUN dotnet restore "ActivTrades.MailService/ActivTrades.MailService.csproj"
COPY . .

COPY ["ActivTrades.MailService.Tests/ActivTrades.MailService.Tests.csproj", "ActivTrades.MailService.Tests/"]
RUN dotnet restore "ActivTrades.MailService.Tests/ActivTrades.MailService.Tests.csproj"
COPY . .

WORKDIR "/src/ActivTrades.MailService"
RUN dotnet build "ActivTrades.MailService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivTrades.MailService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivTrades.MailService.dll"]