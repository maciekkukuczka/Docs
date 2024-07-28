namespace Docs.Modules.Generic.Services;

public class GenericService<T>(IDbContextFactory<ApplicationDbContext> dbContextFactory) where T : BaseModel, new()
{
    // GET
    protected async Task<Result<HashSet<T>>> Get(
         HashSet<Expression<Func<T, object>>>? includes=null
        ,HashSet<Expression<Func<T, bool>>>? filters=null
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

        if (filters is not null&&filters.Count > 0)
        {
            foreach (var expression in filters)
            {
                query = query.Where(expression);
            }
        }

        var result = await query.AsNoTracking().ToHashSetAsync(cancellationToken: cancellationToken);

        try
        {
            return Result.OK(result);
        }
        catch (Exception e)
        {
            return Result.Error<HashSet<T>>($"{Messages.ObjectCannotBeGet<T>()}\n\n{e.Message}");
        }
    }
}