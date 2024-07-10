namespace Docs.Modules.Docs.Models;

public class Doc:BaseModel
{
    [Required(ErrorMessage = "Tytul jest wymagany")]
    [MaxLength(200, ErrorMessage = "Tytuł jest za długi. Max. 200 znaków")]
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    
    // NAV
    public DocPath  Path { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public ICollection<DocPath>? RelatedDocs { get; set; }
    public ICollection<Link>? Links { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Image>? Images { get; set; }
}