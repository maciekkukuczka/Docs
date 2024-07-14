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

        // MUDBLAZOR
        services.AddMudBlazorServices();

        // APP
        services.AddSingleton<AppState>();
        services.AddScoped(typeof(GenericService<>));
        services.AddScoped<DocsService>();
        services.AddScoped<SubjectService>();

        return services;
    }
}