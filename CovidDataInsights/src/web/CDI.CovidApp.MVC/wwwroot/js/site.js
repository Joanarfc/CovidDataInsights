// Url Endpoints
var dataUrl = "https://localhost:7266/covid-geojson-data";
var dataCountryUrl = "https://localhost:7266/covid-data/by-country";

// Thresholds array definition
var thresholds = [0, 1000, 10000, 50000, 100000, 250000, 500000, 700000];

// Global variable to store the selected country
var selectedCountry = null;

// Basemap urls
var baseOsmLayer = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
});

var esri_dark_gray_base = L.tileLayer('http://services.arcgisonline.com/arcgis/rest/services/Canvas/World_Dark_Gray_Base/MapServer/tile/{z}/{y}/{x}', {
    attribution: '&copy; <a href="http://services.arcgisonline.com/arcgis/rest/services">ESRI</a> arcgisonline'
});
var world_street_map = L.tileLayer('http://services.arcgisonline.com/arcgis/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
    attribution: 'Tiles &copy; Esri &mdash; Source: Esri, DeLorme, NAVTEQ, USGS, Intermap, iPC, NRCAN, Esri Japan, METI, Esri China (Hong Kong), Esri (Thailand), TomTom, 2012'
});
var world_imagery = L.tileLayer('http://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
    attribution: 'Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community'
});

// Initialize the map
var map = L.map('map', {
    center: [48.497421, -34.690746],
    zoom: 1.5,
    layers: [baseOsmLayer] // Default layer
});

// Initialize basemaps
var baseLayers = {
    "Open Street Map": baseOsmLayer,
    "Dark Gray Base": esri_dark_gray_base,
    "World Street Map": world_street_map,
    "World Imagery": world_imagery
}

// Add the layers control
L.control.layers(baseLayers).addTo(map);

// Add event listener to the map container to prevent dropdown from closing
var mapContainer = document.getElementById('map');
mapContainer.addEventListener('click', function (e) {
    e.stopPropagation();
});

function getColor(d) {
    var pallete = ['#ffc0b8', '#ff9e93', '#ff7b6f', '#ff594b', '#ff3626', '#ff1302', '#ff0000', '#99000d'];

    return d >= thresholds[7] ? pallete[7] :
        d > thresholds[6] ? pallete[6] :
            d > thresholds[5] ? pallete[5] :
                d > thresholds[4] ? pallete[4] :
                    d > thresholds[3] ? pallete[3] :
                        d > thresholds[2] ? pallete[2] :
                            d > thresholds[1] ? pallete[1] :
                                pallete[0];
}

function createPolygonStyle(item) {
    return {
        weight: 1,
        opacity: 1,
        color: 'grey',
        dashArray: '',
        fillOpacity: 0.9,
        fillColor: getColor(item.covidData.cumulativeDeaths)
    };
}

function createMouseoverPolygonStyle(item) {
    return {
        weight: 5,
        color: 'yellow',
        dashArray: '',
        fillOpacity: 0.7
    };
}

// Function to format numbers
function formatNumber(value) {
    return value !== null ? value.toLocaleString('en-US') : 'N/A';
}

$.getJSON(dataUrl, function (data) {
    // Iterate over each item in the 'data' array
    $.each(data, function (i, item) {
        // Parse the 'geoJsonData.coordinates' property from JSON to a JavaScript array
        var conv_poly = JSON.parse(item.geoJsonData.coordinates);
        // Create a polygon using Leaflet library
        var poly = L.polygon(conv_poly, createPolygonStyle(item));

        poly.bindPopup(
            "<div class='custom-leaflet-popup-title'>" + item.geoJsonData.nameEN.toString() + "</div>" +
            "</br><div class='custom-leaflet-popup-subtitle1'>Confirmed Deaths and Cases:</div>" +
            "<div class='custom-leaflet-popup-content'>- " + formatNumber(item.covidData.cumulativeDeaths) + " Deaths </div>" +
            "<div class='custom-leaflet-popup-content'>- " + formatNumber(item.covidData.cumulativeCases) + " Cases</div>" +
            "</br><div class='custom-leaflet-popup-subtitle2'>Vaccination:</div>" +
            "<div class='custom-leaflet-popup-content'>- " + (typeof item.covidData.totalVaccineDoses === 'number' ? formatNumber(item.covidData.totalVaccineDoses) : item.covidData.totalVaccineDoses) + " Vaccines</div>"
        );
        poly.on('mouseover', function (e) {
            this.setStyle(createMouseoverPolygonStyle(item));
            this.openPopup();
        });
        poly.on('mouseout', function (e) {
            this.setStyle(createPolygonStyle(item));
            this.closePopup();
        });
        poly.on('click', function (e) {
            selectedCountry = item.geoJsonData.nameEN;
            updateFilterData();
        });

        poly.addTo(map);
    });
});

