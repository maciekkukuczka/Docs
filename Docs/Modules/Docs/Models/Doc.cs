namespace Docs.Modules.Docs.Models;

public class Doc:BaseModel
{
    public string Title { get; set; }
    public DocPath  Path { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    
    // NAV
    public ICollection<Category>? Categories { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public ICollection<DocPath>? RelatedDocs { get; set; }
    public ICollection<Link>? Links { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Image>? Images { get; set; }
}