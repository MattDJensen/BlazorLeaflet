using System;

namespace BlazorLeaflet.DrawHandlers
{
    public interface IDrawHandler
    {
        event EventHandler DrawFinished;
        public void OnDrawToggle(bool isToggled);

        void Dispose();
    }
}
