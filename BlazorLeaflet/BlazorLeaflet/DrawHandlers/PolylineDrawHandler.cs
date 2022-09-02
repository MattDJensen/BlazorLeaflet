using BlazorLeaflet.Models;
using BlazorLeaflet.Models.Events;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace BlazorLeaflet.DrawHandlers
{
    public class PolylineDrawHandler : IDisposable, IDrawHandler
    {
        public event EventHandler DrawFinished;
        private bool IsDrawing { get; set; }
        private Map _map;
        private IJSRuntime _jsRuntime;
        private List<MouseEvent> _mouseClickEvents = new List<MouseEvent>();
        private ObservableCollection<Polyline> Polylines = new ObservableCollection<Polyline>();
        private Polyline currentPolyline = new Polyline( 1, Color.Red);
        private DotNetObjectReference<PolylineDrawHandler> objRef;
        public PolylineDrawHandler(Map map, IJSRuntime jsRuntime)
        {
            _map = map;
            _jsRuntime = jsRuntime;
            objRef = DotNetObjectReference.Create(this);
        }

        public PolylineDrawHandler(
            Map map,
            IJSRuntime jsRuntime,
            ObservableCollection<Polyline> callBackPoints
        )
        {
            _map = map;
            _jsRuntime = jsRuntime;
            Polylines = callBackPoints;
            objRef = DotNetObjectReference.Create(this);


        }
        public async void OnDrawToggle(bool isToggled)
        {
            currentPolyline = new Polyline(1, Color.Red);
            _mouseClickEvents.Clear();
            await _map.AddCompleteButton(objRef);
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
        async void OnMapClick(object sender, MouseEvent e)
        {
            _mouseClickEvents.Add(e);
            if (IsDrawing)
            {
                UpdatePolyline(e.LatLng);



            }
        }

        [JSInvokable("DrawingComplete")]
        public async void OnPolylineDrawComplete()
        {
            UpdatePolyline(null);
            Polylines.Add(currentPolyline);
            updateSavedLayers();
            IsDrawing = false;
            DrawFinished?.Invoke(this, null);
        }
        void UpdatePolyline(LatLng latLng)
        {
            // copy over previous points, add a new one if LatLng defined
            var size = _mouseClickEvents.Count;
            var shape = new PointF[1][];
            // hyrdrating list with given count of current points
            shape[0] = new PointF[latLng == null ? size : size + 1];
            // add prev points
            for (int i = 0; i < size; i++)
            {
                shape[0][i] = _mouseClickEvents[i].LatLng.ToPointF();
            }
            // add current point
            if (latLng != null)
            {
                shape[0][size] = latLng.ToPointF();
            }
            currentPolyline.Shape = shape;
            AddOrUpdateShape(currentPolyline);
        }
        void AddOrUpdateShape(Layer shape)
        {
            var x = _map.GetLayers();
            if (x.Any(x => x.Id == shape.Id))
            {
                LeafletInterops.UpdateShape(_jsRuntime, _map.Id, shape);
            }
            else
            {
                _map.AddLayer(shape);
            }
        }

        void updateSavedLayers()
        {
            var x = _map.GetLayers();
            foreach (var poly in Polylines)
            {
                if (!x.Any(x => x.Id == currentPolyline.Id))
                {
                    _map.AddLayer(poly);
                }
            }
        }

        public void Dispose() => UnsubscribeFromMapEvents();


    }
}
