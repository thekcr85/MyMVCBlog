using System.ComponentModel.DataAnnotations;

namespace MyMVCBlog.Models;

public class Category
{
	[Key]
	public int Id { get; set; }
	[Required(ErrorMessage = "Category name is required.")]
	[MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
	public string Name { get; set; } = string.Empty;
	[MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
	public string? Description { get; set; }
	public ICollection<Post> Posts { get; set; } = new List<Post>();
}
