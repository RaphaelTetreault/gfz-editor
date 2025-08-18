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
unsafe internal abstract class GfzEditorWindow
{
    private GL glContext;
    private Glfw glfwContext;
    private IInputContext inputContext;
    private ImGuiController imGuiController;
    private IWindow window;

    public GL GLContext => glContext;
    public Glfw GlfwContext => glfwContext;
    public IInputContext InputContext => inputContext;
    public ImGuiController ImGuiController => imGuiController;
    public IWindow Window => window;
    public WindowHandle* WindowHandle => (WindowHandle*)Window.Handle;

    public void ConstructWindow(WindowOptions windowOptions)
    {
        // Create window
        window = Silk.NET.Windowing.Window.Create(windowOptions);
        // Add function to events if defined
        if (Closing != null) window.Closing += Closing;
        if (FileDrop != null) window.FileDrop += FileDrop;
        if (FocusChanged != null) window.FocusChanged += FocusChanged;
        if (FramebufferResize != null) window.FramebufferResize += FramebufferResize;
        window.Load += InitializeContexts;
        if (Load != null) window.Load += Load;
        if (Move != null) window.Move += Move;
        if (Render != null) window.Render += Render;
        if (Resize != null) window.Resize += Resize;
        if (StateChanged != null) window.StateChanged += StateChanged;
        if (Update != null) window.Update += Update;
    }


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


    /// <summary>
    ///     Generic function to run when window is resized.
    /// </summary>
    /// <param name="newSize"></param>
    protected void OnFramebufferResize(Vector2D<int> newSize)
    {
        glContext.Viewport(newSize);
        GlfwContext.SetWindowSize(WindowHandle, newSize.X, newSize.Y);
    }

    /// <summary>
    ///     Prepares various contexts after windw has begun running.
    /// </summary>
    private void InitializeContexts()
    {
        // Create contexts
        glContext = Window.CreateOpenGL();
        glfwContext = Glfw.GetApi();
        inputContext = Window.CreateInput();
        imGuiController = new ImGuiController(glContext, Window, InputContext);
    }

    public void Run()
    {
        window.Run();
    }
}
