using Microsoft.EntityFrameworkCore;
using MvcApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri("http://webapi:5000");
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    if (!context.Persons.Any())
    {
        context.Persons.AddRange(
            new Person { FullName = "Alice Johnson", BirthDate = new DateTime(1990, 5, 1) },
            new Person { FullName = "Bob Smith", BirthDate = new DateTime(1985, 11, 15) },
            new Person { FullName = "Charlie Davis", BirthDate = new DateTime(1978, 3, 30) },
            new Person { FullName = "Diana King", BirthDate = new DateTime(1995, 8, 12) },
            new Person { FullName = "Edward Lewis", BirthDate = new DateTime(2000, 1, 20) },
            new Person { FullName = "Fady Samy", BirthDate = new DateTime(2000, 1, 20) }
        );
        context.SaveChanges();
    }
}

app.MapGet("/persons", async (AppDbContext ctx) => {
    return await ctx.Persons.ToListAsync();
});
app.MapGet("/person", async (AppDbContext ctx) => {
    return await ctx.Persons.FirstOrDefaultAsync();

});

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
