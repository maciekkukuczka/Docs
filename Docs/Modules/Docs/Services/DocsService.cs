namespace Docs.Modules.Items.Services;

public class DocsService(
    IDbContextFactory<ApplicationDbContext> dbContextFactory)
{

// GET
    public async Task<Result<HashSet<DocVM>>> GetDocsByFilter(
        HashSet<Expression<Func<Doc, bool>>>? filters
        , HashSet<Expression<Func<Doc, object>>>? includes
        , CancellationToken cancellationToken = default
    )
    {
        /*return await cache.GetOrCreateAsync(
            $"Docs", async cancel=>*/

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        IQueryable<Doc> query = db.Docs;

        //Includes
        if (includes is not null && includes.Count > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }


        //Filters
        if (filters is not null && filters.Count > 0)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }


        var queryResult =  query.AsNoTracking().ToHashSet();
            // .ToHashSetByEnvironment();


        var resultVM = queryResult.Select(x => DocVM.ToVM(x)).ToHashSet();
        return Result.OK(resultVM);
    }


    public async Task<Result<Doc>> GetDocByld(string id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var exist = await dbContext.Docs.FindAsync(id);
        if (exist == null) return Result.Error<Doc>($"{Messages.ObjectNotFound<Doc>()}: {id}");
        return Result.OK(exist);
    }


    //ADD
    public async Task<Result> AddDoc(DocVM newDocVM)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var newDoc = DocVM.ToModel(newDocVM);
        var existSubjects = dbContext.Subjects;


        // Subjects
        foreach (var existSubject in existSubjects)
        {
            if (newDoc.Subjects.Any(x => x.Id == existSubject.Id))
            {
                var untracked = newDoc.Subjects.FirstOrDefault(x => x.Id == existSubject.Id);
                if (untracked != null) newDoc.Subjects.Remove(untracked);
                newDoc.Subjects.Add(existSubject);
            }
        }


        // Categories
        var existCategories = dbContext.Categories;
        foreach (var existCategory in existCategories)
        {
            if (newDoc.Categories.Any(x => x.Id == existCategory.Id))
            {
                var untracked = newDoc.Categories.FirstOrDefault(x => x.Id == existCategory.Id);
                if (untracked != null) newDoc.Categories.Remove(untracked);
                newDoc.Categories.Add(existCategory);
            }
        }


        // Links
        dbContext.Links.AddRangeAsync(newDoc.Links);

        dbContext.Add(newDoc);

        try
        {
            var saveResult = await dbContext.SaveChangesAsync();
            return saveResult < 0
                ? Result.Error($"{Messages.ObjectCannotBeSaved<Doc>()}: {newDoc.Id}")
                : Result.OK($"{Messages.ObjectSaved<Doc>()}: {newDoc.Id}");
        }
        catch (DbUpdateException e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Doc>()}: {newDoc.Id}\n\n{e.Message}");
        }
        catch (Exception e)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Doc>()}: {newDoc.Id}\n\n{e.Message}");
        }
    }


    // UPDATE
    public async Task<Result> UpdateDoc(DocVM? newDocVM)
    {
        var newDoc = DocVM.ToModel(newDocVM);
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existDoc = await dbContext.Docs
            .Include(x => x.Categories)
            .Include(x => x.Links)
            .FirstOrDefaultAsync(x => x.Id == newDoc.Id);

        if (existDoc is null) return Result.Error($"{Messages.ObjectNotFound<Doc>()}: {newDoc.Title}");

        //Common
        existDoc.Title = newDoc.Title;
        existDoc.ShortDescription = newDoc.ShortDescription;
        existDoc.Description = newDoc.Description;


        //Categories
//! UWAGA SPRAWDZIC CZY POTRZEBNE
        // newDoc.Categories = newDoc.Categories.DistinctBy(x => x.Id).ToHashSet();

        var existCategories = dbContext.Categories.AsQueryable();

        await existCategories.ForEachAsync(e =>
        {
            if (newDoc.Categories.Any(n => n.Id == e.Id))
                // && !existDoc.Categories.Any(ed => ed.Id == e.Id))
            {
                var untracked = newDoc.Categories.FirstOrDefault(n => n.Id == e.Id);
                newDoc.Categories.Remove(untracked);
                newDoc.Categories.Add(e);
            }
        });

        existDoc.Categories = newDoc.Categories.ToHashSet();

        //Links
        var existLinks = dbContext.Links.AsQueryable();
        existLinks.ForEachAsync(e =>
        {
            if (newDoc.Links.Any(n => n.Id == e.Id))
            {
                var untracked = newDoc.Links.FirstOrDefault(n => n.Id == e.Id);
                newDoc.Links.Remove(untracked);
                newDoc.Links.Add(e);
            }
        });

        /*
        Stopwatch sw = new();
        try
        {
            sw.Start();
        }
        finally
        {
            sw.Stop();
            Console.WriteLine($"SPEED! Updated Doc: {sw.ElapsedMilliseconds}ms");
        }
        */

        existDoc.Links = newDoc.Links.ToHashSet();

        dbContext.Update(existDoc);

        try
        {
            // dbContext.Entry(exist).State = EntityState.Modified;

            var saveResult = await dbContext.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Messages.ObjectCannotBeSaved<Doc>()}: {newDoc.Title}");
            return Result.OK($"{Messages.ObjectSaved<Doc>()}: {newDoc.Title}");
        }
        catch (DbUpdateException ex)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Doc>()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Error($"{Messages.ObjectCannotBeSaved<Doc>()}: {ex.Message}");
        }
    }


    // DELETE
    public async Task<Result> DeleteDoc(string docId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var exist = await dbContext.Docs.FindAsync(docId);
        if (exist is null) return Result.Error($"{Messages.ObjectNotFound<Doc>()}:  {exist?.Title}");
        dbContext.Docs.Remove(exist);
        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Messages.ObjectCannotBeDeleted<Doc>()}: {exist.Title}");
        return Result.OK($"{Messages.ObjectDeleted<Doc>()}: {exist.Title}");
    }

    // ------------------------------------

    #region Another

    public async Task<Result<HashSet<DocVM>>> GetAllDocs()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var res = dbContext.Docs
            .AsNoTracking()
            .ToHashSet();
        
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

            if (firstSubject is null)
                return Result.Error<HashSet<DocVM>>($"{Messages.ObjectNotExist<Subjects.Models.Subject>()}");

            res = db.Docs.Where(x => x.Subjects.Any(x => x.Id == firstSubject.Id));
        }
        else
        {
            res = db.Docs.Where(x => x.Subjects.Any(x => x.Id == subjectId));
            // .ToHashSetAsync();
        }

        var result =  res
            .Include(x => x.Subjects)
            .AsNoTracking()
            .ToHashSet();

        var resultVM = result.Select(x => DocVM.ToVM(x)).ToHashSet();
        return Result.OK(resultVM);
    }

    #endregion
}