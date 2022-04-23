using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SignalRWebApplication.Context;
using SignalRWebApplication.Hubs;
using SignalRWebApplication.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SignanlR service 
builder.Services.AddSignalR();

builder.Services.AddDbContext<DataBaseContext>(option =>
{
    option.UseSqlServer(builder.Configuration["ConnectionString"]);
});

builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => {
    options.LoginPath = "/Home/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Config Database context

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SiteChatHub>("/chathub");
app.MapHub<SupportHub>("/supporthub");

app.Run();
