namespace Docs.Modules.Docs.Models;

public class Note:BaseModel
{
    public string Title { get; set; }=string.Empty;
    public string? Content { get; set; }
    
    //NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];

}