using Docs.Modules.Subject.Services;

namespace Docs.Config;

public static class DI
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //SERILOG
        services.AddSerilog();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // IDENTITY
        services.AddIdentityServices();

        //HTTP
        services.AddHttpContextAccessor();

        // DB
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlite(connectionString),
            lifetime: ServiceLifetime.Scoped);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        // IDENTITY2
        services.AddIdentityServices2();

        // CACHE
        services.AddHybridCache();
        // MUDBLAZOR
        services.AddMudBlazorServices();

        // APP
        services.AddScoped<AppState>();
        // services.AddScoped(typeof(GenericService<,>));
        services.AddScoped<DocsService>();
        services.AddScoped<DocsVMService>();
        services.AddScoped<SubjectService>();
        services.AddScoped<SubjectVMService>();
        services.AddScoped<CategoriesVMService>();

        return services;
    }
}