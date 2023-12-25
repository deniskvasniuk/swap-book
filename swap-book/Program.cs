using swap_book.Models;
using swap_book.Services;
using swap_book;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Serilog;
using Microsoft.AspNetCore.Identity;
using XLocalizer.Routing;
using XLocalizer.Xml;
using XLocalizer;
using XLocalizer.Translate;
using XLocalizer.Translate.MyMemoryTranslate;
using swap_book.LocalizationResources;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddXLocalizer<LocSource, MyMemoryTranslateService>(ops =>
    {
        ops.ResourcesPath = "LocalizationResources";
        ops.AutoAddKeys = true;
        ops.AutoTranslate = true;
        ops.TranslateFromCulture = "en";
        ops.UseExpressMemoryCache = true;
    })
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(LangResource));
    });

builder.Services.Configure<RequestLocalizationOptions>(ops =>
{
	var cultures = new CultureInfo[] {
		new CultureInfo("en"),
		new CultureInfo("uk"),
		   }; ops.SupportedCultures = cultures;
	ops.SupportedUICultures = cultures;
	ops.DefaultRequestCulture = new RequestCulture("en");
    ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
});

string connection = builder.Configuration.GetConnectionString("BookContext");


builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IEmailSender, MailService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddSingleton<IXResourceProvider, XmlResourceProvider>();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<ITranslator, MyMemoryTranslateService>();

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
    name: "publicProfile",
    pattern: "user/{publicProfileLink}",
    defaults: new { controller = "User", action = "PublicProfile" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "book/{bookUrl}",
	defaults: new { controller = "Offers", action = "BookPage" }
);
app.MapControllerRoute(
    name: "default",
    pattern: "/msg/sendMessage",
    defaults: new { controller = "User", action = "SendMessage" }
);


using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}
app.MapRazorPages();
app.Run();
