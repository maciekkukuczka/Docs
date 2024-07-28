namespace Docs.Modules.Items.Models.ViewModels;

public class LinkVM : BaseModel
{
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Pole obowiązkowe")]
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    // NAV
    public ICollection<DocVM?> Docs { get; set; } = (HashSet<DocVM>) [];

    //MAP
    public static Link ToModel(LinkVM link, bool includeDescendants) => new()
    {
        Name = link.Name,
        Url = link.Url,
        Description = link.Description,
        Docs = includeDescendants ? link.Docs.Select(x => DocVM.ToModel(x, false)).ToHashSet() : []
    };

    public static LinkVM ToVM(Link link, bool includeDescendants = false) => new()
    {
        Name = link.Name,
        Url = link.Url,
        Description = link.Description,
        Docs = includeDescendants ? link.Docs.Select(x => DocVM.ToVM(x, false)).ToHashSet() : [],
    };
}