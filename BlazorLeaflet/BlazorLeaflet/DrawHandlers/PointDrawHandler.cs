using BlazorLeaflet;
using BlazorLeaflet.DrawHandlers;
using BlazorLeaflet.Models;
using BlazorLeaflet.Models.Events;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace BlazorLeaflet.DrawHandlers
{
    public class PointDrawHandler : IDisposable, IDrawHandler
    {
        private bool IsDrawing { get; set; }
        public event EventHandler DrawFinished;
        private Map _map;
        private IJSRuntime _jsRuntime;
        private Marker currentPoint = new Marker(new LatLng());
        private List<MouseEvent> _mouseClickEvents = new List<MouseEvent>();
        private ObservableCollection<Marker> points = new ObservableCollection<Marker>();

        public PointDrawHandler(Map map, IJSRuntime jsRuntime)
        {
            _map = map;
            _jsRuntime = jsRuntime;
        }

        public PointDrawHandler(
            Map map,
            IJSRuntime jsRuntime,
            ObservableCollection<Marker> callBackPoints
        )
        {
            _map = map;
            _jsRuntime = jsRuntime;
            points = callBackPoints;
        }

        public void OnDrawToggle(bool isToggled)
        {
            currentPoint = new Marker(new LatLng());
            _mouseClickEvents.Clear();
            if (isToggled)
            {
                _map.OnClick += OnMapClick;
                IsDrawing = true;
            }
            else
            {
                IsDrawing = false;
                UnsubscribeFromMapEvents();
            }
        }

        void UnsubscribeFromMapEvents()
        {
            _map.OnClick -= OnMapClick;
        }

        void OnMapClick(object sender, MouseEvent e)
        {
            _mouseClickEvents.Add(e);
            if (IsDrawing)
            {
                currentPoint.Position = _mouseClickEvents[0].LatLng;
                currentPoint.Popup = new Popup
                {
                    Content =
                        currentPoint.Position.Lat.ToString()
                        + ","
                        + currentPoint.Position.Lng.ToString(),

                };
                points.Add(currentPoint);
                currentPoint.Title = points
                    .ToList()
                    .FindIndex(x => x.Position == currentPoint.Position)
                    .ToString();
                updateSavedLayers();
                IsDrawing = false;
                DrawFinished?.Invoke(this, null);
            }
        }

        void updateSavedLayers()
        {
            var x = _map.GetLayers();
            foreach (var point in points)
            {
                if (!x.Any(x => x.Id == currentPoint.Id))
                {
                    _map.AddLayer(point);
                }
            }
        }

        public void Dispose() => UnsubscribeFromMapEvents();
    }
}
