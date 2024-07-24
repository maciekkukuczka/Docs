using Docs.Modules.Common.Models;

namespace Docs.Modules.Items.Models;

public class Image:BaseModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string ImagePath { get; set; }=string.Empty;
    
    //NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];
}