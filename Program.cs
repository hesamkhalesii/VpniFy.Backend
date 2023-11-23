using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VpniFy.Backend.Common;
using VpniFy.Backend.Common.Settings;
using VpniFy.Backend.Contracts;
using VpniFy.Backend.Data;
using VpniFy.Backend.Model;
using VpniFy.Backend.Repositories;
using VpniFy.Backend.Services;

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


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
var commonAssembly = typeof(SiteSettings).Assembly;
var entitiesAssembly = typeof(IEntity).Assembly;
var dataAssembly = typeof(ApplicationDbContext).Assembly;
var servicesAssembly = typeof(JwtService).Assembly;
//var applicationAssmemly = typeof(IQuery<>).Assembly;
builder.Services.Scan(s =>
s.FromAssemblies(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly/*, applicationAssmemly*/)
.AddClasses(c => c.AssignableTo(typeof(IScopedDependency))
).AsImplementedInterfaces()
.WithScopedLifetime());

builder.Services.Scan(s =>
s.FromAssemblies(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly/*, applicationAssmemly*/)
.AddClasses(c => c.AssignableTo(typeof(ITransientDependency))
).AsImplementedInterfaces()
.WithTransientLifetime());

builder.Services.Scan(s =>
s.FromAssemblies(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly/*, applicationAssmemly*/)
.AddClasses(c => c.AssignableTo(typeof(ISingletonDependency))
).AsImplementedInterfaces()
.WithSingletonLifetime());

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
