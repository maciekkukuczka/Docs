namespace Docs.Modules.Docs.Services;

public class SubjectService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    // GET
    public async Task<Result<HashSet<Subject>>> GetSubjects(string? userName)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.Subjects
            .Include(x => x.Docs)
            .Where(x => x.User.UserName.Equals(userName))
            .AsNoTracking()
            .ToHashSetAsync();
        if (result is null) return Result.Error<HashSet<Subject>>($"{Errors.ObjectNotFound<HashSet<Subject>>()}");
        return Result<HashSet<Subject>>.OK(result);
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
            if(saveResult <=0) return Result.Error($"{Errors.ObjectCannotBeSaved<Subject>()}: {subject.Name}");
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
}