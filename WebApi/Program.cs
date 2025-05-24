using Microsoft.EntityFrameworkCore;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors(builder =>
{
    builder.AddPolicy("allowreactorigin",
        policy => policy.WithOrigins("http://localhost:3000", "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            );
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(u =>
    {
        u.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        u.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
//app.UseRouting();
app.UseCors("allowreactorigin");

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
            new Person { FullName = "Edward Lewis", BirthDate = new DateTime(2000, 1, 20) }
        );
        context.SaveChanges();
    }
}

app.MapGet("/persons", async (AppDbContext ctx) => {
    return await ctx.Persons.ToListAsync();
    });
app.MapGet("/allpersons", async (AppDbContext ctx) =>
{
    return await ctx.Persons.ToListAsync();
});

app.MapGet("/person", async (AppDbContext ctx, string name ) => {
    ctx.Persons.Add(new Person { FullName = name, BirthDate = DateTime.Now });
    await ctx.SaveChangesAsync();
    return await ctx.Persons.ToListAsync();
});

app.MapGet("/debugdb", (AppDbContext ctx) =>
{
    var conn = ctx.Database.GetDbConnection();
    return new { conn.DataSource, conn.Database };
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
