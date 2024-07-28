namespace Docs.Modules.Subjects.Models.ViewModels;

public class SubjectVM : BaseModel
{
    [Required(ErrorMessage = "Pole wymagane")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000, ErrorMessage = "Maksymalnie 5000 znaków ")]
    public string? Description { get; set; }

    public string? UserId { get; set; }


    public ICollection<DocVM> Docs { get; set; } = new HashSet<DocVM>();


    public static SubjectVM ToVm(Subject subject, bool includeDocs = true) =>
        new()
        {
            Id = subject.Id,
            Name = subject.Name,
            Description = subject.Description,
            UserId = subject.UserId,
            Docs = includeDocs ? subject.Docs.Select(x => DocVM.ToVM(x, false)).ToHashSet() : new HashSet<DocVM>()
        };
    


    public static Subject ToModel(SubjectVM vm, bool includeSubjects = true) =>
        new()
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description,
            UserId = vm.UserId,
            Docs = includeSubjects ? vm.Docs.Select(x => DocVM.ToModel(x, false)).ToHashSet() : new HashSet<Doc>()
        };
}