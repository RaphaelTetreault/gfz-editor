using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Silk.NET.Windowing;


namespace gfz_editor;

internal class GfzEditor
{

    private GfzEditorLanguage Language = GfzEditorLanguage.English;

    //private GfzEditorWindowThread mainEditorWindow;
    //private List<GfzEditorWindow> subEditorWindows = [];
    // TODO: track threads, queue up new threads, kill threads
    // if main closes, close all.
    GfzEditorWindow gfzEditorWindow;


    public GfzEditor()
    {
        gfzEditorWindow = new GfzEditorWindow();
        gfzEditorWindow.Control.Window.Title = "GFZ Editor";
        Console.WriteLine("Editor main init");

        gfzEditorWindow.Control.Window.Load += () =>
        {
            GfzEditorWindowThread.Create<GfzEditorWindow>();
        };
    }

    public void Run()
    {
        gfzEditorWindow.Control.Window.Run();
    }
}


public readonly record struct GfzEditorWindowThread
{
    //
    private static int LastID = 0;

    //
    public int ID { get; init; }
    public GfzEditorWindow EditorWindow { get; init; }
    public Thread Thread { get; init; }

    //
    public static GfzEditorWindowThread Create<TGfzEditorWindow>()
        where TGfzEditorWindow : GfzEditorWindow, new()
    {
        var editorWindow = new TGfzEditorWindow();
        var editorWindowThread = new GfzEditorWindowThread()
        {
            ID = ++LastID,
            EditorWindow = editorWindow,
            Thread = new Thread(editorWindow.Control.Window.Run),
        };
        editorWindowThread.EditorWindow.Control.Window.Title = $"Window {editorWindowThread.ID}";
        editorWindowThread.Thread.Start();
        return editorWindowThread;
    }
}
