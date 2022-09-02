using System.Drawing;

namespace BlazorLeaflet.Models
{
    /// <summary>
    /// A class for drawing rectangle overlays on a map. Extends Polygon.
    /// <example>
    /// Example:
    /// <code>
    /// [[54.559322, -5.767822], [56.1210604, -3.021240]]
    /// </code>
    /// </example>
    /// </summary>
    public class Rectangle : Polyline<System.Drawing.RectangleF>
    {
        public Rectangle()
        {

        }

        public Rectangle(bool fill, int width, Color fillColor)
        {
            Fill = fill;
            StrokeWidth = width;
            FillColor = fillColor;
        }
    }
}
