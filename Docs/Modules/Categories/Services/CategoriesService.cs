namespace Docs.Modules.Categories.Services;

public class CategoriesService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    // GET
    public async Task<Result<HashSet<Category>>> GetCategories(string? userName)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.Categories
            .Include(x => x.Docs)
            // .Where(x => x.User.UserName.Equals(userName))
            .AsNoTracking()
            .ToHashSetAsync();

        if (result is null || result.Count <= 0)
            return Result.Error<HashSet<Category>>($"{Errors.ObjectNotFound<HashSet<Category>>()}");
        return Result.OK(result);
    }

    // ADD
    public async Task<Result> AddCategory(Category category)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        await db.Categories.AddAsync(category);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0)
                return Result.Error($"{Errors.ObjectCannotBeSaved<Subject.Models.Subject>()}: {category.Name}");
            return Result.OK($"{Errors.ObjectSaved<Subject.Models.Subject>()}:{category.Name}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error(
                $"{Errors.ObjectCannotBeSaved<Subject.Models.Subject>()}:{category.Name}\n {e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error(
                $"{Errors.ObjectCannotBeSaved<Subject.Models.Subject>()}:{category.Name}\n {e.Message}");
        }
    }


//UPDATE
    public async Task<Result> UpdateCategory(Category category)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Subjects.FindAsync(category.Id);
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Category>()}: {category.Name}");

        db.Subjects.Entry(exist).CurrentValues.SetValues(category);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Category>()}: {category.Name}");
            return Result.OK($"{Errors.ObjectSaved<Subject.Models.Subject>()}: {category.Name}");
        }
        catch (DbUpdateException ex)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Category>()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Category>()}: {ex.Message}");
        }
    }


    // DELETE
    public async Task<Result> DeleteSubject(string subjectId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Subjects.Include(x => x.Docs)
            .FirstOrDefaultAsync(x => x.Id.Equals(subjectId));
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Subject.Models.Subject>()}: {subjectId}");

        var relatedDocsNames = string.Join(",", exist.Docs.Select(x => x.Title));

        if (exist.Docs.Any())
            return Result.Error(
                $"{Errors.ObjectCannotBeDeletedHasRelatedEntities<Subject.Models.Subject>()}: {subjectId}\n" +
                $"Powiązane encje: {relatedDocsNames}");

        db.Subjects.Remove(exist);

        try
        {
            if (await db.SaveChangesAsync() <= 0)
                return Result.Error($"{Errors.ObjectCannotBeDeleted<Subject.Models.Subject>()}: {subjectId}");
            return Result.OK($"{Errors.ObjectDeleted<Subject.Models.Subject>()}: {subjectId}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error(
                $"{Errors.ObjectCannotBeDeleted<Subject.Models.Subject>()}: {subjectId}\n\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error(
                $"{Errors.ObjectCannotBeDeleted<Subject.Models.Subject>()}: {subjectId}\n\n{e.Message}");
        }
    }
}