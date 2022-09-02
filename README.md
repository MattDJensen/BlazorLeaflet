
# Description

BlazorLeaflet is a wrapper offering easy-to-use Blazor components that expose the <a href="https://leafletjs.com/">Leaflet API</a> in C#. It allows you to create easily customizable maps without getting outside your existing .NET ecosystem.

Forked from original (abandoned) project - https://github.com/Mehigh17/BlazorLeaflet


# Usage

Reference project in your client target. 

In your `_Host.cshtml` (Blazor Server ) or `index.html` (Blazor WebAssembly / Blazor Maui), reference the interoperability script in the `<head>` element like so:

```html
<script src="_content/BlazorLeaflet/leafletBlazorInterops.js"></script>
```


# Added Features

- Add shapes to map 
- Use Leaflet.TileLayer.MBTiles.js to load your own MBTiles sets. 
- Use Leaflet.KML to import kml to map 
- Use Leaflet.tilelayer.fallback.js to cache tiles into local 
- Use Leaflet.easybutton to add new controls to map. (in-progress)
- Export features to GeoJson using GeoJson.Net (in-progress)
- Create features from GeoJson string (in-progress)

