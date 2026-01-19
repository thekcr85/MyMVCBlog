using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCBlog.Models;

public class Post
{
	[Key]
	public int Id { get; set; }
	[Required(ErrorMessage = "Title is required.")]
	[MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
	public string Title { get; set; } = string.Empty;
	[Required(ErrorMessage = "Content is required.")]
	public string Content { get; set; } = string.Empty;
	[Required(ErrorMessage = "Author is required.")]
	[MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
	public string Author { get; set; } = string.Empty;
	[ValidateNever]
	public string? FeatureImagePath { get; set; }
	[DataType(DataType.Date)]
	public DateTime PublishedDate { get; set; } = DateTime.Now;
	[ForeignKey(nameof(Category))]
	[Display(Name = "Category")]
	public int CategoryId { get; set; }
	[ValidateNever]
	public Category Category { get; set; } = null!;
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
