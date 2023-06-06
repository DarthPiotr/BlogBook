using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogBook.Model
{
	public class AppIdentityUser : IdentityUser
	{
		public ICollection<Like> Posts { get; set; } = new List<Like>();
	}
}
