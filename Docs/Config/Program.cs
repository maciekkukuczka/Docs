Log.Logger = new LoggerConfiguration().SerilogConfig();

try
{
    Log.Logger.Information("DOCS STARTED!");

    var builder = WebApplication.CreateBuilder(args);


//DI
    builder.Services.AddServices(builder.Configuration);

    var app = builder.Build();


// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
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
    app.UseSerilogRequestLogging();
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

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