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

## Framework
* .NET 6

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
