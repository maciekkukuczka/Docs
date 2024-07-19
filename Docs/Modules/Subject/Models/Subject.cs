namespace Docs.Modules.Subjects;

public class Subject:BaseModel
{
    [Required(ErrorMessage = "Pole wymagane")]
    public string Name { get; set; }=string.Empty;
    [MaxLength(5000,ErrorMessage = "Maksymalnie 5000 znaków ")]
    public string? Description { get; set; }
    
    // NAV
    [JsonIgnore]
    public ICollection<Doc> Docs { get; set; } = (HashSet<Doc>)[];
    // public ICollection<ApplicationUser> Users { get; set; } = (HashSet<ApplicationUser>) [];

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; } 
}