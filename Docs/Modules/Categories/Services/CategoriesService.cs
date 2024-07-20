namespace Docs.Modules.Subjects;

public class CategoriesService(IDbContextFactory<ApplicationDbContext> dbContextFactory) 
{
    // GET
    public async Task<Result<HashSet<Subject>>> GetSubjects(string? userName)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.Subjects
            .Include(x => x.Docs)
            // .Where(x => x.User.UserName.Equals(userName))
            .AsNoTracking()
            .ToHashSetAsync();

        if (result is null||result.Count <= 0) return Result.Error<HashSet<Subject>>($"{Errors.ObjectNotFound<HashSet<Subject>>()}");
        return Result.OK(result);
    }
    
    // ADD
    public async Task<Result> AddSubject(Subject subject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        await db.Subjects.AddAsync(subject);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}: {subject.Name}");
            return Result.OK($"{Errors.ObjectSaved<Subject>()}:{subject.Name}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}:{subject.Name}\n {e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}:{subject.Name}\n {e.Message}");
        }
    }


//UPDATE
    public async Task<Result> UpdateSubject(Subject subject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Subjects.FindAsync(subject.Id);
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Subject>()}: {subject.Name}");

        db.Subjects.Entry(exist).CurrentValues.SetValues(subject);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}: {subject.Name}");
            return Result.OK($"{Errors.ObjectSaved<Subject>()}: {subject.Name}");
        }
        catch (DbUpdateException ex)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}: {ex.Message}");
        }
    }


    // DELETE
    public async Task<Result> DeleteSubject(string subjectId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Subjects.Include(x => x.Docs)
            .FirstOrDefaultAsync(x => x.Id.Equals(subjectId));
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Subject>()}: {subjectId}");

        var relatedDocsNames = string.Join(",", exist.Docs.Select(x => x.Title));

        if (exist.Docs.Any())
            return Result.Error($"{Errors.ObjectCannotBeDeletedHasRelatedEntities<Subject>()}: {subjectId}\n" +
                                $"Powiązane encje: {relatedDocsNames}");

        db.Subjects.Remove(exist);

        try
        {
            if (await db.SaveChangesAsync() <= 0)
                return Result.Error($"{Errors.ObjectCannotBeDeleted<Subject>()}: {subjectId}");
            return Result.OK($"{Errors.ObjectDeleted<Subject>()}: {subjectId}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Errors.ObjectCannotBeDeleted<Subject>()}: {subjectId}\n\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Errors.ObjectCannotBeDeleted<Subject>()}: {subjectId}\n\n{e.Message}");
        }
    }
}