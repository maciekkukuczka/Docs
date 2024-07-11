namespace Docs.Modules.Docs.Models;

// [ComplexType]
public class DocPath:BaseModel
{
    public string Path { get; set; }=string.Empty;
    
    // NAV
    public string DocId { get; set; }
    public Doc? Doc { get; set; }
    public ICollection<Doc>? Docs { get; set; }
}