// Create the Legend
createLegend();
function createLegend() {
    var legend = L.control({ position: 'bottomright' });
    legend.onAdd = function (map) {
        var legendContainer = L.DomUtil.create('div', 'legend');
        var labels = [];
        var from, to;

        $(legendContainer).append("<h5 id='legendTitle'>COVID CASES LEGEND</h5>");

        // Generate a label with a coloured square
        for (var i = 0; i < thresholds.length; i++) {
            from = thresholds[i];
            to = thresholds[i + 1];

            labels.push(
                '<span class="legend-color" style="background:' + getColor(from + 1) + '"></span> ' +
                '<span class="legend-label">' + from + (to ? '&ndash;' + to : '+') + '</span>');
        }

        $(legendContainer).append(labels.join('<br>'));

        return legendContainer;
    };

    legend.addTo(map);
}

// Create and add data selection control
var dataSelectionControl = L.control({ position: 'bottomleft' });

dataSelectionControl.onAdd = function (map) {
    var container = L.DomUtil.create('div', 'leaflet-control-dropdown');

    container.innerHTML = `
        <label for="metricSelect">Select Metric:</label>
        <select id="metricSelect">
            <option value="Cases">Cases</option>
            <option value="Vaccination">Vaccination</option>
            <option value="Deaths">Deaths</option>
        </select>

        </br>
        </br>

        <div id="selectedRegion" class="leaflet-control-title"></div>
        <div id="filteredData" class="leaflet-control"></div>
    `;

    L.DomEvent.disableClickPropagation(container);

    return container;
};

dataSelectionControl.addTo(map);

// Get the metric select element
var metricSelect = document.getElementById('metricSelect');

// Function to update filtered data
function updateFilterData() {
    var selectedMetric = metricSelect.value;
    var apiUrl = dataCountryUrl + (selectedCountry ? "?country=" + encodeURIComponent(selectedCountry) : "");

    var metricInfo = {
        Cases: {
            label: "Cumulative Cases",
            property: "cumulativeCases",
        },
        Vaccination: {
            label: "Total Vaccines",
            property: "totalVaccineDoses",
        },
        Deaths: {
            label: "Cumulative Deaths",
            property: "cumulativeDeaths",
        },
    };

    $.get(apiUrl, function (data) {
        var filteredDataDiv = document.getElementById('filteredData');
        var selectedRegionDiv = document.getElementById('selectedRegion');

        if (data && data.covidData) {
            selectedRegionDiv.textContent = "Region: " + (selectedCountry || "Global");

            var metricData = metricInfo[selectedMetric];
            var numberHTML = `<div class="metric-number">${formatNumber(data.covidData[metricData.property])}</div>`;
            var labelHTML = `<div class="metric-label">${metricData.label}</div></br>`;
            var filteredData = numberHTML + labelHTML;

            if (selectedMetric === "Cases") {
                var newCasesHTML = `<div class="metric-number">${formatNumber(data.covidData.newCasesLast7Days)}</div>`;
                var newCasesLabelHTML = "<div class='metric-label'> New cases in the last 7 days</div>";
                filteredData += newCasesHTML + newCasesLabelHTML;
            } else if (selectedMetric === "Vaccination") {
                var vaccinatedAtLeastOneDoseHTML = `<div class="metric-number">${formatNumber(data.covidData.totalPersonsVaccinatedAtLeastOneDose)}</div>`;
                var vaccinatedAtLeastOneDoseLabelHTML = "<div class='metric-label'> Total Vaccinated With At Least 1 Dose</div></br>";
                var completePrimarySeriesHTML = `<div class="metric-number">${formatNumber(data.covidData.totalPersonsVaccinatedWithCompletePrimarySeries)}</div>`;
                var completePrimarySeriesLabelHTML = "<div class='metric-label'> Total Vaccinated With Complete Primary Series</div>";

                filteredData += vaccinatedAtLeastOneDoseHTML + vaccinatedAtLeastOneDoseLabelHTML;
                filteredData += completePrimarySeriesHTML + completePrimarySeriesLabelHTML;
            } else if (selectedMetric === "Deaths") {
                var newDeathsHTML = `<div class="metric-number">${formatNumber(data.covidData.newDeathsLast7Days)}</div>`;
                var newDeathsLabelHTML = "<div class='metric-label'> New deaths in the last 7 days</div>";
                filteredData += newDeathsHTML + newDeathsLabelHTML;
            }

            filteredDataDiv.innerHTML = filteredData;
        } else {
            filteredDataDiv.textContent = "Error fetching data.";
        }
    }).fail(function () {
        var filteredDataDiv = document.getElementById('filteredData');
        filteredDataDiv.textContent = "Error fetching data.";
    });
}

// Add change event listener to metric select
metricSelect.addEventListener('change', updateFilterData);

// Initial call to provide global filtered data
updateFilterData();