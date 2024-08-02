namespace Docs.Modules.Shortcuts;

public class Shortcut:BaseModel
{
    public string? FullShortcut { get; set; }
    public string? Application { get; set; }
    public string? Description { get; set; }
    public bool Alt { get; set; }
    public bool Ctrl { get; set; }
    public bool Shift { get; set; }
    public string Key { get; set; }
}