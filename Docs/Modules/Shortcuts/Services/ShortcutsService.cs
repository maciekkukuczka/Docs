namespace Docs.Modules.Shortcuts;

public class ShortcutsService(IDbContextFactory<ApplicationDbContext>contextFactory):GenericService<Shortcut>(contextFactory)
{
    
}