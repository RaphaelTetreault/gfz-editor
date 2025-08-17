using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;


namespace gfz_editor
{
    unsafe internal class GfzEditor
    {
        private readonly GfzEditorWindow mainWindow;
        //private readonly List<GfzEditorWindow> EditorWindows = [];

        public GfzEditor()
        {
            // Create initial window
            mainWindow = new GfzEditorWindow("My Cool Window");
            mainWindow.Window.Run();
        }

        private void Update(double deltaTime)
        {
            mainWindow.Window.Render += (double _) => mainWindow.GLContext.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
