using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System;

namespace gfz_editor;

unsafe public class GfzEditorWindow
{
    private GL glContext;
    private Glfw glfwContext;
    private IInputContext inputContext;
    private ImGuiController imGuiController;
    private readonly IWindow window;

    public GL GLContext => glContext;
    public Glfw GlfwContext => glfwContext;
    public IInputContext InputContext => inputContext;
    public ImGuiController ImGuiController => imGuiController;
    public IWindow Window => window;
    public WindowHandle* WindowHandle => (WindowHandle*)Window.Handle;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public GfzEditorWindow(string windowTitle)
    {
        // Create window
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = windowTitle,
        };
        window = Silk.NET.Windowing.Window.Create(options);
        window.FramebufferResize += OnResize;
        window.Load += OnLoad;
        //window.Run();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private void OnLoad()
    {
        // Create contexts
        glContext = Window.CreateOpenGL();
        glfwContext = Glfw.GetApi();
        inputContext = Window.CreateInput();
        imGuiController = new ImGuiController(glContext, Window, InputContext);
    }

    private void OnResize(Vector2D<int> newSize)
    {
        glContext.Viewport(newSize);
        GlfwContext.SetWindowSize(WindowHandle, newSize.X, newSize.Y);
    }

    public void SetAsCurrentContext()
    {
        GlfwContext.MakeContextCurrent(WindowHandle);
    }
}
