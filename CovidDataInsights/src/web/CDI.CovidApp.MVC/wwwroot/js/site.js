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