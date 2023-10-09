// Url Endpoints
var dataUrl = "https://localhost:7266/covid-geojson-data";

// Thresholds array definition
var thresholds = [0, 1000, 10000, 50000, 100000, 250000, 500000, 700000];

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
            "<div class='custom-leaflet-popup-content'>- " + item.covidData.cumulativeDeaths.toLocaleString('en-US') + " Deaths </div>" +
            "<div class='custom-leaflet-popup-content'>- " + item.covidData.cumulativeCases.toLocaleString('en-US') + " Cases</div>" +
            "</br><div class='custom-leaflet-popup-subtitle2'>Vaccination:</div>" +
            "<div class='custom-leaflet-popup-content'>- " + (typeof item.covidData.totalVaccineDoses === 'number' ? item.covidData.totalVaccineDoses.toLocaleString('en-US') : item.covidData.totalVaccineDoses) + " Vaccines</div>"
        );
        poly.on('mouseover', function (e) {
            this.setStyle(createMouseoverPolygonStyle(item));
            this.openPopup();
        });
        poly.on('mouseout', function (e) {
            this.setStyle(createPolygonStyle(item));
            this.closePopup();
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