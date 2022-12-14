! function (i, e) {
    "function" == typeof define && define.amd ? define(["leaflet"], e) : e("object" == typeof module && module.exports ? require("leaflet") : i.L)
}(this, function (i) {
    i.TileLayer.Fallback = i.TileLayer.extend({
        options: {
            minNativeZoom: 0
        },
        initialize: function (e, r) {
            i.TileLayer.prototype.initialize.call(this, e, r)
        },
        createTile: function (e, r) {
            var t = i.TileLayer.prototype.createTile.call(this, e, r);
            return t._originalCoords = e, t._originalSrc = t.src, t
        },
        _createCurrentCoords: function (i) {
            var e = this._wrapCoords(i);
            return e.fallback = !0, e
        },
        _originalTileOnError: i.TileLayer.prototype._tileOnError,
        _tileOnError: function (i, e, r) {
            var t, l, o, a = this,
                n = e._originalCoords,
                c = e._currentCoords = e._currentCoords || a._createCurrentCoords(n),
                s = e._fallbackZoom = void 0 === e._fallbackZoom ? n.z - 1 : e._fallbackZoom - 1,
                f = e._fallbackScale = 2 * (e._fallbackScale || 1),
                p = a.getTileSize(),
                u = e.style;
            if (s < a.options.minNativeZoom) return this._originalTileOnError(i, e, r);
            c.z = s, c.x = Math.floor(c.x / 2), c.y = Math.floor(c.y / 2), t = a.getTileUrl(c), u.width = p.x * f + "px", u.height = p.y * f + "px", l = (n.y - c.y * f) * p.y, u.marginTop = -l + "px", o = (n.x - c.x * f) * p.x, u.marginLeft = -o + "px", u.clip = "rect(" + l + "px " + (o + p.x) + "px " + (l + p.y) + "px " + o + "px)", a.fire("tilefallback", {
                tile: e,
                url: e._originalSrc,
                urlMissing: e.src,
                urlFallback: t
            }), e.src = t
        },
        getTileUrl: function (e) {
            var r = e.z = e.fallback ? e.z : this._getZoomForUrl(),
                t = {
                    r: i.Browser.retina ? "@2x" : "",
                    s: this._getSubdomain(e),
                    x: e.x,
                    y: e.y,
                    z: r
                };
            if (this._map && !this._map.options.crs.infinite) {
                var l = this._globalTileRange.max.y - e.y;
                this.options.tms && (t.y = l), t["-y"] = l
            }
            return i.Util.template(this._url, i.extend(t, this.options))
        }
    }), i.tileLayer.fallback = function (e, r) {
        return new i.TileLayer.Fallback(e, r)
    }
});