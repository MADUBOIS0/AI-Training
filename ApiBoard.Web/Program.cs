using ApiBoard.Core.Entities;
using ApiBoard.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var databasePath = Path.Combine(builder.Environment.ContentRootPath, "api_board.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.ApiRequests.AnyAsync())
    {
        var utcNow = DateTimeOffset.UtcNow;

        var environment = new Environment
        {
            Name = "Sample",
            Notes = "Demo environment created at startup.",
            CreatedAt = utcNow,
            UpdatedAt = utcNow,
            Variables =
            {
                new EnvironmentVar
                {
                    Key = "BaseUrl",
                    Value = "https://localhost:5001",
                    IsSecret = false
                }
            }
        };

        var collection = new RequestCollection
        {
            Name = "Getting Started",
            Notes = "Sample collection created at startup.",
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };

        var request = new ApiRequest
        {
            Name = "Sample Health",
            Method = "GET",
            Url = "{{BaseUrl}}/health",
            HeadersJson = """{"Accept":"application/json"}""",
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };

        environment.Requests.Add(request);
        collection.Requests.Add(request);

        db.Environments.Add(environment);
        db.RequestCollections.Add(collection);

        await db.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
