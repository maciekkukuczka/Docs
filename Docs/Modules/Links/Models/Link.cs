namespace Docs.Modules.Links.Models;

public class Link : BaseModel
{
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pole wymagane")]
    public string Url { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    //NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];
}