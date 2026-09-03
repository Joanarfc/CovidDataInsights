# CovidDataInsights

## Application Description:
Covid Data Insights is a web-based mapping application that uses the <ins>Leaflet.js</ins> library (documentation available here: https://leafletjs.com/) for creating an interactive map to display a chloropleth map for COVID-19 cases. It makes requests to 2 APIs: 
- **<ins>CovidDataManagement API</ins>**: this API retrieves the files provided by World Health Organizatinon - WHO, and uses <ins>CSV Helper</ins> library for reading those files. Then, it loads the COVID-19 data contained in those files and stores the information in SQL Server database
- **<ins>GeoSpatialDataLoader API</ins>**: this API consumes a <ins>GeoJson</ins> file obtained in Natural Earth website (https://www.naturalearthdata.com/downloads/10m-cultural-vectors/) and saves the data contained in that file in a <ins>PostgreSQL</ins> database, using <ins>PostGIS</ins> extension

**<ins>Important considerations used during the development:</ins>**
- GeoJson uses [long, lat] format to represent coordinates positions (https://datatracker.ietf.org/doc/html/rfc7946#section-3.1.1)
- Leaflet.js uses [lat, long] format to represent coordinates positions (https://leafletjs.com/reference.html#latlng)

## Architecture Overview

<img src="https://github.com/Joanarfc/CovidDataInsights/assets/36134456/aa92c1e0-b406-48e4-ab19-9b4420ae4843" alt="architecture image" title="architecture image">

## How to Run

Prerequisites: .NET 6 SDK, SQL Server, PostgreSQL with PostGIS extension.

1. Clone and restore:
```bash
   git clone https://github.com/Joanarfc/CovidDataInsights.git
   cd CovidDataInsights
   dotnet restore
```
2. Configure connection strings in `appsettings.Development.json` for
   both the `CovidDataManagement` API (SQL Server) and the
   `GeoSpatialDataLoader` API (PostgreSQL/PostGIS).
3. Run database migrations for each API:
```bash
   dotnet ef database update --project src/services/CDI.CovidDataManagement.API
   dotnet ef database update --project src/services/CDI.GeoSpatialDataLoader.API
```
4. Start both APIs (in separate terminals, since both need to stay running):
```bash
   dotnet run --project src/services/CDI.CovidDataManagement.API/CDI.CovidDataManagement.API.csproj
   dotnet run --project src/services/CDI.GeoSpatialDataLoader.API/CDI.GeoSpatialDataLoader.API.csproj
```
5. Start the frontend (also its own terminal):
```bash
   dotnet run --project src/web/CDI.CovidApp.MVC/CDI.CovidApp.MVC.csproj
```
6. Open the URL shown in the MVC project's console output to view the map.

## Data Refresh

WHO case/vaccination data is loaded once from a static CSV snapshot. GeoJSON boundary data from Natural Earth is static and only needs to be loaded once, since country/region boundaries rarely change.


## Technologies Used

* C#
* ASP.NET MVC Core
* ASP.NET WebApi
* Background Services
* Entity Framework Core
* LINQ
* SQL Server
* PostgreSQL
* NLog
* CsvHelper
* Javascript
* CSS
* HTML5
* Leaflet.js

## Application Overview

Home page displaying a Leaflet map with a legend and a filter that shows the Global/World detailed information:

<img src="https://github.com/Joanarfc/CovidDataInsights/assets/36134456/1f23367d-c833-4192-8610-122af3d22319" alt="architecture image" title="architecture image">

Hover the mouse over the polygon regions, and a popup with acumulated cases, vaccinations and deaths will appear:

<img src="https://github.com/Joanarfc/CovidDataInsights/assets/36134456/c3097842-020f-4d18-8c7f-a2d860701b1a" alt="architecture image" title="architecture image">

Click in a specific polygon region, and the filter will update with the data for that specific region:

<img src="https://github.com/Joanarfc/CovidDataInsights/assets/36134456/7c9a65ae-1e61-4784-82ba-dc5d375fbe07" alt="architecture image" title="architecture image">
