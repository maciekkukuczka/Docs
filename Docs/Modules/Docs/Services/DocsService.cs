namespace Docs.Modules.Docs.Services;

public class DocsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<Result<List<Doc>>> GetAllDocs()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return Result<List<Doc>>.OK(await dbContext.Docs.AsNoTracking().ToListAsync());
    }

    public async Task<Result> AddDoc(Doc doc)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Add(doc);
       var saveResult= await dbContext.SaveChangesAsync();
        if (saveResult<=0)return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {doc.Title}");
        return Result.OK($"{Errors.ObjectSaved<Doc>()}: {doc.Title}" );
    }

    public async Task<Result> UpdateDoc(Doc doc)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        if (doc is null) return Result.Error($"{Errors.ObjectNotExist<Doc>()}: {doc.Title}" );

        var exist = await dbContext.Docs.FindAsync(doc.Id);
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Doc>()}: {doc.Title}" );
        
        exist.Title = doc.Title;
        
        var saveResult=await dbContext.SaveChangesAsync();
        if (saveResult<=0)return Result.Error($"{Errors.ObjectCannotBeSaved<Doc>()}: {doc.Title}");
        return Result.OK($"{Errors.ObjectSaved<Doc>()}: {doc.Title}");
    }

    public async Task<Result> DeleteDoc(string docId)
    {
        await using var dbContext=await dbContextFactory.CreateDbContextAsync();
        var exist = await dbContext.Docs.FindAsync(docId);
        if (exist is null) return Result.Error($"{Errors.ObjectNotFound<Doc>()}:  {exist.Title}");
        dbContext.Docs.Remove(exist);
        var saveResult= await dbContext.SaveChangesAsync();
        if (saveResult<=0)return Result.Error($"{Errors.ObjectCannotBeDeleted<Doc>()}: {exist.Title}");
        return Result.OK($"{Errors.ObjectDeleted<Doc>()}: {exist.Title}" );

    }
}