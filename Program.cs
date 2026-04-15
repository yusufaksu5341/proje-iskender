using System.Text;
using ProjeIskender.Middlewares;
using ProjeIskender.Services;
using ProjeIskender.Services.Implementation;

#if !_TEST
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Custom services
builder.Services.AddScoped<IUserService, TestUserService>(x => new TestUserService(Encoding.ASCII.GetBytes("test-key")));
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IProductService, TestProductService>();

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

app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<AuthorizationMiddleware>();

app.MapStaticAssets();

app.MapControllers();

app.Run();

#else

var tester = new ProjeIskender.Tester();

tester.Init();
tester.RunAll();

#endif
