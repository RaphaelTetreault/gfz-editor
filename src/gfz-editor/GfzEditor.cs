using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gfz_editor;

internal class GfzEditor
{
    private GfzEditorLanguage Language = GfzEditorLanguage.English;

    private GfzEditorWindowMain mainWindow;
    private List<GfzEditorWindow> subwindows = [];
    // TODO: track threads, queue up new threads, kill threads
    // if main closes, close all.

    public GfzEditor()
    {
        mainWindow = new GfzEditorWindowMain("GFZ Editor");
        mainWindow.Window.Load += RunSubWindows;
        //subwindows.Add(new GfzEditorWindowMain("Sub 1"));
        //subwindows.Add(new GfzEditorWindowMain("Sub 2"));
    }

    public void Run()
    {
        mainWindow.Run();
    }

    private void RunSubWindows()
    {
        foreach (GfzEditorWindow window in subwindows)
        {
            new Thread(window.Run).Start();
        }
    }

}
