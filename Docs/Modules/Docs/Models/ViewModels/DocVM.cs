namespace Docs.Modules.Items.Models.ViewModels;

public class DocVM : BaseModel
{
    [Required(ErrorMessage = "Tytul jest wymagany")]
    [MaxLength(1000, ErrorMessage = "Tytuł jest za długi. Max. 1000 znaków")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(5000, ErrorMessage = "Opis jest za długi. Max. 5000 znaków")]
    public string? ShortDescription { get; set; }

    [MaxLength(100000, ErrorMessage = "Opis jest za długi. Max. 100.000 znaków")]
    public string? Description { get; set; }

    public ICollection<SubjectVM> Subjects { get; set; } = (HashSet<SubjectVM>) [];
    public ICollection<CategoryVM> Categories { get; set; } = (HashSet<CategoryVM>) [];
    public ICollection<LinkVM> Links { get; set; } = (HashSet<LinkVM>) [];
    public ICollection<TagVM> Tags { get; set; } = (HashSet<TagVM>) [];

    public static DocVM ToVM(Doc doc, bool hasChildren = true) =>
        new()
        {
            Id = doc.Id,
            Title = doc.Title,
            ShortDescription = doc.ShortDescription,
            Description = doc.Description,
            Subjects = hasChildren ? doc.Subjects.Select(x => SubjectVM.ToVm(x, false)).ToHashSet() : [],
            Categories = hasChildren ? doc.Categories.Select(x => CategoryVM.ToVm(x, false)).ToHashSet() : [],
            Links = hasChildren ? doc.Links.Select(x => LinkVM.ToVM(x, false)).ToHashSet() : [],
            Tags = hasChildren? doc.Tags.Select(x => TagVM.ToVm(x, false)).ToHashSet() : [],
        };

    public static Doc ToModel(DocVM docVm, bool hasChildren = true) =>
        new()
        {
            Id = docVm.Id,
            Title = docVm.Title,
            ShortDescription = docVm.ShortDescription,
            Description = docVm.Description,
            Subjects = hasChildren ? docVm.Subjects.Select(x => SubjectVM.ToModel(x, false)).ToHashSet() : [],
            Categories = hasChildren
                ? docVm.Categories.Select(x => CategoryVM.ToModel(x, false)).ToHashSet()
                : [],
            Links = hasChildren ? docVm.Links.Select(x => LinkVM.ToModel(x, false)).ToHashSet() : [],
            Tags = hasChildren? docVm.Tags.Select(x=>TagVM.ToModel(x, false)).ToHashSet():[],
        };
}