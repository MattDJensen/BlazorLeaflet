using BlazorLeaflet.Models;
using BlazorLeaflet.Models.Events;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Rectangle = BlazorLeaflet.Models.Rectangle;


namespace BlazorLeaflet.DrawHandlers
{
    public class RectangleDrawHandler : IDisposable, IDrawHandler
    {
        public event EventHandler DrawFinished;
        private bool IsDrawing { get; set; }
        private Map _map;
        private IJSRuntime _jsRuntime;
        private List<MouseEvent> _mouseClickEvents = new List<MouseEvent>();
        private ObservableCollection<Rectangle> Rectangles = new ObservableCollection<Rectangle>();
        private Rectangle currentRectangle = new Rectangle(true,1, Color.Red);
        private DotNetObjectReference<RectangleDrawHandler> objRef;
        public RectangleDrawHandler(Map map, IJSRuntime jsRuntime)
        {
            _map = map;
            _jsRuntime = jsRuntime;
            objRef = DotNetObjectReference.Create(this);
        }

        public RectangleDrawHandler(
            Map map,
            IJSRuntime jsRuntime,
            ObservableCollection<Rectangle> callBackPoints
        )
        {
            _map = map;
            _jsRuntime = jsRuntime;
            Rectangles = callBackPoints;
            objRef = DotNetObjectReference.Create(this);


        }
        public async void OnDrawToggle(bool isToggled)
        {
            currentRectangle = new Rectangle(true,1, Color.Red);
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
                UpdateRectangle(e.LatLng);



            }
        }

        [JSInvokable("DrawingComplete")]
        public async void OnRectangleDrawComplete()
        {
            
            Rectangles.Add(currentRectangle);
            updateSavedLayers();
            IsDrawing = false;
            DrawFinished?.Invoke(this, null);
        }
        void UpdateRectangle(LatLng latLng)
        {
            currentRectangle.Shape = new RectangleF(
                _mouseClickEvents[0].LatLng.Lng,
                _mouseClickEvents[0].LatLng.Lat,
                latLng.Lng - _mouseClickEvents[0].LatLng.Lng,
                latLng.Lat - _mouseClickEvents[0].LatLng.Lat
            );
            AddOrUpdateShape(currentRectangle);
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
            foreach (var poly in Rectangles)
            {
                if (!x.Any(x => x.Id == currentRectangle.Id))
                {
                    _map.AddLayer(poly);
                }
            }
        }

        public void Dispose() => UnsubscribeFromMapEvents();


    }
}
