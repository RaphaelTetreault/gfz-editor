using System.Collections.Generic;

namespace gfz_editor;

internal static class StringTable
{
    private static readonly Dictionary<GfzEditorText, string[]> Table = new()
        {
            { GfzEditorText.title, ["Title"] },
            { GfzEditorText.save, ["Save"] },
            { GfzEditorText.load, ["Load"] },
        };

    public static string GetString(GfzEditorText text, GfzEditorLanguage language)
    {
        string[] items = Table[text];
        int languageIndex = (int)language;
        string item = items[languageIndex];
        return item;
    }
}

public enum GfzEditorLanguage
{
    English,
}

public enum GfzEditorText
{
    title,
    save,
    load,
}