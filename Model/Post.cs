using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlogBook.Model
{
	public class Post
	{
		[Required]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey("User")]
		public string UserId { get; set; }
		public IdentityUser User { get; set; } = null!;

		[Required]
		public string? Title { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
		public DateTime PostDate { get; set; } = DateTime.Now;

		public string? Content { get; set; }

		public int Likes { get; set; } = 0;

	}
}
