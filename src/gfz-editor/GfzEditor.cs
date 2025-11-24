using System;
using Silk.NET.Windowing;

namespace gfz_editor;

public readonly partial record struct GfzEditor
{
    public readonly Control Control { get; private init; } = new Control();

    public GfzEditor()
    {
        Control = new Control();
    }

    public GfzEditor(WindowOptions windowOptions)
    {
        Control = new Control(windowOptions);
    }

    public static GfzEditor CreateMainEditor()
    {
        var editor = new GfzEditor();
        var window = editor.Control.Window;
        window.Title = "GFZ Editor";
        window.Load += editor.Generic_SetWindowedFullscreen;
        window.Load += editor.MainEditor_HandleKeyboardEvents;
        window.FramebufferResize += editor.Generic_OnFramebufferResize;
        return editor;
    }

}
