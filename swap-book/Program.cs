
using swap_book.Models;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using swap_book.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("BookContext");

builder.Services.AddDbContext<BookContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IEmailSender, MailService>();
builder.Services.AddScoped<IFileService, FileService>();




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
