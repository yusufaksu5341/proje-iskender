using ProjeIskender.Middlewares;

#if !_TEST
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

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
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.Run();

#else

var tester = new ProjeIskender.Tester();

tester.Init();
tester.RunAll();

#endif