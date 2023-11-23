using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VpniFy.Backend.Common.Settings;
using VpniFy.Backend.Data;
using VpniFy.Backend.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options
		.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
var _siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
builder.Services.AddIdentity<User, Role>(identityOptions =>
{
    //Password Settings
    identityOptions.Password.RequireDigit = _siteSetting.IdentitySettings.PasswordRequireDigit;
    identityOptions.Password.RequiredLength = _siteSetting.IdentitySettings.PasswordRequiredLength;
    identityOptions.Password.RequireNonAlphanumeric = _siteSetting.IdentitySettings.PasswordRequireNonAlphanumeric; //#@!
    identityOptions.Password.RequireUppercase = _siteSetting.IdentitySettings.PasswordRequireUppercase;
    identityOptions.Password.RequireLowercase = _siteSetting.IdentitySettings.PasswordRequireLowercase;

    //UserName Settings
    identityOptions.User.RequireUniqueEmail = _siteSetting.IdentitySettings.RequireUniqueEmail;

    //Singin Settings
    //identityOptions.SignIn.RequireConfirmedEmail = false;
    //identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

    //Lockout Settings
    //identityOptions.Lockout.MaxFailedAccessAttempts = 5;
    //identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //identityOptions.Lockout.AllowedForNewUsers = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
