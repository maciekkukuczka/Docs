var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Config", "Json"))
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true)
    .Build();

Log.Logger = new LoggerConfiguration().SerilogConfig(configuration);

try
{
    Log.Logger.Information("DOCS STARTED!");

    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddConfiguration(configuration);

//DI
 
    builder.Services.AddServices(builder.Configuration);
    var app = builder.Build();


// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/errormid");

        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    // app.MapStaticAssets();
    app.UseSerilogRequestLogging();


    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        // .WithStaticAssets()
        .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    // SEED
    var scope=app.Services.CreateScope();
    var db=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    
    
    app.Run();
}
catch (Exception e)
{
    Log.Logger.Fatal(e, "\nDOCS TERMINATED!");
}
finally
{
    await Log.CloseAndFlushAsync();
}