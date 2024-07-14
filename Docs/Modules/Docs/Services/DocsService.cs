namespace Docs.Modules.Docs.Services;

public class DocsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<Result<List<Doc>>> GetAllDocs()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var res = Result<List<Doc>>.OK(await dbContext.Docs
            .AsNoTracking()
            .ToListAsync());
        return res;
    }

    public async Task<Result<HashSet<Doc>>> GetDocBySubject(string? subjectId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        HashSet<Doc> result;
        if (string.IsNullOrWhiteSpace(subjectId))
        {
            var firstSubject = await db.Docs
                .Where(x=>x.Subjects.Any())
                .Select(x => x.Subjects.FirstOrDefault())
                .FirstOrDefaultAsync();

            if (firstSubject is null) return Result.Error<HashSet<Doc>>($"{Errors.ObjectNotExist<Subject>()}");
            
            result = await db.Docs.Where(x => x.Subjects.Any(x => x.Id == firstSubject.Id))
                .ToHashSetAsync();
        }
        else
        {
            result = await db.Docs.Where(x => x.Subjects.Any(x => x.Id == subjectId)).ToHashSetAsync();
        }

        return Result<HashSet<Doc>>.OK(result);
    }


    public async Task<Result<Doc>> GetDocByld(string id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var exist = await dbContext.Docs.FindAsync(id);
        if (exist == null) return Result.Error<Doc>($"{Errors.ObjectNotFound<Doc>()}: {id}");
        return Result<Doc>.OK(exist);
    }

    public async Task<Result> AddDoc(Doc newDoc)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existSubjects = dbContext.Subjects;

        foreach (var existSubject in existSubjects)
        {
            if (newDoc.Subjects.Any(x => x.Id == existSubject.Id))
            {
                var untracked = newDoc.Subjects.FirstOrDefault(x => x.Id == existSubject.Id);
                newDoc.Subjects.Remove(untracked);
                newDoc.Subjects.Add(existSubject);
            }
        }

        dbContext.Add(newDoc);
        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {newDoc.Title}");
        return Result.OK($"{Errors.ObjectSaved<Doc>()}: {newDoc.Title}");
    }

    public async Task<Result> UpdateDoc(Doc doc)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        if (doc is null) return Result.Error($"{Errors.ObjectNotExist<Doc>()}: {doc.Title}");

        var exist = await dbContext.Docs.FindAsync(doc.Id);
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Doc>()}: {doc.Title}");

        exist.Title = doc.Title;
        exist.ShortDescription = doc.ShortDescription;
        exist.Description = doc.Description;

        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {doc.Title}");
        return Result.OK($"{Errors.ObjectSaved<Doc>()}: {doc.Title}");
    }

    public async Task<Result> DeleteDoc(string docId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var exist = await dbContext.Docs.FindAsync(docId);
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Doc>()}:  {exist.Title}");
        dbContext.Docs.Remove(exist);
        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeDeleted<Doc>()}: {exist.Title}");
        return Result.OK($"{Errors.ObjectDeleted<Doc>()}: {exist.Title}");
    }
}