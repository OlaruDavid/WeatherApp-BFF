# WeatherApp BFF

Backend for a Romanian weather application built with .NET 8, Azure Functions, and PostgreSQL.

## Architecture

- **WeatherApi** - ASP.NET Core Web API that serves weather data to clients
- **WeatherFunction** - Azure Functions timer that fetches and updates weather data hourly

## Features

- Real-time weather data for 41 Romanian cities
- Automatic geocoding for unknown locations
- 50km proximity matching to nearest main city
- Permanent weather history stored in PostgreSQL
- Search history tracking
- Hourly automated updates via Azure Functions timer

## Tech Stack

- .NET 8
- Azure Functions v4
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- OpenWeatherMap API

### Setup

1. Clone the repository

```bash
git clone https://github.com/OlaruDavid/WeatherApp-BFF.git
cd WeatherApp-BFF
dotnet restore
```

2. Run database migrations:

```bash
cd WeatherFunction
dotnet ef database update
```

3. Start Azurite in a terminal:

```bash
azurite
```

4. Start WeatherFunction in another terminal:

```bash
cd WeatherFunction
dotnet run
```

5. Start WeatherApi in another terminal:

```bash
cd WeatherApi
dotnet run
```

## Romanian Cities Covered

41 cities covering all regions of Romania including:
Iași, Suceava, Bacău, Vaslui, Galați, Constanța, București, Cluj, Timișoara, Brașov, Sibiu and more.

! Add your key from "openweathermap" in appsetings and local.settings.
