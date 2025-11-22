using BookBuddy.Data;
using BookBuddy.Models;
using BookBuddy.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add Entity Framework with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=bookbuddy.db"),
    ServiceLifetime.Singleton);  // DODAJ TO - naj bo tudi Singleton

// Register DataStore for dependency injection
builder.Services.AddSingleton<DataStore>();

var app = builder.Build();

// Create database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // NALOŽI PODATKE V DATASTORE
    var dataStore = scope.ServiceProvider.GetRequiredService<DataStore>();

    // Pokliči LoadInitialData preko reflection
    var method = typeof(DataStore).GetMethod("LoadInitialData",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    method?.Invoke(dataStore, null);
}

// Configure the HTTP request pipeline.
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