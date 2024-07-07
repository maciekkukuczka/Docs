namespace Docs.Modules.Docs.Models;

public class Tag : BaseModel
{
    public string Name { get; set; }
    
    //NAV
    public ICollection<Doc>? Docs { get; set; }
}