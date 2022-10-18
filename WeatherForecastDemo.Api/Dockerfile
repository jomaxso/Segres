FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WeatherForecastDemo.Api/WeatherForecastDemo.Api.csproj", "WeatherForecastDemo.Api/"]
RUN dotnet restore "WeatherForecastDemo.Api/WeatherForecastDemo.Api.csproj"
COPY . .
WORKDIR "/src/WeatherForecastDemo.Api"
RUN dotnet build "WeatherForecastDemo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherForecastDemo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecastDemo.Api.dll"]
