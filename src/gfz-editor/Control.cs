using System;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.WebGPU;
using Silk.NET.WebGPU.Extensions;
using Silk.NET.WebGPU.Extensions.Dawn;
using Silk.NET.WebGPU.Extensions.Disposal;
using Silk.NET.WebGPU.Extensions.WGPU;
using Silk.NET.Windowing;

namespace gfz_editor;

unsafe public class Control
{
    private GL? glContext;
    private Glfw? glfwContext;
    private IInputContext? inputContext;
    private ImGuiController? imGuiController;
    private WebGPU? webGPU;
    private readonly IWindow window;

    public GL GLContext => glContext!;
    public Glfw GlfwContext => glfwContext!;
    public IInputContext InputContext => inputContext!;
    public ImGuiController ImGuiController => imGuiController!;
    public WebGPU WebGPU => webGPU!;
    public IWindow Window => window;
    public WindowHandle* WindowHandle => (WindowHandle*)Window.Handle;
    public bool IsInitialized { get; private set; }

    public Control(string title = "Editor Window") : this(CreateWindow(DefaultWindow(title)))
    {
    }

    public Control(WindowOptions windowOptions) : this(CreateWindow(windowOptions))
    {
    }

    public Control(IWindow window)
    {
        this.window = window;
        void InitializeContexts()
        {
            glContext = Window.CreateOpenGL();
            glfwContext = Glfw.GetApi();
            inputContext = Window.CreateInput();
            imGuiController = new ImGuiController(glContext, Window, InputContext);
            webGPU = WebGPU.GetApi();
            IsInitialized = true;
        }
        window.Load += InitializeContexts;
    }

    public static IWindow CreateWindow(WindowOptions windowOptions)
    {
        IWindow window = Silk.NET.Windowing.Window.Create(windowOptions);
        return window;
    }

    public static WindowOptions DefaultWindow(string title)
    {
        // Create window
        var options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = title, //StringTable.GetString(GfzEditorText.title, Language),
        };
        return options;
    }
   


}
