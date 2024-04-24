using ImGuiNET;
using System.Drawing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gfz_editor
{
    public class GfzRenderWindow
    {
        const int Width = 800;
        const int Height = 600;
        private IWindow? window;

        public void Temp()
        {
            // Create a Silk.NET window as usual
            using var window = Window.Create(WindowOptions.Default);

            // Declare some variables
            ImGuiController controller = null;
            GL gl = null;
            IInputContext inputContext = null;

            // Our loading function
            window.Load += () =>
            {
                controller = new ImGuiController(
                    gl = window.CreateOpenGL(), // load OpenGL
                    window, // pass in our window
                    inputContext = window.CreateInput() // create an input context
                );
            };

            // Handle resizes
            window.FramebufferResize += s =>
            {
                // Adjust the viewport to the new window size
                gl.Viewport(s);
            };


            var programWindow = new ProgramWindow();

            // The render function
            window.Render += delta =>
            {
                // Make sure ImGui is up-to-date
                controller.Update((float)delta);

                // This is where you'll do any rendering beneath the ImGui context
                // Here, we just have a blank screen.
                gl.ClearColor(Color.FromArgb(255, (int)(.45f * 255), (int)(.55f * 255), (int)(.60f * 255)));
                gl.Clear((uint)ClearBufferMask.ColorBufferBit);

                // This is where you'll do all of your ImGUi rendering
                // Here, we're just showing the ImGui built-in demo window.
                //ImGui.ShowDemoWindow();

                programWindow.Display();

                //IInputContext input = window.CreateInput();
                //var primaryKeyboard = input.Keyboards.FirstOrDefault();
                //bool showHoverWindow = primaryKeyboard.IsKeyPressed(Key.Space);
                //if (showHoverWindow)
                //{
                //    ImGui.Begin("GFZ", (ImGuiWindowFlags)0);
                //    if (ImGui.BeginMenu("File"))
                //    {
                //        if (ImGui.MenuItem("Open", "Ctrl+O"))
                //        {
                //            Console.WriteLine("Open file.");
                //        }
                //        if (ImGui.MenuItem("Save", "Ctrl+S"))
                //        {
                //            Console.WriteLine("Save file.");
                //        }
                //        if (ImGui.MenuItem("Close", "Ctrl+W"))
                //        {
                //            Console.WriteLine("Close window.");
                //        }
                //        ImGui.EndMenu();
                //    }
                //    ImGui.End();
                //}

                // Make sure ImGui renders too!
                controller.Render();
            };

            // The closing function
            window.Closing += () =>
            {
                // Dispose our controller first
                controller?.Dispose();

                // Dispose the input context
                inputContext?.Dispose();

                // Unload OpenGL
                gl?.Dispose();
            };

            // Now that everything's defined, let's run this bad boy!
            window.Run();
        }


    }
}
