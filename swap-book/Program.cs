using swap_book.Models;
using swap_book.Services;
using swap_book;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(LangResource));
    });

builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk"),
    };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
string connection = builder.Configuration.GetConnectionString("BookContext");


builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IEmailSender, MailService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Use((context, next) =>
{
    Log.Information($"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString} [:] " +
                    $"T:{DateTime.Now}, " +
                    $"IP:{context.Connection.RemoteIpAddress}");
    return next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
