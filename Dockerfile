FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ARG BUILD_CONFIGURATION=Release
ARG NUGET_PASSWORD

COPY . .

RUN sed -i "s|</configuration>|<packageSourceCredentials><github><add key=\"Username\" value=\"takerman\"/><add key=\"ClearTextPassword\" value=\"${NUGET_PASSWORD}\"/></github></packageSourceCredentials></configuration>|" nuget.config
RUN dotnet nuget add source https://nuget.pkg.github.com/takermanltd/index.json --name github
RUN echo //npm.pkg.github.com/:_authToken=$NUGET_PASSWORD >> ~/.npmrc
RUN echo @takermanltd:registry=https://npm.pkg.github.com/ >> ~/.npmrc
RUN echo "user.email=tivanov@takerman.net" > .npmrc
RUN echo "user.name=takerman" > .npmrc
RUN echo "user.username=takerman" > .npmrc

WORKDIR "/src/Takerman.Notifications"
RUN dotnet build "Takerman.Notifications.csproj" -c Release -o /app/build
RUN dotnet test "Takerman.Notifications.csproj"

FROM build AS publish
RUN dotnet publish "Takerman.Notifications.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Takerman.Notifications.dll"]