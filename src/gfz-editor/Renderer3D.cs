using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Silk.NET.GLFW;
using Silk.NET.Input.Extensions;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;



namespace gfz_editor
{
    public class Renderer3D
    {
        public Renderer3D()
        {
            INativeContext context = GL.CreateDefaultContext("");
            GL = GL.GetApi(context);
        }


        GL GL;

        uint handle_vertexBufferObject;
        uint handle_vertexArrayObject;
        uint handle_elementBufferObject;

        float[] vertices = {
              // positions        // colors
              0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
             -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
              0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
        };
        uint[] indices = {
            0, 1, 2,   // first triangle
        };
        float[] texCoords = {
            0.0f, 0.0f,  // lower-left corner  
            1.0f, 0.0f,  // lower-right corner
            0.5f, 1.0f   // top-center corner
        };


        private nuint SizeOf(float[] array)
        {
            int size = array.Length * sizeof(float);
            return (nuint)size;
        }
        private nuint SizeOf(uint[] array)
        {
            int size = array.Length * sizeof(uint);
            return (nuint)size;
        }

        protected unsafe void OnLoad()
        {
            // Set clear color for each frame
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // load the shader
            shader = new Shader("D:\\test-gl\\shader.vert", "D:\\test-gl\\shader.frag");

            // VBO
            // Create area in memory to store vertex data.
            handle_vertexBufferObject = GL.GenBuffer();
            nuint verticesSize = SizeOf(vertices);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, handle_vertexBufferObject);
            GL.BufferData<float>(BufferTargetARB.ArrayBuffer, verticesSize, new Span<float>(vertices), BufferUsageARB.StaticDraw);

            // VAO
            // Create/bind handle for vertex attributes.
            // Used to describe the vertex format.
            handle_vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(handle_vertexArrayObject);
            // Set vertex attributes to the VAO
            uint vertexAttributeLocation_pos = shader.GetAttribLocation("pos");
            uint vertexAttributeLocation_clr = shader.GetAttribLocation("clr");
            int vertexAttributeLength_pos = 3; // pos vec3
            int vertexAttributeLength_clr = 3; // clr vec3
            int bufferOffset_pos = 0;
            int bufferOffset_clr = vertexAttributeLength_pos * sizeof(float);
            uint stride = (uint)((vertexAttributeLength_pos + vertexAttributeLength_clr) * sizeof(float)); // for whole vertex
            GL.VertexAttribPointer(vertexAttributeLocation_pos, vertexAttributeLength_pos, VertexAttribPointerType.Float, false, stride, bufferOffset_pos);
            GL.VertexAttribPointer(vertexAttributeLocation_clr, vertexAttributeLength_clr, VertexAttribPointerType.Float, false, stride, bufferOffset_clr);
            GL.EnableVertexAttribArray(vertexAttributeLocation_pos);
            GL.EnableVertexAttribArray(vertexAttributeLocation_clr);

            // EBO
            handle_elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle_elementBufferObject);
            int indicesSize = SizeOf(indices);
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, indicesSize, new Span<uint>(indices), BufferUsageARB.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Set up frame
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            //shader.UpdateShaderTime();
            GL.BindVertexArray(handle_vertexArrayObject);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            //
            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            //
            shader.Dispose();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
