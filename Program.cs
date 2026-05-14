using System.Text;
using Microsoft.EntityFrameworkCore;
using ProjeIskender.Context;
using ProjeIskender.Middlewares;
using ProjeIskender.Models.Account;
using ProjeIskender.Services;
using ProjeIskender.Services.Implementation;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#if !_TEST
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json"); // config.json eklemeyi unutmayın!

JwtToken.LoadKey(builder.Configuration["JwtKey"]!);
if (!Directory.Exists("./resource"))
{
    Directory.CreateDirectory("resource");
}

// Add services to the container.
builder.Services.AddDbContext<IskenderContext>(x => x.UseNpgsql(builder.Configuration["ConnectionString"]!));
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Custom services
builder.Services.AddScoped<IUserService, UserService>(x => new UserService(x.GetService<IskenderContext>(), Encoding.ASCII.GetBytes("test-key")));
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<AuthorizationMiddleware>();
app.UseMiddleware<ContentAcceptMiddleware>();

app.MapStaticAssets();

app.MapControllers();

app.Run();

#else

var tester = new ProjeIskender.Tester();

tester.Init();
tester.RunAll();

#endif