using Microsoft.EntityFrameworkCore;
using LandingAPI.Data;
using LandingAPI;
using LandingAPI.Interfaces;
using LandingAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IFilesRepository, FilesRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

var app = builder.Build();

// Seed data
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    SeedData(app);
}

void SeedData(IHost app)
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = scopeFactory.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetService<Seed>();
        seeder.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
