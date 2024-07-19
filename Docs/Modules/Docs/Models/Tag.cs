namespace Docs.Modules.Tags;

public class Tag : BaseModel
{
    public string Name { get; set; }=string.Empty;
    
    //NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];
}