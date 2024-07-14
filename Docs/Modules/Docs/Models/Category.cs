namespace Docs.Modules.Docs.Models;

public class Category:BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string? Descritpion { get; set; }
    
    // NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];
}