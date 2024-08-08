namespace Docs.Modules.Tags;

public class TagsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    : GenericService<Tag>(dbContextFactory)
{
    // GET
    public async Task<Result<HashSet<TagVM>>> GetTags(
        HashSet<Expression<Func<Tag, object>>>? includes = null
        , HashSet<Expression<Func<Tag, bool>>>? filters = null)
    {
        var models = await Get(includes);

        return models.Success
            ? Result.OK(models.Data.Select(x => TagVM.ToVm(x)).ToHashSet())
            : Result.Error<HashSet<TagVM>>($"{Messages.ObjectCannotBeGet<HashSet<TagVM>>()}");
    }

    // ADD
    public async Task<Result> AddTag(TagVM tagVm)
    {
        if (tagVm is null) return Result.Error($"{Messages.ObjectNotExist<TagVM>()}");

        var model = TagVM.ToModel(tagVm);

        var result = await Add(model);

        return result.Success
            ? Result.OK($"{Messages.ObjectCreated<TagVM>()}: {tagVm.Name}")
            : Result.Error($"{Messages.ObjectCannotBeSaved<TagVM>()}: {tagVm.Name}");
    }
    
    // UPDATE
    public async Task<Result> UpdateTag(TagVM tagVm)
    {
        if (tagVm is null) return Result.Error($"{Messages.ObjectNotExist<TagVM>()}");
        var model = TagVM.ToModel(tagVm);
        
        var result=await Update(model);
        return result.Success
            ? Result.OK($"{Messages.ObjectCreated<TagVM>()}: {tagVm.Name}")
            : Result.Error($"{Messages.ObjectCannotBeSaved<TagVM>()}: {tagVm.Name}");

    }
    
    // DELETE
    public async Task<Result> DeleteTag(TagVM tagVm)
    {
        return await Delete(TagVM.ToModel(tagVm));
    }
}