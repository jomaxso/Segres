FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["demo/WeatherForecastDemo.UI/WeatherForecastDemo.UI.csproj", "WeatherForecastDemo.UI/"]
RUN dotnet restore "demo/WeatherForecastDemo.UI/WeatherForecastDemo.UI.csproj"
COPY . .
WORKDIR "/src/WeatherForecastDemo.UI"
RUN dotnet build "WeatherForecastDemo.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherForecastDemo.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecastDemo.UI.dll"]
