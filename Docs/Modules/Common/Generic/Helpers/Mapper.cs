/*namespace Docs.Modules.Generic.Helpers;

public class Mapper<T> where T :  BaseModel,new()
{
    //MAP
    public static Link ToModel(T vm, bool includeDescendants) => new()
    {
        Url = vm.Url,
        Description = vm.Description,
        Docs = includeDescendants ? vm.Docs.Select(x => DocVM.ToModel(x, false)).ToHashSet() : []
    };

    public static LinkVM ToVM(Link link, bool includeDescendants = false) => new()
    {
        Url = link.Url,
        Description = link.Description,
        Docs = includeDescendants ? link.Docs.Select(x => DocVM.ToVM(x, false)).ToHashSet() : [],
    }; 
}*/