
using Docs.Modules.Categories.Models;
using Docs.Modules.Common.Models;

namespace Docs.Modules.Items.Models;

public class Doc:BaseModel
{
    [Required(ErrorMessage = "Tytul jest wymagany")]
    [MaxLength(200, ErrorMessage = "Tytuł jest za długi. Max. 200 znaków")]
    public string Title { get; set; } = string.Empty;
    [MaxLength(1000, ErrorMessage = "Opis jest za długi. Max. 1000 znaków")]
    public string? ShortDescription { get; set; }
    [MaxLength(5000, ErrorMessage = "Opis jest za długi. Max. 5000 znaków")]
    public string? Description { get; set; }
    
    // NAV
    public DocPath? Path { get; set; }
    // public ICollection<ApplicationUser> Users { get; set; } = (HashSet<ApplicationUser>) [];
    public ICollection<Subject.Models.Subject> Subjects { get; set; }=(HashSet<Subject.Models.Subject>) [];
    public ICollection<Category> Categories { get; set; } = (HashSet<Category>) [];
    public ICollection<Note> Notes { get; set; }=(HashSet<Note>) [];
    public ICollection<DocPath> RelatedDocs { get; set; }=(HashSet<DocPath>) [];
    public ICollection<Link> Links { get; set; }=(HashSet<Link>) [];
    public ICollection<Tag> Tags { get; set; }=(HashSet<Tag>) [];
    public ICollection<Image> Images { get; set; }=(HashSet<Image>) [];
}