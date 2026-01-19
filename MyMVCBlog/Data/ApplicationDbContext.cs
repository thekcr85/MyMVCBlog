using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMVCBlog.Models;

namespace MyMVCBlog.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{

	}

	public DbSet<Post> Posts { get; set; } = null!;
	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Comment> Comments { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Category>().HasData
			(
			new Category { Id = 1, Name = "Technology", Description = "Posts related to technology." },
			new Category { Id = 2, Name = "Health", Description = "Posts related to health." },
			new Category { Id = 3, Name = "Lifestyle", Description = "Posts related to lifestyle." }
		);
		modelBuilder.Entity<Post>().HasData
			(
			new Post
			{
				Id = 1,
				Title = "The Rise of AI in Everyday Life",
				Content = "Artificial Intelligence (AI) is becoming increasingly prevalent in our daily lives...",
				Author = "Jane Doe",
				PublishedDate = new DateTime(2024, 1, 15),
				CategoryId = 1,
				FeatureImagePath = "images/ai-rise.jpg"
			},
			new Post
			{
				Id = 2,
				Title = "10 Tips for a Healthier Lifestyle",
				Content = "Living a healthy lifestyle doesn't have to be complicated. Here are 10 simple tips...",
				Author = "John Smith",
				PublishedDate = new DateTime(2024, 2, 10),
				CategoryId = 2,
				FeatureImagePath = "images/healthy-lifestyle.jpg"
			},
			new Post
			{
				Id = 3,
				Title = "Exploring Minimalist Living",
				Content = "Minimalism is more than just a design trend; it's a lifestyle choice that emphasizes simplicity...",
				Author = "Alice Johnson",
				PublishedDate = new DateTime(2024, 3, 5),
				CategoryId = 3,
				FeatureImagePath = "images/minimalist-living.jpg"
			}
			);
	}
}
