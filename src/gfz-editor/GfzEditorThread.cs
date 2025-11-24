using System.Threading;
using Silk.NET.Windowing;

namespace Manifold.GfzEditor;

public readonly record struct GfzEditorThread
{
    // Static state
    private static int LastID = 0;

    // Local state
    public int ID { get; init; }
    public GfzEditor Editor { get; init; }
    public Thread Thread { get; init; }

    // Utility function
    public static GfzEditorThread Create<TGfzEditorWindow>()
    {
        var editor = new GfzEditor();
        var editorThread = new GfzEditorThread()
        {
            ID = ++LastID,
            Editor = editor,
            Thread = new Thread(editor.Control.Window.Run),
        };
        editorThread.Editor.Control.Window.Title = $"Window {editorThread.ID}";
        editorThread.Thread.Start();
        return editorThread;
    }
}
