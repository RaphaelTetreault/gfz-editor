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

    private GfzEditorMainWindow mainWindow;
    private List<GfzEditorWindow> subwindows = [];
    // TODO: track threads, queue up new threads, kill threads

    public GfzEditor()
    {
        mainWindow = new GfzEditorMainWindow("GFZ Editor");
        mainWindow.Window.Load += RunSubWindows;
        subwindows.Add(new GfzEditorMainWindow("Sub 1"));
        subwindows.Add(new GfzEditorMainWindow("Sub 2"));
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
