using System;
using System.Collections.Generic;
using Silk.NET.Windowing;


namespace gfz_editor;

internal class GfzEditorProgram
{
    private GfzEditorLanguage Language = GfzEditorLanguage.English;

    private readonly GfzEditor editorMain;
    private readonly List<GfzEditorThread> editorSubwindows = [];

    public GfzEditorProgram()
    {
        // Main editor window
        editorMain = GfzEditor.CreateMainEditor();
        // Close all subwindows when closing
        editorMain.Control.Window.Closing += OnClosing;

        //// TEST subwindow
        //editorMain.Control.Window.Load += () =>
        //{
        //    var editorThread = GfzEditorThread.Create<GfzEditor>();
        //    editorSubwindows.Add(editorThread);
        //};
    }

    private void OnClosing()
    {
        // TODO: some day, save editor windows in local settings file

        // Close all subwindows
        foreach (var editorThread in editorSubwindows)
        {
            editorThread.Editor.Control.Window.Close();
        }
    }

    public void Run()
    {
        editorMain.Control.Window.Run();
    }
}


