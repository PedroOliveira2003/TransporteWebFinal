using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Data;
using TransporteWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var conStr2 = builder.Configuration.GetConnectionString("conexao") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(conStr2));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Adiciona suporte para roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);

    // Atribuição de roles aos usuários
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var usersWithRoles = new Dictionary<string, string>
    {
        { "pedro2003_oliveira@outlook.com", "Funcionario" },
        { "estudante@teste.com.br", "Estudante" },
        { "motorista@teste.com.br", "Motorista" }
    };

    foreach (var userWithRole in usersWithRoles)
    {
        var user = await userManager.FindByEmailAsync(userWithRole.Key);
        if (user != null)
        {
            var result = await userManager.AddToRoleAsync(user, userWithRole.Value);
            if (result.Succeeded)
            {
                Console.WriteLine($"Usuário {userWithRole.Key} adicionado à role {userWithRole.Value} com sucesso.");
            }
            else
            {
                Console.WriteLine($"Erro ao adicionar usuário {userWithRole.Key} à role {userWithRole.Value}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine($"Usuário {userWithRole.Key} não encontrado.");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Função para criar roles
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Estudante", "Motorista", "Funcionario" };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
