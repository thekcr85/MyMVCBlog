using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMVCBlog.Models.ViewModels;

namespace MyMVCBlog.Controllers
{
	public class AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager) : Controller
	{
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(registerViewModel);
			}

			var user = new IdentityUser
			{
				UserName = registerViewModel.Email,
				Email = registerViewModel.Email
			};

			var result = await userManager.CreateAsync(user, registerViewModel.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			if (!roleManager.RoleExistsAsync("User").Result)
			{
				await roleManager.CreateAsync(new IdentityRole("User"));
			}

			await userManager.AddToRoleAsync(user, "User");

			await signInManager.SignInAsync(user, true); 

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}

			var user = await userManager.FindByEmailAsync(loginViewModel.Email);

			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return View(loginViewModel);
			}

			var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return View(loginViewModel);
			}

			return RedirectToAction("Index", "Post");

		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Post");
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}


	}
}
