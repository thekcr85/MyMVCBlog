using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCBlog.Models;

public class Comment
{
	[Key]
	public int Id { get; set; }
	[Required(ErrorMessage = "User name is required.")]
	[MaxLength(100, ErrorMessage = "User name cannot exceed 100 characters.")]
	public string UserName { get; set; } = string.Empty;
	[DataType(DataType.Date)]
	public DateTime CommentDate { get; set; } = DateTime.Now;
	[Required(ErrorMessage = "Content is required.")]
	public string Content { get; set; } = string.Empty;
	[ForeignKey(nameof(Post))]
	public int PostId { get; set; }
	public Post Post { get; set; } = null!;
}
