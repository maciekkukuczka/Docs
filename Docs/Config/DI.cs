using Docs.Modules.Shortcuts;

namespace Docs.Config;

public static class DI
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        //SERILOG
        services.AddSerilog();
        
        // OPEN TELEMETRY
        
        //GLOBAL EXCEPTION HANDLER
        services.AddExceptionHandler<ExceptionHandlerMiddleware>();
        services.AddProblemDetails();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // IDENTITY
        services.AddIdentityServices();
        services.AddCascadingAuthenticationState();

        //HTTP
        services.AddHttpContextAccessor();

        // DB
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        if (environment.IsDevelopment())
        {
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(connectionString, x =>
                    {
                        x.MigrationsAssembly("Docs");
                        x.MigrationsHistoryTable("__EFMigrationsHistory", "Migrations.Sqlite");
                    });
                },
                lifetime: ServiceLifetime.Scoped);
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(connectionString, x =>
                    {
                        x.MigrationsAssembly("Docs");
                        x.MigrationsHistoryTable("__EFMigrationsHistory", "Migrations.Sqlite");
                    });
                }
            );
            
            // Log.Information("Using SQLite Database with Migration Assembly: {assembly}", typeof(Program).Assembly.FullName)
        }
        else
        {
            services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
                x =>
                {
                    x.MigrationsAssembly("Docs");
                    x.MigrationsHistoryTable("__EFMigrationsHistory", "Migrations.SqlServer");
                }));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
                x =>
                {
                    x.MigrationsAssembly("Docs");
                    x.MigrationsHistoryTable("__EFMigrationsHistory", "Migrations.SqlServer");
                }));
        }

        /*
        services.AddDbContextFactory<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        */

        services.AddDatabaseDeveloperPageExceptionFilter();

        // DATASEED
        services.AddTransient<DataSeed>();
        
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
        services.AddScoped<SubjectService>();
        services.AddScoped<SubjectVMService>();
        services.AddScoped<CategoriesVMService>();
        services.AddScoped(typeof(LinksService<>));
        services.AddScoped<ShortcutsService>();

        return services;
    }
}