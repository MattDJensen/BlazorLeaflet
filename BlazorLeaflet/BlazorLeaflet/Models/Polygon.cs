using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LineString = GeoJSON.Net.Geometry.LineString;
using Polygon = GeoJSON.Net.Geometry.Polygon;
using Position = GeoJSON.Net.Geometry.Position;

namespace BlazorLeaflet.Models
{
    public class Polygon : Polyline
    {

        public Polygon()
        {

        }
        public Polygon(bool fill, int width, Color fillColor)
        {
            Fill = fill;
            StrokeWidth = width;
            FillColor = fillColor;
        }

        public Polygon(GeoJSON.Net.Geometry.Polygon poly)
        {
            Shape = new PointF[1][];
            Shape[0] = new PointF[poly.Coordinates.Sum(x => x.Coordinates.Count)];
            foreach (var coord in poly.Coordinates)
            {
                for (int i = 0; i < coord.Coordinates.Count; i++)
                {
                    Shape[0][i] = new PointF((float)coord.Coordinates[i].Latitude, (float)coord.Coordinates[i].Longitude);
                }
            }
        }

        public Polygon(string GeoJson)
        {
            var poly = JsonConvert.DeserializeObject<GeoJSON.Net.Geometry.Polygon>(GeoJson);
            Shape = new PointF[1][];
            Shape[0] = new PointF[poly.Coordinates.Sum(x => x.Coordinates.Count)];
            foreach (var coord in poly.Coordinates)
            {
                for (int i = 0; i < coord.Coordinates.Count; i++)
                {
                    Shape[0][i] = new PointF((float)coord.Coordinates[i].Latitude, (float)coord.Coordinates[i].Longitude);
                }
            }
        }

        public string ToGeoJson()
        {
            var posList = new List<IPosition>();
            foreach (var pos in Shape)
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    posList.Add(new Position(pos[i].X, pos[i].Y));

                }
                posList.Add(new Position(pos[0].X, pos[0].Y));
            }
            var polygon = new GeoJSON.Net.Geometry.Polygon(new List<LineString>
            {
                new LineString(posList)
            });
            return JsonConvert.SerializeObject(polygon);

        }

    }
}
