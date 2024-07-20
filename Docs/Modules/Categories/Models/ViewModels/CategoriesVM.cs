namespace Docs.Modules.Categories;

public class CategoryVM : BaseModel
{
    [Required(ErrorMessage = "Pole wymagane")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000, ErrorMessage = "Maksymalnie 5000 znaków ")]
    public string? Description { get; set; }


    public ICollection<DocVM> Docs { get; set; } = new HashSet<DocVM>();


    public static CategoryVM ToVm(Category category, bool includeDocs = true) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Descritpion,
            Docs = includeDocs ? category.Docs.Select(x => DocVM.ToVM(x, false)).ToHashSet() : []
        };
    


    public static Category ToModel(CategoryVM vm, bool includeCategories = true) =>
        new()
        {
            Id = vm.Id,
            Name = vm.Name,
            Descritpion = vm.Description,
            Docs = includeCategories ? vm.Docs.Select(x => DocVM.ToModel(x, false)).ToHashSet() : []
        };
}