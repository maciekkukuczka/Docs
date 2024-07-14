namespace Docs.Modules.Docs.Services;

public class SubjectService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<Result<HashSet<Subject>>> GetSubjects(string? userName)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.Subjects
            // .Include(x=>x.User)
            .Where(x => x.User.UserName.Equals(userName))
            .AsNoTracking()
            .ToHashSetAsync();
        if (result is null) return Result.Error<HashSet<Subject>>($"{Errors.ObjectNotFound<HashSet<Subject>>()}");
        return Result<HashSet<Subject>>.OK(result);
    }
}