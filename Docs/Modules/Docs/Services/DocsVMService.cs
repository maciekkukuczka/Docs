// ReSharper disable InconsistentNaming
namespace Docs.Modules.Docs;

public class DocsVMService(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    HybridCache cache)
{
    HybridCache cache = cache;

    public async Task<Result<HashSet<DocVM>>> GetAllDocs()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var res = await dbContext.Docs
            .AsNoTracking()
            .ToHashSetAsync();

        var resultVM = res.Select(x => DocVM.ToVM(x)).ToHashSet();
        return Result.OK(resultVM);
    }

    public async Task<Result<HashSet<DocVM>>> GetDocBySubject(string? subjectId,
        CancellationToken cancellationToken = default)
    {
        /*return await cache.GetOrCreateAsync(
            $"Docs", async cancel=>*/

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<Doc> res;
        if (string.IsNullOrWhiteSpace(subjectId))
        {
            var firstSubject = await db.Docs
                // .Where(x=>x.Subjects.Any())
                .Select(x => x.Subjects.FirstOrDefault())
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (firstSubject is null) return Result.Error<HashSet<DocVM>>($"{Errors.ObjectNotExist<Subject>()}");

            res = db.Docs.Where(x => x.Subjects.Any(x => x.Id == firstSubject.Id));
        }
        else
        {
            res = db.Docs.Where(x => x.Subjects.Any(x => x.Id == subjectId));
            // .ToHashSetAsync();
        }

        var result = await res
            .Include(x => x.Subjects)
            .AsNoTracking()
            .ToHashSetAsync(cancellationToken: cancellationToken);

        var resultVM = result.Select(x => DocVM.ToVM(x)).ToHashSet();
        return Result.OK(resultVM);
    }


    public async Task<Result<Doc>> GetDocByld(string id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var exist = await dbContext.Docs.FindAsync(id);
        if (exist == null) return Result.Error<Doc>($"{Errors.ObjectNotFound<Doc>()}: {id}");
        return Result.OK(exist);
    }

    //ADD
    // ReSharper disable once InconsistentNaming
    public async Task<Result> AddDoc(DocVM newDocVM)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var newDoc = DocVM.ToModel(newDocVM);
        var existSubjects = dbContext.Subjects;

        foreach (var existSubject in existSubjects)
        {
            if (newDoc.Subjects.Any(x => x.Id == existSubject.Id))
            {
                var untracked = newDoc.Subjects.FirstOrDefault(x => x.Id == existSubject.Id);
                if (untracked != null) newDoc.Subjects.Remove(untracked);
                newDoc.Subjects.Add(existSubject);
            }
        }

        dbContext.Add(newDoc);
        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {newDoc.Title}");
        return Result.OK($"{Errors.ObjectSaved<Doc>()}: {newDoc.Title}");
    }

    // UPDATE
    public async Task<Result> UpdateDoc(DocVM? docVM)
    {
        var doc = DocVM.ToModel(docVM);
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

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
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Doc>()}:  {exist?.Title}");
        dbContext.Docs.Remove(exist);
        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeDeleted<Doc>()}: {exist.Title}");
        return Result.OK($"{Errors.ObjectDeleted<Doc>()}: {exist.Title}");
    }
}