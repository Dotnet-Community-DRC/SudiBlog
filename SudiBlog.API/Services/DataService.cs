namespace SudiBlog.API.Services;
public class DataService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<BlogUser> _userManager;
    public DataService(ApplicationDbContext dbContext,
        RoleManager<IdentityRole> roleManager,
        UserManager<BlogUser> userManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task ManageDataAsync()
    {
        await _dbContext.Database.MigrateAsync();

        await SeedRolesAsync();

        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (_dbContext.Roles.Any())
        {
            return;
        }

        foreach (var role in Enum.GetNames(typeof(BlogRole)))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

    }
    private async Task SeedUsersAsync()
    {
        if (_dbContext.Users.Any())
        {
            return;
        }

        var adminUser = new BlogUser
        {
            Email = "c",
            UserName = "sudisimbadav@gmail.com",
            FirstName = "Sudi",
            LastName = "Dav",
            DisplayName = "Admin",
            PhoneNumber = "+243817334881",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(adminUser, "Pa$$w0rd");
        await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());


        var modUser = new BlogUser
        {
            Email = "sudi@la-difference.com",
            UserName = "sudi@la-difference.com",
            FirstName = "Simba",
            LastName = "Petit",
            DisplayName = "Moderator",
            PhoneNumber = "+243996534078",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(modUser, "Pa$$w0rd");
        await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
    }
}