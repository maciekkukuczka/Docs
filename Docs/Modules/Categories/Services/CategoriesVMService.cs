namespace Docs.Modules.Categories.Services;

public class CategoriesVMService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    // GET
    public async Task<Result<HashSet<CategoryVM>>> GetCategories(string? userName)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = db.Categories
            .Include(x => x.Docs)
            // .Where(x => x.User.UserName.Equals(userName))
            .AsNoTracking().ToHashSet();
        
        // var result = await queryable.ToHashSetByEnvironment();

        if (result is null || result.Count <= 0)
            return Result.Error<HashSet<CategoryVM>>($"{Messages.ObjectNotFound<HashSet<CategoryVM>>()}");
        var resultVms = result.Select(x => CategoryVM.ToVm(x)).ToHashSet();
        return Result.OK(resultVms);
    }


    public async Task<Result<HashSet<CategoryVM>>> GetCategoriesBySubject(string? subjectId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = db.Categories
            .Where(x => x.Docs.Any(x => x.Subjects.Any(x => x.Id == subjectId)))
            .Include(x => x.Docs)
            // .AsNoTracking()
            .ToHashSet();
        if (result is null || result.Count <= 0)
            return Result.Error<HashSet<CategoryVM>>($"{Messages.ObjectNotFound<HashSet<CategoryVM>>()}");
        var resultVms = result.Select(x => CategoryVM.ToVm(x)).ToHashSet();
        return Result.OK(resultVms);
    }


    // ADD
    public async Task<Result> AddCategory(CategoryVM categoryVM)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var category = CategoryVM.ToModel(categoryVM);
        await db.Categories.AddAsync(category);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Messages.ObjectCannotBeSaved<Category>()}: {category.Name}");
            return Result.OK($"{Messages.ObjectSaved<Category>()}:{category.Name}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Category>()}:{category.Name}\n {e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Category>()}:{category.Name}\n {e.Message}");
        }
    }


//UPDATE
    public async Task<Result> UpdateCategory(CategoryVM category)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Categories.FindAsync(category.Id);
        if (exist is null) return Result.Error($"{Messages.ObjectNotFound<Category>()}: {category.Name}");

        exist.Name = category.Name;
        exist.Descritpion = category.Description;
        // db.Categories.Entry(exist).CurrentValues.SetValues(category);
        db.Categories.Update(exist);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Messages.ObjectCannotBeSaved<Category>()}: {category.Name}");
            return Result.OK($"{Messages.ObjectSaved<Category>()}: {category.Name}");
        }
        catch (DbUpdateException ex)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Category>()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Category>()}: {ex.Message}");
        }
    }


    // DELETE
    public async Task<Result> DeleteCategory(string categoryId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Categories.Include(x => x.Docs)
            .FirstOrDefaultAsync(x => x.Id.Equals(categoryId));
        if (exist is null) return Result.Error($"{Messages.ObjectNotFound<Category>()}: {categoryId}");

        var relatedDocsNames = string.Join(",", exist.Docs.Select(x => x.Title));

        if (exist.Docs.Any())
            return Result.Error($"{Messages.ObjectCannotBeDeletedHasRelatedEntities<Category>()}: {categoryId}\n" +
                                $"Powiązane encje: {relatedDocsNames}");

        db.Categories.Remove(exist);

        try
        {
            if (await db.SaveChangesAsync() <= 0)
                return Result.Error($"{Messages.ObjectCannotBeDeleted<Category>()}: {categoryId}");
            return Result.OK($"{Messages.ObjectDeleted<Category>()}: {categoryId}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeDeleted<Category>()}: {categoryId}\n\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeDeleted<Category>()}: {categoryId}\n\n{e.Message}");
        }
    }
}