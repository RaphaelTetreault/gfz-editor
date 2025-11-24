using System;
using Silk.NET.Maths;

namespace gfz_editor;

public readonly partial record struct GfzEditor
{
    unsafe private void Generic_OnFramebufferResize(Vector2D<int> newSize)
    {
        Control.GLContext.Viewport(newSize);
        Control.GlfwContext.SetWindowSize(Control.WindowHandle, newSize.X, newSize.Y);
    }

    unsafe private void Generic_SetWindowedFullscreen()
    {
        //var monitorHandle = GlfwContext.GetWindowMonitor(WindowHandle);
        var monitorHandle = Control.GlfwContext.GetPrimaryMonitor();
        var videoModeHandle = Control.GlfwContext.GetVideoMode(monitorHandle);
        var videoMode = *videoModeHandle;
        // Calc size with gap
        int gap = 100; // pixels
        int w = videoMode.Width - gap / 2;
        int h = videoMode.Height - gap;
        Control.GlfwContext.SetWindowSize(Control.WindowHandle, w, h);
        Control.GlfwContext.SetWindowPos(Control.WindowHandle, gap / 4, gap / 2);
    }
}
