namespace Docs.Modules.Categories;

public class Category:BaseModel
{
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string Name { get; set; } = string.Empty;
    public string? Descritpion { get; set; }
    
    // NAV
    public ICollection<Doc>? Docs { get; set; } = (HashSet<Doc>) [];
}