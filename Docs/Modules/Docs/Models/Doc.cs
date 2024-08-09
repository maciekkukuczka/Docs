namespace Docs.Modules.Items.Models;

public class Doc:BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    
    // NAV
    public DocPath? Path { get; set; }
    // public ICollection<ApplicationUser> Users { get; set; } = (HashSet<ApplicationUser>) [];
    public ICollection<Subject> Subjects { get; set; }=(HashSet<Subject>) [];
    public ICollection<Category> Categories { get; set; } = (HashSet<Category>) [];
    public ICollection<Note> Notes { get; set; }=(HashSet<Note>) [];
    public ICollection<DocPath> RelatedDocs { get; set; }=(HashSet<DocPath>) [];
    public ICollection<Link> Links { get; set; }=(HashSet<Link>) [];
    public ICollection<Tag> Tags { get; set; }=(HashSet<Tag>) [];
    public ICollection<Image> Images { get; set; }=(HashSet<Image>) [];
}