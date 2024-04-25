using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;
//using System.Numerics;
using Silk.NET.GLFW;
//using Silk.NET.Maths;
using System;
//using System.Reflection;

namespace TestSilkNet;
internal class Program
{
    // State
    private static IWindow _window;
    private static GL _gl;
    private static Glfw _glfw;
    private static uint _vao;
    private static uint _vbo;
    private static uint _ebo;
    private static uint _program; // shader program

    // Data
    private static readonly float[] vertices =
    {
        // positions         // colors
         0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
        -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
         0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
    };
    private static readonly uint[] indices =
    {
        0u, 1u, 2u,
    };
    private const uint NULL = 0;

    static void Main(string[] args)
    {
        _glfw = Glfw.GetApi();
        InitWindow();
    }

    private static unsafe void OnLoad()
    {
        InitInput();
        InitOpenGL();
    }

    private static void OnUpdate(double deltaTime)
    {
        //Console.WriteLine(nameof(OnUpdate));
    }

    private static unsafe void OnRender(double deltaTime)
    {
        //Console.WriteLine(nameof(OnRender));
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        

        // Connect preset buffers to GL context, render
        _gl.BindVertexArray(_vao);
        _gl.UseProgram(_program);

        // Test wireframe
        //_gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line);

        // TIME
        double time = _glfw.GetTime();
        float r = (float)Math.Sin(time * Math.Tau / 5f) / 2f + 0.5f;
        float g = (float)Math.Sin(time * Math.Tau / 3f) / 2f + 0.5f;
        float b = (float)Math.Sin(time * Math.Tau / 7f) / 2f + 0.5f;
        // This function returns -1 if it can find the string parameter's location
        int vertexColorLocation = _gl.GetUniformLocation(_program, "uniform_color");
        //_gl.Uniform4(vertexColorLocation, r, g, b, 1);

        //
        Vector2D<int> windowSize = _window.Size;
        float aspectRatio = (float)windowSize.X / windowSize.Y;
        float fov = 60 / 360f * MathF.Tau;

        float z = (float)(-1.1 + Math.Sin(time * Math.Tau / 10f));

        Matrix4X4<float> model = Matrix4X4<float>.Identity;
        Matrix4X4<float> view = Matrix4X4.CreateTranslation(0.0f, 0.0f, z); // camera!
        Matrix4X4<float> projection = Matrix4X4.CreatePerspective(fov, aspectRatio, 0.1f, 100.0f);

        int modelLoc = _gl.GetUniformLocation(_program, "model");
        _gl.UniformMatrix4(modelLoc, 1, false, (float*)&model);
        int viewLoc = _gl.GetUniformLocation(_program, "view");
        _gl.UniformMatrix4(viewLoc, 1, false, (float*)&view);
        int projectionLoc = _gl.GetUniformLocation(_program, "projection");
        _gl.UniformMatrix4(projectionLoc, 1, false, (float*)&projection);

        // 6 = 3 verts * 2 triangles
        _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, null);
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        //Console.WriteLine($"Keyboard: {keyboard.Name}, key: {key}, key code: {keyCode}");

        if (key == Key.Escape)
            _window.Close();
    }


    private static void InitWindow()
    {
        // Create window
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "My first Silk.NET application!",
        };
        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Run();
    }
    private static void InitInput()
    {
        // Get input
        IInputContext input = _window.CreateInput();
        // Subscribe each keyboard to the function
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }
    private static unsafe void InitOpenGL()
    {
        // Create context
        _gl = _window.CreateOpenGL();
        // Set clear color
        _gl.ClearColor(Color.CornflowerBlue);
        // Set VAO - Vertex Array Object
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        // VBO - Vertex Buffer Object
        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        // EBO - Element Buffer Object (Index Buffer)
        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

        // BELOW IS NOT GL SETUP CODE

        // Pass verts and indices as pointers to GL.
        // I assume verts go to the created VBO and indices to EBO
        // TODO: make a function
        fixed (float* buf = vertices)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, GetSize(vertices), buf, BufferUsageARB.StaticDraw);
        //
        fixed (uint* buf = indices)
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, GetSize(indices), buf, BufferUsageARB.StaticDraw);

        //// Shader
        // Load, compile, and link shader program
        _program = _gl.CreateShaderProgram("shader.vert", "shader.frag");

        // Bind vertex data.  TODO: bind to VAO to reuse later on.
        uint vertexStride = SizeOfVector3 + SizeOfVector3;
        // Position
        const uint positionLoc = 0; // offset in shader
        const int positionSize = 3; // 3 = vert.xyz (floats)
        _gl.VertexAttribPointer(positionLoc, positionSize, VertexAttribPointerType.Float, false, vertexStride, null);
        _gl.EnableVertexAttribArray(positionLoc);
        // Color
        uint colorAddress = SizeOfVector3; // stride start of component
        const uint colorLoc = 1; // offset in shader
        const int colorSize = 3; // 3 = color.rgb (floats)
        _gl.VertexAttribPointer(colorLoc, colorSize, VertexAttribPointerType.Float, false, vertexStride, (void*)colorAddress);
        _gl.EnableVertexAttribArray(colorLoc);

        // CLEANUP
        // Unbind buffers. Above code set everything up for these buffers.
        _gl.BindVertexArray(NULL);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, NULL);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, NULL);
    }

    // Unsigned integer types
    public static nuint GetSize(byte[] items) => (nuint)(items.Length * sizeof(byte));
    public static nuint GetSize(ushort[] items) => (nuint)(items.Length * sizeof(ushort));
    public static nuint GetSize(uint[] items) => (nuint)(items.Length * sizeof(uint));
    public static nuint GetSize(ulong[] items) => (nuint)(items.Length * sizeof(ulong));
    //public static nuint GetSize(IEnumerable<uint> items) => (nuint)(items.Count() * sizeof(uint));
    // Signed integer types
    public static nuint GetSize(sbyte[] items) => (nuint)(items.Length * sizeof(sbyte));
    public static nuint GetSize(short[] items) => (nuint)(items.Length * sizeof(short));
    public static nuint GetSize(int[] items) => (nuint)(items.Length * sizeof(int));
    public static nuint GetSize(long[] items) => (nuint)(items.Length * sizeof(long));
    // Floating-point types
    public static nuint GetSize(float[] items) => (nuint)(items.Length * sizeof(float));
    public static nuint GetSize(double[] items) => (nuint)(items.Length * sizeof(double));
    // Vector sizes
    public static readonly uint SizeOfVector2 = 2 * sizeof(float);
    public static readonly uint SizeOfVector3 = 3 * sizeof(float);
    public static readonly uint SizeOfVector4 = 4 * sizeof(float);
}
