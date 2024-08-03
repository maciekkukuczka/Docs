namespace Mac.Modules.DataSeed;

public class DataSeed(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration
)
{
    public async Task Seed(bool deleteDbBeforeSeed)
    {
    // CONFIG
        if (deleteDbBeforeSeed)
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

#if DEBUG
        /*if (pendingMigrations is not null && pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }*/
#endif

        if (await dbContext.Users.AnyAsync()) return;


        var isDbOk = await dbContext.Database.CanConnectAsync();

        if (isDbOk)
        {
            // ADD ROLE
            var roleName = "Admin";

            var createRoleResult = await CreateRoleAsync(roleName);
            if (createRoleResult is null)
            {
                Log.ForContext<DataSeed>().Debug("Rola {roleName} - OK", roleName);
            }
            else
            {
                if (createRoleResult.Succeeded)
                {
                    Log.ForContext<DataSeed>().Information("Utworzono role {roleName}", roleName);
                }
                else
                {
                    Log.ForContext<DataSeed>().Warning("Nie utworzono roli {roleName}\n Błąd:{error}", roleName,
                        createRoleResult.Errors.FirstOrDefault().Description);
                }
            }


            // ADD USER
            var admins = configuration.GetSection("Admins").Get<List<Admin>>();

            foreach (var admin in admins)
            {
                var createUserResult = await CreateUserAsync(admin.Email, admin.Email, admin.Password);
                if (createUserResult is null)
                {
                    Log.Logger.Debug("Użytkownik {adminEmail} - OK", admin.Email);
                }
                else
                {
                    if (createUserResult.Succeeded)
                    {
                        Log.Logger.ForContext<DataSeed>().Information("Utworzono użytkownia {adminEmail}", admin.Email);
                    }
                    else
                    {
                        Log.Logger.ForContext<DataSeed>().Warning(
                            "Nie utworzono użytkownika {adminEmail} \nBłąd:{error}", admin.Email,
                            createUserResult.Errors.FirstOrDefault().Description);
                    }
                }

                //ADD USER TO ROLE
                var addUserToRoleResult = await AddUserToRoleAsync(admin.Email, roleName);

                if (addUserToRoleResult is null)
                {
                    Log.Logger.ForContext<DataSeed>()
                        .Information(" {adminEmail} w roli {roleName}", admin.Email, roleName);
                }
                else
                {
                    if (!addUserToRoleResult.Succeeded)
                    {
                        Log.Logger.ForContext<DataSeed>().Warning(
                            "Nie dodano użytkownika {adminEmail} do roli {roleName}\nBłąd:{addUserToRoleResultErrors}",
                            admin.Email, roleName, addUserToRoleResult.Errors.FirstOrDefault().Description);
                    }
                    else
                    {
                        Log.Logger.ForContext<DataSeed>().Information(
                            "Dodano użytkownika {adminEmail} do roli {roleName}", admin.Email,
                            roleName);
                    }
                }
            }

            // await AddSteps();
        }


// METHODS----------------------------------------------------------------


        // CREATE ROLE
        async Task<IdentityResult?> CreateRoleAsync(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName)) return null;

            var role = new IdentityRole(roleName);

            return await roleManager.CreateAsync(role);
        }


//CREATE USER
        async Task<IdentityResult?> CreateUserAsync(string email, string name, string password)
        {
            if (await userManager.FindByEmailAsync(email) is not null) return null;

            var user = new ApplicationUser()
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            // await userStore.SetUserNameAsync(user, email, CancellationToken.None); -TO CHECK
            return await userManager.CreateAsync(user, password);
        }


// ADD USER TO ROLE
        async Task<IdentityResult?> AddUserToRoleAsync(string email, string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains(roleName))
            {
                return null;
            }

            return await userManager.AddToRoleAsync(user, roleName);
        }
    }
}

class Admin
{
    public string Email { get; set; }
    public string Password { get; set; }
}