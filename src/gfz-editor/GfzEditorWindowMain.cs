using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;


namespace gfz_editor;
unsafe internal sealed class GfzEditorWindowMain : GfzEditorWindow
{
    public override Action<Vector2D<int>>? FramebufferResize => OnFramebufferResize;

    public override Action? Load => InitMainScreen;

    public GfzEditorWindowMain(string temp)
    {
        // Create window
        var options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = temp, //StringTable.GetString(GfzEditorText.title, Language),
        };
        ConstructWindow(options);
    }


    private void InitMainScreen()
    {
        //var monitorHandle = GlfwContext.GetWindowMonitor(WindowHandle);
        var monitorHandle = GlfwContext.GetPrimaryMonitor();
        var videoModeHandle = GlfwContext.GetVideoMode(monitorHandle);
        var videoMode = *videoModeHandle;
        // Calc size with gap
        int w = videoMode.Width - 100;
        int h = videoMode.Height - 100;
        GlfwContext.SetWindowSize(WindowHandle, w, h);
        GlfwContext.SetWindowPos(WindowHandle, 50, 50);
    }



}
