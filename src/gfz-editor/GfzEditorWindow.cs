using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;


namespace gfz_editor;
unsafe public class GfzEditorWindow
{
    public Control Control { get; private set; }

    public virtual Action? Closing { get; }
    public virtual Action<string[]>? FileDrop { get; }
    public virtual Action<bool>? FocusChanged { get; }
    public virtual Action<Vector2D<int>>? FramebufferResize { get; }
    public virtual Action? Load { get; }
    public virtual Action<Vector2D<int>>? Move { get; }
    public virtual Action<double>? Render { get; }
    public virtual Action<Vector2D<int>>? Resize { get; }
    public virtual Action<WindowState>? StateChanged { get; }
    public virtual Action<double>? Update { get; }

    public GfzEditorWindow()
    {
        Console.WriteLine("Create control");
        Control = new Control();
    }

}
