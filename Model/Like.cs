using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogBook.Model
{
	[PrimaryKey(nameof(UserId), nameof(PostId))]
	public class Like
	{
		//[ForeignKey("User")]
		public string? UserId { get; set; }
		public AppIdentityUser? User { get; set; } = null!;

		//[ForeignKey("Post")]
		//[Required]
		public int PostId { get; set; }
		public Post Post { get; set; } = null!;
	}
}
