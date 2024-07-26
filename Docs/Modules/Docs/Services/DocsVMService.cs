namespace Docs.Modules.Items.Services;

public class DocsVMService(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    HybridCache cache)
{
    HybridCache cache = cache;


// GET
    public async Task<Result<HashSet<DocVM>>> GetDocsByFilter(
        // CategoryVM category
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
                // query = query.Where(                 x => x.Subjects.Any(x=>string.Equals(x.Id,"d1ba5577-08cf-41c5-9f48-7d08e372b996"))
                query = query.Where(filter);
            }
        }

        /*if (category is not null)
        {
            query = query.Where(x => x.Categories.Any(x=>x.Id == category.Id));
        }*/

        // else query = query.Where(x => x.Subjects != null && x.Subjects.FirstOrDefault() != null);

        // query = query.Where(x => x.Categories.FirstOrDefault() == new Category());


        var queryResult = await query
            .AsNoTracking()
            // .ToListAsync();
            .ToHashSetAsync(cancellationToken: cancellationToken);


        var resultVM = queryResult.Select(x => DocVM.ToVM(x)).ToHashSet();
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

        dbContext.Add(newDoc);
        var saveResult = await dbContext.SaveChangesAsync();
        if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {newDoc.Title}");
        return Result.OK($"{Errors.ObjectSaved<Doc>()}: {newDoc.Title}");
    }


    // UPDATE
    public async Task<Result> UpdateDoc(DocVM? newDocVM)
    {
        var newDoc = DocVM.ToModel(newDocVM);
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existDoc = await dbContext.Docs
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == newDoc.Id);

        if (existDoc is null) return Result.Error($"{Errors.ObjectNotFound<Doc>()}: {newDoc.Title}");

        existDoc.Title = newDoc.Title;
        existDoc.ShortDescription = newDoc.ShortDescription;
        existDoc.Description = newDoc.Description;

        newDoc.Categories = newDoc.Categories.DistinctBy(x => x.Id).ToHashSet();

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

        dbContext.Update(existDoc);

        try
        {
            // dbContext.Entry(exist).State = EntityState.Modified;
            /*
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                Debug.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
            }
            */


            var saveResult = await dbContext.SaveChangesAsync();
            if (saveResult <= 0) return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {newDoc.Title}");
            return Result.OK($"{Errors.ObjectSaved<Doc>()}: {newDoc.Title}");
        }
        catch (DbUpdateException ex)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {ex.Message}");
        }
    }


    // DELETE
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

    // ------------------------------------

    #region Another

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

            if (firstSubject is null)
                return Result.Error<HashSet<DocVM>>($"{Errors.ObjectNotExist<Subject.Models.Subject>()}");

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

    #endregion
}