using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMVCBlog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 1;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Auth/Login";
	options.AccessDeniedPath = "/Auth/AccessDenied";
	options.ExpireTimeSpan = TimeSpan.FromDays(7);
	options.SlidingExpiration = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	string adminEmail = "admin@gmail.com";
	string adminPassword = "admin";

	var existingAdminRole = await roleManager.FindByNameAsync("Admin");

	if (existingAdminRole == null)
	{
		await roleManager.CreateAsync(new IdentityRole("Admin"));
	}

	var existingAdminUser = await userManager.FindByEmailAsync(adminEmail);

	if (existingAdminUser == null)
	{
		var adminUser = new IdentityUser
		{
			UserName = adminEmail,
			Email = adminEmail
		};

		await userManager.CreateAsync(adminUser, adminPassword);
		await userManager.AddToRoleAsync(adminUser, "Admin");
	}
}

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();

app.Run();
