using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;


namespace gfz_editor;
unsafe internal sealed class GfzEditorMainWindow : GfzEditorWindow
{
    public override Action<Vector2D<int>>? FramebufferResize => OnFramebufferResize;


    public GfzEditorMainWindow(string temp)
    {
        // Create window
        var options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = temp, //StringTable.GetString(GfzEditorText.title, Language),
        };
        ConstructWindow(options);
    }

}
