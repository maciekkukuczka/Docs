namespace Docs.Modules.Docs.Models;

public class Image:BaseModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string ImagePath { get; set; }=string.Empty;
    
    //NAV
    public ICollection<Doc>? Docs { get; set; }
}