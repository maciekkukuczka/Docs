namespace Docs.Modules.Docs.Models;

public class Link:BaseModel
{
    public string Url { get; set; }
    
    //NAV
    public ICollection<Doc>? Docs { get; set; }
}