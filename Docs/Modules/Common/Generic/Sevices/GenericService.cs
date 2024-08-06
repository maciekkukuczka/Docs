namespace Docs.Modules.Generic.Services;

public class GenericService<T>(IDbContextFactory<ApplicationDbContext> dbContextFactory) where T : BaseModel, new()
{
    // GET
    public async Task<Result<HashSet<T>>> Get(
        HashSet<Expression<Func<T, object>>>? includes = null
        , HashSet<Expression<Func<T, bool>>>? filters = null
        , CancellationToken cancellationToken = default
    )
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<T> query = db.Set<T>();

        if (includes is not null && includes.Count > 0)
        {
            foreach (var expression in includes)
            {
                query = query.Include(expression);
            }
        }

        if (filters is not null && filters.Count > 0)
        {
            foreach (var expression in filters)
            {
                query = query.Where(expression);
            }
        }

        var result =  query.AsNoTracking().ToHashSet();

        try
        {
            return Result.OK(result);
        }
        catch (Exception e)
        {
            return Result.Error<HashSet<T>>($"{Messages.ObjectCannotBeGet<T>()}\n\n{e.Message}");
        }
    }


    // ADD
    public async Task<Result> Add(T entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        if (entity is null) return Result.Error($"{Messages.ObjectNotExist<T>()}");
        await db.Set<T>().AddAsync(entity);
        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Messages.ObjectCannotBeSaved<T>()}\n\nID {entity.Id}");
            return Result.OK($"{Messages.ObjectSaved<T>()}: {entity.Id}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<T>()}\n\nID {entity.Id}\n\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<T>()}\n\nID {entity.Id}\n\n{e.Message}");
        }
    }


    // UPDATE
    public async Task<Result> Update(T entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Set<T>().FindAsync(entity.Id);
        if (exist is null) return Result.Error<T>($"{Messages.ObjectNotFound<T>()}: {entity.Id}");

        db.Set<T>().Entry(exist).CurrentValues.SetValues(entity);
        try
        {
            if (await db.SaveChangesAsync() <= 0) return Result.Error<T>(Messages.ObjectCannotBeUpdate<T>());
            return Result.OK($"{Messages.ObjectSaved<T>()}: {entity.Id}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<T>()}\nID {entity.Id}\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<T>()}\nID {entity.Id}\n{e.Message}");
        }
    }

    //DELETE
    public async Task<Result> Delete(T entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Set<T>().FindAsync(entity.Id);
        if (exist is null) return Result.Error($"{Messages.ObjectNotFound<T>()}\n {entity.Id}");

        db.Set<T>().Remove(exist);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error<T>(Messages.ObjectCannotBeDeleted<T>());
            return Result.OK($"{Messages.ObjectDeleted<T>()}: {entity.Id}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeDeleted<T>()}\nID {entity.Id}\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeDeleted<T>()}\nID {entity.Id}\n{e.Message}");
        }

        // if (entity is null)
    }
}