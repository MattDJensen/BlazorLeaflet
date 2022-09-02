
# Description

BlazorLeaflet is a wrapper offering easy-to-use Blazor components that expose the <a href="https://leafletjs.com/">Leaflet API</a> in C#. It allows you to create easily customizable maps without getting outside your existing .NET ecosystem.

Forked from original (abandoned) project - https://github.com/Mehigh17/BlazorLeaflet


# Usage

Reference project in your client target. 

In your `_Host.cshtml` (Blazor Server ) or `index.html` (Blazor WebAssembly / Blazor Maui), reference the interoperability script in the `<head>` element like so:

```html
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.6.0/dist/leaflet.css"
          integrity="sha512-xwE/Az9zrjBIphAcBb3F6JVqxf46+CDLwfLMHloNu6KEQCAWi6HcDUbeOfBIptF7tcCzusKFjFw2yuvEpDL9wQ=="
          crossorigin="" />
    <script src="https://unpkg.com/leaflet@1.6.0/dist/leaflet.js"
            integrity="sha512-gZwIG9x3wUXg2hdXF6+rVkLF/0Vi9U8D2Ntg4Ga5I5BZpVkVxlJWbSQtXPSiUTtC0TjtGOmxa1AJPuV0CPthew=="
            crossorigin=""></script>
    <script src="https://unpkg.com/sql.js@0.3.2/js/sql.js" crossorigin=""></script>
    <script src="_content/BlazorLeaflet/leafletBlazorInterops.js"></script>
    <script src="_content/BlazorLeaflet/Leaflet.TileLayer.MBTiles.js"></script>
    <script src="_content/BlazorLeaflet/leaflet.tilelayer.fallback.js"></script>
    <script src="_content/BlazorLeaflet/L.KML.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/leaflet-easybutton@2/src/easy-button.css">
    <script src="https://cdn.jsdelivr.net/npm/leaflet-easybutton@2/src/easy-button.js"></script>
```


# Added Features

- Add shapes to map 
- Use Leaflet.TileLayer.MBTiles.js to load your own MBTiles sets. 
- Use Leaflet.KML to import kml to map 
- Use Leaflet.tilelayer.fallback.js to cache tiles into local 
- Use Leaflet.easybutton to add new controls to map. (in-progress)
- Export features to GeoJson using GeoJson.Net (in-progress)
- Create features from GeoJson string (in-progress)

