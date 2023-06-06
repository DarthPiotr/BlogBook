using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogBook.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BlogBook.Pages.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BlogBook.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogbookDbContext _context;
        private readonly UserManager<AppIdentityUser> _userManager;

        private AppIdentityUser? _currentUser;

		public PostsController(BlogbookDbContext context, UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string? query)
        {
            if (!string.IsNullOrEmpty(query))
            {

                var filteredContext = _context.Post
                    .Where(p => EF.Functions.Like(p.Content, $"%{query}%") || 
                                EF.Functions.Like(p.Title, $"%{query}%") || 
                                EF.Functions.Like(p.User.UserName, $"%{query}%"))
                .Include(p => p.User)
                .Include(p => p.Likes)
					.ThenInclude(l => l.User)
				.OrderByDescending(p => p.PostDate);

                return View(await filteredContext.ToListAsync());
            }

			var blogbookDbContext = _context.Post
                .Include(p => p.User)
				.Include(p => p.Likes)
					.ThenInclude(l => l.User)
				.OrderByDescending(p => p.PostDate);
			return View(await blogbookDbContext.ToListAsync());
		}

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.User)
				.Include(p => p.Likes)
					.ThenInclude(l => l.User)
				.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Post post)
        {
            var user = await GetCurrentUser();
			if (user != null)
            {
                post.PostDate = DateTime.Now;
                post.EditDate = post.PostDate;
                post.UserId = user.Id;
                post.User = user;

                ModelState.Clear();
                TryValidateModel(post, nameof(Post));
            }

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "/Posts");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
            return View(post);
        }

		// GET: Posts/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
				return NotFound();
            }

			if (!await CheckIfCurrentUser(post.UserId))
			{
				return RedirectToAction("Index", "/Posts");
			}

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,PostDate,Content,Likes")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

			if (!await CheckIfCurrentUser(post.UserId))
			{
				return RedirectToAction("Index", "/Posts");
			}

            var user = await GetCurrentUser();
			if (user != null)
            {
                post.User = user;
				post.EditDate = DateTime.Now;

				ModelState.Clear();
                TryValidateModel(post, nameof(Post));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "/Posts");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
            return View(post);
        }

		// GET: Posts/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.User)
				.Include(p => p.Likes)
					.ThenInclude(l => l.User)
				.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

			if (!await CheckIfCurrentUser(post.UserId))
            {
                return RedirectToAction("Index", "/Posts");
			}

			return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'BlogbookDbContext.Post'  is null.");
            }
            var post = await _context.Post
				.Include(p => p.Likes)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (post != null)
            {
				if (!await CheckIfCurrentUser(post.UserId))
				{
					return RedirectToAction("Index", "/Posts");
				}

				try
				{
                    foreach(var like in post.Likes)
                    {
                        _context.Like.Remove(like);
                    }
                    await _context.SaveChangesAsync();

					_context.Post.Remove(post);
                    await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PostExists(post.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
            }
            
            return RedirectToAction("Index", "/Posts");
		}

        // POST: Posts/LikeAction/5
		[HttpPost, ActionName("LikeAction")]
		[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeAction(int? id)
        {
			if (id == null || _context.Post == null)
			{
				return NotFound();
			}

			var post = await _context.Post
				.Include(p => p.User)
				.Include(p => p.Likes)
					.ThenInclude(l => l.User)
				.FirstOrDefaultAsync(p => p.Id == id);
			if (post == null)
			{
				return NotFound();
			}

			var user = await GetCurrentUser();
			if (user != null)
			{
                try
				{
                    var like = post.Likes.FirstOrDefault(like => like.User == user);
                    if (like != null)
                    {
						post.Likes.Remove(like);
					}
                    else
                    {
				        post.Likes.Add(new Like { Post = post, User = user});
                    }

				    _context.Update(post);
				    await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PostExists(post.Id))
					{
						return NotFound();
					}
				}
			}

            var headerPartialType = HttpContext.Request.Headers["x-partial-type"].ToString();
            var viewName = headerPartialType == "Details" ? "_PostDetailsPartial" : "_PostPartial";

			return PartialView(viewName, post);
        }

        private bool PostExists(int id)
        {
          return (_context.Post?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<AppIdentityUser> GetCurrentUser()
        {
            _currentUser ??= await _userManager.GetUserAsync(User);
            return _currentUser;
        }

        private async Task<bool> CheckIfCurrentUser(string id)
        {
			var user = await _userManager.GetUserAsync(User);
            return (user?.Id ?? "") == id;
		}
    }
}
