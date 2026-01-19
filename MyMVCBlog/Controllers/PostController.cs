using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMVCBlog.Data;
using MyMVCBlog.Models;
using MyMVCBlog.Models.ViewModels;

namespace MyMVCBlog.Controllers;

[Authorize]
public class PostController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment) : Controller
{
	[HttpGet]
	[AllowAnonymous]
	public IActionResult Index(int? categoryId)
	{
		var postQuery = applicationDbContext.Posts.Include(p => p.Category).AsQueryable();
		if (categoryId.HasValue)
		{
			postQuery = postQuery.Where(p => p.CategoryId == categoryId.Value);
		}
		var posts = postQuery.ToList();
		ViewBag.Categories = applicationDbContext.Categories.ToList();
		return View(posts);
	}

	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		if (id < 1)
		{
			return BadRequest();
		}

		var post = await applicationDbContext.Posts
			.Include(p => p.Category)
			.Include(p => p.Comments)
			.FirstOrDefaultAsync(p => p.Id == id);
		if (post == null)
		{
			return NotFound();
		}
		return View(post);
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public IActionResult Create()
	{
		var postViewModel = new PostViewModel
		{
			Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList()
		};

		return View(postViewModel);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create(PostViewModel postViewModel)
	{
		if (!ModelState.IsValid)
		{
			postViewModel.Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			return View(postViewModel);
		}

		if (postViewModel.FeatureImage == null || postViewModel.FeatureImage.Length == 0)
		{
			ModelState.AddModelError("FeatureImage", "Please select an image file.");
			postViewModel.Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			return View(postViewModel);
		}

		var inputFileExtension = Path.GetExtension(postViewModel.FeatureImage.FileName).ToLower();
		var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".avif" };
		if (!allowedExtensions.Contains(inputFileExtension))
		{
			ModelState.AddModelError("FeatureImage", "Invalid image format. Please upload a JPG, JPEG, PNG, GIF, WebP, or AVIF image.");
			postViewModel.Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			return View(postViewModel);
		}

		try
		{
			var fileName = await UploadFileToFolder(postViewModel.FeatureImage);
			postViewModel.Post.FeatureImagePath = $"/images/{fileName}";
			applicationDbContext.Posts.Add(postViewModel.Post);
			await applicationDbContext.SaveChangesAsync();
			return RedirectToAction("Index", "Home");
		}
		catch (InvalidOperationException ex)
		{
			ModelState.AddModelError("FeatureImage", ex.Message);
			postViewModel.Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			return View(postViewModel);
		}
		catch (Exception)
		{
			ModelState.AddModelError("", "An error occurred while saving the post. Please try again.");
			postViewModel.Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			return View(postViewModel);
		}
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Edit(int id)
	{
		if (id < 1)
		{
			return NotFound();
		}

		var post = await applicationDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
		if (post == null)
		{
			return NotFound();
		}

		var editViewModel = new EditViewModel
		{
			Post = post,
			Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList()
		};

		return View(editViewModel);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Edit(EditViewModel editViewModel)
	{
		if (!ModelState.IsValid)
		{
			editViewModel.Categories = applicationDbContext.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			return View(editViewModel);
		}

		// Load original as no-tracking so we can inspect existing FeatureImagePath
		var originalPost = await applicationDbContext.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editViewModel.Post.Id);

		if (originalPost == null)
		{
			return NotFound();
		}

		// Preserve existing image if no new file uploaded
		if (editViewModel.FeatureImage == null || editViewModel.FeatureImage.Length == 0)
		{
			editViewModel.Post.FeatureImagePath = originalPost.FeatureImagePath;
		}
		else
		{
			var inputFileExtension = Path.GetExtension(editViewModel.FeatureImage.FileName).ToLower();
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".avif" };

			if (!allowedExtensions.Contains(inputFileExtension))
			{
				ModelState.AddModelError("FeatureImage", "Invalid image format. Please upload a JPG, JPEG, PNG, GIF, WebP, or AVIF image.");
				editViewModel.Categories = applicationDbContext.Categories
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name
					}).ToList();
				return View(editViewModel);
			}

			try
			{
				var existingFilePath = Path.Combine(webHostEnvironment.WebRootPath, "images",
					Path.GetFileName(originalPost.FeatureImagePath ?? string.Empty));
				if (System.IO.File.Exists(existingFilePath))
				{
					System.IO.File.Delete(existingFilePath);
				}
			}
			catch (Exception)
			{
				// Continue even if old file deletion fails
			}

			var fileName = await UploadFileToFolder(editViewModel.FeatureImage);
			editViewModel.Post.FeatureImagePath = $"/images/{fileName}";
		}

		// Ensure the Id is preserved (should come from the form)
		editViewModel.Post.Id = originalPost.Id;

		// Attach and mark modified by calling Update since originalPost was loaded no-tracking
		applicationDbContext.Posts.Update(editViewModel.Post);
		await applicationDbContext.SaveChangesAsync();
		return RedirectToAction("Index");
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(int id)
	{
		if (id < 1)
		{
			return NotFound();
		}
		var post = await applicationDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
		if (post == null)
		{
			return NotFound();
		}
		return View(post);
	}

	[HttpPost, ActionName("Delete")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> DeleteConfirm(int id)
	{
		var post = await applicationDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

		if (string.IsNullOrEmpty(post?.FeatureImagePath) == false)
		{
			try
			{
				var existingFilePath = Path.Combine(webHostEnvironment.WebRootPath, "images",
					Path.GetFileName(post.FeatureImagePath));
				if (System.IO.File.Exists(existingFilePath))
				{
					System.IO.File.Delete(existingFilePath);
				}
			}
			catch (Exception)
			{
				// Continue even if file deletion fails
			}
		}
		if (post == null)
		{
			return NotFound();
		}

		applicationDbContext.Posts.Remove(post);
		await applicationDbContext.SaveChangesAsync();
		return RedirectToAction("Index");
	}

	[Authorize]
	public async Task<JsonResult> AddComment([FromBody] Comment comment)
	{
		if (comment == null)
		{
			return Json(new { success = false, error = "Invalid comment data" });
		}

		try
		{
			comment.CommentDate = DateTime.Now;
			applicationDbContext.Comments.Add(comment);
			await applicationDbContext.SaveChangesAsync();
			return Json(new
			{
				success = true,
				userName = comment.UserName,
				commentDate = comment.CommentDate.ToString("MMM dd yyyy"),
				content = comment.Content
			});
		}
		catch (Exception)
		{
			return Json(new
			{
				success = false,
				error = "An error occurred while saving the comment"
			});
		}
	}

	private async Task<string> UploadFileToFolder(IFormFile featureImage)
	{
		var fileExtension = Path.GetExtension(featureImage.FileName).ToLower();
		var fileName = $"{Guid.NewGuid()}{fileExtension}";
		var imagesFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
		if (!Directory.Exists(imagesFolderPath))
		{
			Directory.CreateDirectory(imagesFolderPath);
		}
		var filePath = Path.Combine(imagesFolderPath, fileName);
		await using var fileStream = new FileStream(filePath, FileMode.Create);
		await featureImage.CopyToAsync(fileStream);
		return fileName;
	}
}
