using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder => builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});


builder.Services.AddCookiePolicy(options => {
	options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None;
	options.MinimumSameSitePolicy = SameSiteMode.Lax;
	options.Secure = CookieSecurePolicy.SameAsRequest;
});

// Configura los servicios de autenticación en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //Routes Cookies
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";

    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
    options.ExpireTimeSpan = TimeSpan.FromDays (30);
    options.SlidingExpiration = true;
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
