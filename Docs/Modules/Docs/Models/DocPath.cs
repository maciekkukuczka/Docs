namespace Docs.Modules.Docs.Models;

[ComplexType]
public class DocPath
{
    public string Path { get; set; }
    
    // NAV
    public ICollection<Doc> Docs { get; set; }
}