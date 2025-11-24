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

unsafe public class Control
{
    private GL? glContext;
    private Glfw? glfwContext;
    private IInputContext? inputContext;
    private ImGuiController? imGuiController;
    private readonly IWindow window;

    public GL GLContext => glContext!;
    public Glfw GlfwContext => glfwContext!;
    public IInputContext InputContext => inputContext!;
    public ImGuiController ImGuiController => imGuiController!;
    public IWindow Window => window;
    public WindowHandle* WindowHandle => (WindowHandle*)Window.Handle;
    public bool IsInitialized { get; private set; }

    public Control(string title = "Editor Window") : this(CreateWindow(DefaultWindow(title)))
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
            IsInitialized = true;
            Console.WriteLine("Window load");
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
   
    /// <summary>
    ///     Makes the curren GLfw window fullscreen... kinda.
    /// </summary>
    /// <param name="glfw"></param>
    private void SetFullscreen(Glfw glfw)
    {
        //var monitorHandle = GlfwContext.GetWindowMonitor(WindowHandle);
        var monitorHandle = glfw.GetPrimaryMonitor();
        var videoModeHandle = glfw.GetVideoMode(monitorHandle);
        var videoMode = *videoModeHandle;
        // Calc size with gap
        int w = videoMode.Width - 100;
        int h = videoMode.Height - 100;
        glfw.SetWindowSize(WindowHandle, w, h);
        glfw.SetWindowPos(WindowHandle, 50, 50);
    }

}
