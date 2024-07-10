namespace Docs.Modules.Docs.Services;

public class DocsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<Result<List<Doc>>> GetAllDocs()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return Result<List<Doc>>.OK(await dbContext.Docs.ToListAsync());
    }

    public async Task<Result> AddDoc(Doc doc)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        context.Add(doc);
        await context.SaveChangesAsync();
        return Result.OK();
    }
}