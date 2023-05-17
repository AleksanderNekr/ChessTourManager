using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChessTourContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services
       .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
       .AddEntityFrameworkStores<ChessTourContext>();

builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
                       "default",
                       "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
