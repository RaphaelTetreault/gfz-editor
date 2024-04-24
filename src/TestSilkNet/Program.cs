using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;
using System.Numerics;
using Silk.NET.GLFW;

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
         0.5f,  0.5f, 0.0f,
         0.5f, -0.5f, 0.0f,
        -0.5f, -0.5f, 0.0f,
        -0.5f,  0.5f, 0.0f,
    };
    private static readonly uint[] indices =
    {
        0u, 1u, 3u,
        1u, 2u, 3u,
    };
    private const uint NULL = 0;

    static void Main(string[] args)
    {
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

        // BELOW IS NOT GL CODE SETUP

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

        // Bind(?) vertex data to shader
        const uint positionLoc = 0; // offset in shader, stride
        const int vertexSize = 3; // 3 = vert.xyz (floats)
        _gl.EnableVertexAttribArray(positionLoc);
        _gl.VertexAttribPointer(positionLoc, vertexSize, VertexAttribPointerType.Float, false, SizeOfVector3, null);

        // CLEANUP
        // Unbind buffers. Above code set everything up for these buffers.
        _gl.BindVertexArray(NULL);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, NULL);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, NULL);
    }

    public static nuint GetSize(byte[] items) => (nuint)(items.Length * sizeof(byte));
    public static nuint GetSize(ushort[] items) => (nuint)(items.Length * sizeof(ushort));
    public static nuint GetSize(uint[] items) => (nuint)(items.Length * sizeof(uint));
    public static nuint GetSize(ulong[] items) => (nuint)(items.Length * sizeof(ulong));
    //public static nuint GetSize(IEnumerable<uint> items) => (nuint)(items.Count() * sizeof(uint));

    public static nuint GetSize(sbyte[] items) => (nuint)(items.Length * sizeof(sbyte));
    public static nuint GetSize(short[] items) => (nuint)(items.Length * sizeof(short));
    public static nuint GetSize(int[] items) => (nuint)(items.Length * sizeof(int));
    public static nuint GetSize(long[] items) => (nuint)(items.Length * sizeof(long));

    public static nuint GetSize(float[] items) => (nuint)(items.Length * sizeof(float));
    public static nuint GetSize(double[] items) => (nuint)(items.Length * sizeof(double));

    public static readonly uint SizeOfVector2 = 2 * sizeof(float);
    public static readonly uint SizeOfVector3 = 3 * sizeof(float);
    public static readonly uint SizeOfVector4 = 4 * sizeof(float);
}
