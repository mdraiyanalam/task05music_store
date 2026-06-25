using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MusicStore.DataGeneration.Generators;
using MusicStore.DataGeneration.Services;
using task05MusicStoreShowcase.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services with factory (to provide seed)
builder.Services.AddScoped<SongGenerator>(sp =>
    new SongGenerator(1234567890123456789L)); // Default seed

builder.Services.AddScoped<AudioService>(sp =>
    new AudioService(1234567890123456789L)); // Default seed

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Music Store API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.Run();