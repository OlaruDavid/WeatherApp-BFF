using Microsoft.EntityFrameworkCore;
using WeatherFunction.Models;

namespace WeatherFunction.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<WeatherCurrent> WeatherCurrents { get; set; }
    public DbSet<WeatherHistory> WeatherHistories { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasIndex(c => c.CityName)
            .IsUnique();

        modelBuilder.Entity<WeatherCurrent>()
            .HasIndex(w => w.CityId)
            .IsUnique();

        modelBuilder.Entity<City>()
            .HasOne(c => c.NearestMainCity)
            .WithMany()
            .HasForeignKey(c => c.NearestMainCityId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<City>().HasData(
    // Nord-Est
    new City { Id = 1, CityName = "Iasi", Latitude = 47.16, Longitude = 27.58, IsMainCity = true },
    new City { Id = 2, CityName = "Suceava", Latitude = 47.63, Longitude = 26.25, IsMainCity = true },
    new City { Id = 3, CityName = "Bacau", Latitude = 46.57, Longitude = 26.91, IsMainCity = true },
    new City { Id = 4, CityName = "Vaslui", Latitude = 46.64, Longitude = 27.73, IsMainCity = true },
    new City { Id = 5, CityName = "Roman", Latitude = 46.92, Longitude = 26.93, IsMainCity = true },
    new City { Id = 6, CityName = "Focsani", Latitude = 45.70, Longitude = 27.18, IsMainCity = true },
    new City { Id = 7, CityName = "Galati", Latitude = 45.43, Longitude = 28.03, IsMainCity = true },
    new City { Id = 8, CityName = "Barlad", Latitude = 46.23, Longitude = 27.67, IsMainCity = true },
    // Sud-Est
    new City { Id = 9, CityName = "Constanta", Latitude = 44.18, Longitude = 28.65, IsMainCity = true },
    new City { Id = 10, CityName = "Tulcea", Latitude = 45.18, Longitude = 28.80, IsMainCity = true },
    new City { Id = 11, CityName = "Braila", Latitude = 45.27, Longitude = 27.96, IsMainCity = true },
    new City { Id = 12, CityName = "Buzau", Latitude = 45.15, Longitude = 26.83, IsMainCity = true },
    new City { Id = 13, CityName = "Medgidia", Latitude = 44.25, Longitude = 28.27, IsMainCity = true },
    new City { Id = 14, CityName = "Mangalia", Latitude = 43.82, Longitude = 28.58, IsMainCity = true },
    new City { Id = 15, CityName = "Slobozia", Latitude = 44.56, Longitude = 27.37, IsMainCity = true },
    // Sud
    new City { Id = 16, CityName = "Bucharest", Latitude = 44.43, Longitude = 26.10, IsMainCity = true },
    new City { Id = 17, CityName = "Ploiesti", Latitude = 44.94, Longitude = 26.02, IsMainCity = true },
    new City { Id = 18, CityName = "Targoviste", Latitude = 44.93, Longitude = 25.45, IsMainCity = true },
    new City { Id = 19, CityName = "Alexandria", Latitude = 43.97, Longitude = 25.33, IsMainCity = true },
    new City { Id = 20, CityName = "Calarasi", Latitude = 44.20, Longitude = 27.33, IsMainCity = true },
    // Sud-Vest
    new City { Id = 21, CityName = "Craiova", Latitude = 44.32, Longitude = 23.80, IsMainCity = true },
    new City { Id = 22, CityName = "Pitesti", Latitude = 44.85, Longitude = 24.87, IsMainCity = true },
    new City { Id = 23, CityName = "Ramnicu Valcea", Latitude = 45.10, Longitude = 24.37, IsMainCity = true },
    new City { Id = 24, CityName = "Targu Jiu", Latitude = 45.04, Longitude = 23.28, IsMainCity = true },
    new City { Id = 25, CityName = "Drobeta Turnu Severin", Latitude = 44.63, Longitude = 22.66, IsMainCity = true },
    // Vest
    new City { Id = 26, CityName = "Timisoara", Latitude = 45.75, Longitude = 21.23, IsMainCity = true },
    new City { Id = 27, CityName = "Arad", Latitude = 46.18, Longitude = 21.31, IsMainCity = true },
    new City { Id = 28, CityName = "Oradea", Latitude = 47.07, Longitude = 21.92, IsMainCity = true },
    new City { Id = 29, CityName = "Deva", Latitude = 45.88, Longitude = 22.91, IsMainCity = true },
    // Nord-Vest
    new City { Id = 30, CityName = "Cluj", Latitude = 46.77, Longitude = 23.59, IsMainCity = true },
    new City { Id = 31, CityName = "Baia Mare", Latitude = 47.66, Longitude = 23.58, IsMainCity = true },
    new City { Id = 32, CityName = "Satu Mare", Latitude = 47.79, Longitude = 22.88, IsMainCity = true },
    new City { Id = 33, CityName = "Zalau", Latitude = 47.19, Longitude = 23.06, IsMainCity = true },
    new City { Id = 34, CityName = "Bistrita", Latitude = 47.13, Longitude = 24.50, IsMainCity = true },
    // Centru
    new City { Id = 35, CityName = "Brasov", Latitude = 45.65, Longitude = 25.61, IsMainCity = true },
    new City { Id = 36, CityName = "Sibiu", Latitude = 45.80, Longitude = 24.15, IsMainCity = true },
    new City { Id = 38, CityName = "Alba Iulia", Latitude = 46.07, Longitude = 23.58, IsMainCity = true },
    new City { Id = 39, CityName = "Sfantu Gheorghe", Latitude = 45.86, Longitude = 25.79, IsMainCity = true },
    new City { Id = 40, CityName = "Miercurea Ciuc", Latitude = 46.36, Longitude = 25.80, IsMainCity = true },
    new City { Id = 41, CityName = "Sinaia", Latitude = 45.35, Longitude = 25.55, IsMainCity = true }
);
    }
}