/*using System.Linq.Expressions;

namespace Docs.Modules.Items.Services;

public class GenericService<TEntity,TViewModel>(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    where TEntity : BaseModel, new()
    where TViewModel : BaseModel, new()
{
    public async Task<Result<HashSet<TViewModel>>> GetAll(params Expression<Func<TEntity, object>>?[]includes
        )
    {
        includes ??= Array.Empty<Expression<Func<TEntity, object>>>();
        await using var db = await dbContextFactory.CreateDbContextAsync();

        IQueryable<TEntity> query=db.Set<TEntity>();
        if (includes is not null&&includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query =query.Include(include);
            }
        }   
        var result = await db.Set<TEntity>()
            // .IgnoreQueryFilters()
            .AsNoTracking()
            .ToHashSetAsync();
        
        var viewModels = result.Select(x=>SubjectVM.ToVm(x as TEntity)).ToHashSet();
        return Result<TViewModel>.OK(viewModels);
    }

    public async Task<Result<TEntity>> GetById(string id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Set<TEntity>().FindAsync(id);
        if (exist is null) return Result<TEntity>.Error<TEntity>($"{Errors.ObjectNotFound<TEntity>()}");
        return Result<TEntity>.OK(exist);
    }

    public async Task<Result> Add(TEntity entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.Set<TEntity>().AddAsync(entity);
        if (await db.SaveChangesAsync() <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<TEntity>()}: {entity.Id}");
        return Result.OK($"{Errors.ObjectSaved<TEntity>()}: {entity.Id}");
    }

    public async Task<Result> Update(TEntity entity)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Set<TEntity>().FindAsync(entity.Id);
        if (exist is null) return Result.Error<TEntity>($"{Errors.ObjectNotFound<TEntity>()}: {entity.Id}");

        db.Set<TEntity>().Entry(exist).CurrentValues.SetValues(entity);

        if(await db.SaveChangesAsync()<=0)return Result.Error<TEntity>(Errors.ObjectCannotBeUpdate<TEntity>());
        return Result.OK();
    }
    
    
}*/