namespace Docs.Modules.Items.Models.ViewModels;

public class DocVM:BaseModel
{

    [Required(ErrorMessage = "Tytul jest wymagany")]
    [MaxLength(200, ErrorMessage = "Tytuł jest za długi. Max. 200 znaków")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Opis jest za długi. Max. 1000 znaków")]
    public string? ShortDescription { get; set; }

    [MaxLength(5000, ErrorMessage = "Opis jest za długi. Max. 5000 znaków")]
    public string? Description { get; set; }

    public ICollection<SubjectVM> Subjects { get; set; } = (HashSet<SubjectVM>) [];
    public ICollection<CategoryVM> Categories { get; set; } = (List<CategoryVM>) [];
    // public ICollection<CategoryVM> Categories { get; set; } = (HashSet<CategoryVM>) [];


    public static DocVM ToVM(Doc doc, bool includeDescendants=true) =>
        new()
        {
            Id = doc.Id,
            Title = doc.Title,
            ShortDescription = doc.ShortDescription,
            Description = doc.Description,
            Subjects =includeDescendants?
                doc.Subjects.Select(x=>SubjectVM.ToVm(x, false)).ToHashSet(): [],
            Categories = includeDescendants?
                doc.Categories.Select(x=>CategoryVM.ToVm(x,false)).ToHashSet(): []

        };

    public static Doc ToModel(DocVM docVm, bool includeDescendants=true) =>
        new()
        {
            Id = docVm.Id,
            Title = docVm.Title,
            ShortDescription = docVm.ShortDescription,
            Description = docVm.Description,
            Subjects =includeDescendants?
                docVm.Subjects.Select(x=>SubjectVM.ToModel(x,false)).ToHashSet(): [],
            Categories = includeDescendants?
                docVm.Categories.Select(x=>CategoryVM.ToModel(x,false)).ToHashSet(): []
        };
}