using System;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace Manifold.GfzEditor;

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

    // GENERIC input polling
    private void KeyboardSubscribeToEvents(Action<IKeyboard, Key, int> keyboardEvent)
    {
        var input = Control.InputContext;
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += keyboardEvent;
    }
    private void KeyboardCloseOnEsc(Key key)
    {
        if (key == Key.Escape)
            Control.Window.Close();
    }

    // Assign specific method to generic event
    private void MainEditor_HandleKeyboardEvents() => KeyboardSubscribeToEvents(MainEditor_HandleKeyboardEvents);

    // Define specific handling
    private void MainEditor_HandleKeyboardEvents(IKeyboard keyboard, Key key, int keyCode)
    {
        KeyboardCloseOnEsc(key);
    }

}
