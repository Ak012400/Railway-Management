using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;
using Railway_Management.Services;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 804857600; // 100 MB
});
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = 100;
    options.MultipartBodyLengthLimit = 804857600; // 100 MB
    options.ValueLengthLimit = 8096; // Default is 2048
});


builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMailService,MailServices>();
builder.Services.AddScoped<IOTPService,OTPHandler>();
builder.Services.AddScoped<IForgotPassword,ForgotPasswordService>();
builder.Services.AddScoped<ICustomers,CustomerServices>();
//builder.Services.AddScoped<IAzureOpenAIService, AzureOpenAiService>();
builder.Services.AddDbContextFactory<ConnectionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RailwayDbConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login"; // Login page path
        options.AccessDeniedPath = "/Home/Index"; // Access denied path
    });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ConnectionContext>();
    dbContext.Database.Migrate(); // Ye line saari pending migrations ko apply kar degi
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
