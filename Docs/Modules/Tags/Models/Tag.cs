namespace Docs.Modules.Tags;

public class Tag : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Description { get; set; }

    //NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];
}