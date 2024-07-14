namespace Docs.Modules.Docs.Services;

public class GenericService<T>(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    where T : BaseModel
{
    public async Task<Result<List<T>>> GetAll()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.Set<T>()
            // .IgnoreQueryFilters()
            .AsNoTracking()
            .ToListAsync();
        return Result<T>.OK(result);
    }

    public async Task<Result<T>> GetById(string id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Set<T>().FindAsync(id);
        if (exist is null) return Result<T>.Error<T>($"{Errors.ObjectNotFound<T>()}");
        return Result<T>.OK(exist);
    }

    public async Task<Result> Add(T entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.Set<T>().AddAsync(entity);
        if (await db.SaveChangesAsync() <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<T>()}: {entity.Id}");
        return Result.OK($"{Errors.ObjectSaved<T>()}: {entity.Id}");
    }

    public async Task<Result> Update(T entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Set<T>().FindAsync(entity.Id);
        if (exist is null) return Result.Error<T>($"{Errors.ObjectNotFound<T>()}: {entity.Id}");

        db.Set<T>().Entry(exist).CurrentValues.SetValues(entity);

        if(await db.SaveChangesAsync()<=0)return Result.Error<T>(Errors.ObjectCannotBeUpdate<T>());
        return Result.OK();
    }
    
    
}