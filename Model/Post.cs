using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

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
		[DisplayName("Tytuł")]
		public string? Title { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy, HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime PostDate { get; set; }


		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy, HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime EditDate { get; set; }

		[DisplayName("Treść")]
		public string? Content { get; set; }

		[DisplayName("Polubienia")]
		public int Likes { get; set; } = 0;

	}
}
