namespace Docs.Config;

public static class DI
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //SERILOG
        services.AddSerilog();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        services.AddAuthorization();

        //Password
        services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }
        );

        //HTTP
        services.AddHttpContextAccessor();
        
        // DB
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlite(connectionString),
            lifetime: ServiceLifetime.Scoped);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        // IDENTITY
        services.AddCascadingAuthenticationState();
        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();


        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


        // MUDBLAZOR
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        // APP
        services.AddSingleton<AppState>();
        services.AddScoped(typeof(GenericService<>));
        services.AddScoped<DocsService>();
        services.AddScoped<SubjectService>();

        return services;
    }
}