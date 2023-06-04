using BlogBook.Model;
using BlogBook.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var movieApiKey = builder.Configuration["Movies:ServiceApiKey"];

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BlogbookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogBookContext") ?? throw new InvalidOperationException("Connection string 'BlogBookContext' not found.")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlogbookDbContext>();
builder.Services.ConfigureApplicationCookie(config => config.LoginPath = "/Login");

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<BlogbookDbContext>()
				.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);

builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.ConfigureApplicationCookie(o => {
	o.ExpireTimeSpan = TimeSpan.FromDays(5);
	o.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
