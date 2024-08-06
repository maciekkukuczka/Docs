using Docs.Modules.Common.Result;

namespace Docs.Modules.Subjects.Services;

public class SubjectService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    // GET
    public async Task<Result<HashSet<Subject>>> GetSubjects(string? userName)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = db.Subjects
            .Include(x => x.Docs)
            // .Where(x => x.User.UserName.Equals(userName))
            .AsNoTracking().ToHashSet();

        // var result = await queryable.ToHashSetByEnvironment();
        
        if (result is null || result.Any())
            return Result.Error<HashSet<Subject>>($"{Messages.ObjectNotFound<HashSet<Models.Subject>>()}");
        return Result.OK(result);
    }

    // ADD
    public async Task<Result> AddSubject(Models.Subject subject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        await db.Subjects.AddAsync(subject);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0)
                return Result.Error($"{Messages.ObjectCannotBeSaved<Models.Subject>()}: {subject.Name}");
            return Result.OK($"{Messages.ObjectSaved<Models.Subject>()}:{subject.Name}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Models.Subject>()}:{subject.Name}\n {e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Models.Subject>()}:{subject.Name}\n {e.Message}");
        }
    }


//UPDATE
    public async Task<Result> UpdateSubject(Models.Subject subject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Subjects.FindAsync(subject.Id);
        if (exist is null) return Result.Error($"{Messages.ObjectNotFound<Models.Subject>()}: {subject.Name}");

        db.Subjects.Entry(exist).CurrentValues.SetValues(subject);

        try
        {
            var saveResult = await db.SaveChangesAsync();
            if (saveResult <= 0)
                return Result.Error($"{Messages.ObjectCannotBeSaved<Models.Subject>()}: {subject.Name}");
            return Result.OK($"{Messages.ObjectSaved<Models.Subject>()}: {subject.Name}");
        }
        catch (DbUpdateException ex)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Models.Subject>()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Models.Subject>()}: {ex.Message}");
        }
    }


    // DELETE
    public async Task<Result> DeleteSubject(string subjectId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var exist = await db.Subjects.Include(x => x.Docs)
            .FirstOrDefaultAsync(x => x.Id.Equals(subjectId));
        if (exist is null) return Result.Error($"{Messages.ObjectNotFound<Models.Subject>()}: {subjectId}");

        var relatedDocsNames = string.Join(",", exist.Docs.Select(x => x.Title));

        if (exist.Docs.Any())
            return Result.Error($"{Messages.ObjectCannotBeDeletedHasRelatedEntities<Models.Subject>()}: {subjectId}\n" +
                                $"Powiązane encje: {relatedDocsNames}");

        db.Subjects.Remove(exist);

        try
        {
            if (await db.SaveChangesAsync() <= 0)
                return Result.Error($"{Messages.ObjectCannotBeDeleted<Models.Subject>()}: {subjectId}");
            return Result.OK($"{Messages.ObjectDeleted<Models.Subject>()}: {subjectId}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeDeleted<Models.Subject>()}: {subjectId}\n\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeDeleted<Models.Subject>()}: {subjectId}\n\n{e.Message}");
        }
    }
}