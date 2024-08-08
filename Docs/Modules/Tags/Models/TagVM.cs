namespace Docs.Modules.Tags;

public class TagVM : BaseModel
{
    public TagVM()
    {
        
    }
    [Required(ErrorMessageResourceType = typeof(Messages), 
        ErrorMessageResourceName="ValidationRequired")]
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Description { get; set; }

    public ICollection<DocVM> DocVms { get; set; } = (HashSet<DocVM>) [];


    public static TagVM ToVm(Tag tag, bool hasChildren = true) =>
        new()
        {
            Id = tag.Id,
            Name = tag.Name,
            Color = tag.Color,
            Description = tag.Description,
            DocVms = hasChildren ? tag.Docs.Select(x => DocVM.ToVM(x, false)).ToHashSet() : new HashSet<DocVM>()
        };


    public static Tag ToModel( TagVM tagVm, bool hasChildren = true) =>
        new()
        {
            Id = tagVm.Id,
            Name = tagVm.Name,
            Color = tagVm.Color,
            Description = tagVm.Description,
            Docs = hasChildren ? tagVm.DocVms.Select(x => DocVM.ToModel(x, false)).ToHashSet() : new HashSet<Doc>()
        };
}