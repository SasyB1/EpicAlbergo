using EpicAlbergo;
using EpicAlbergo.Services;
using EpicAlbergo.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddScoped<IReservationService, ReservationService>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddSingleton<IFiscalCodeService, FiscalCodeService>()
    .AddSingleton<ICsvCityService, CsvCityService>()
    .AddScoped<IRoomService, RoomService>()
    .AddScoped<IUserService, UserService>();

builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth";
        options.AccessDeniedPath = "/Home/Index";
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(Policies.IsAdmin, policy => policy.RequireRole("admin"));
        
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
