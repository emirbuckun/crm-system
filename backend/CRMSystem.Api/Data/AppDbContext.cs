using CRMSystem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRMSystem.Api.Data;
public class AppDbContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    => modelBuilder.Entity<Customer>().Property(c => c.Id)
        .HasIdentityOptions(startValue: 4);

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        try {
            if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator) {
                if (!databaseCreator.CanConnect()) databaseCreator.Create();
                if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                if (Customers != null && !Customers.Any()) SeedData();
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    private void SeedData() {
        Customers.Add(new Customer {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Region = "North America",
            RegistrationDate = DateTime.SpecifyKind(new DateTime(2023, 06, 15), DateTimeKind.Utc)
        });
        Customers.Add(new Customer {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Region = "Europe",
            RegistrationDate = DateTime.SpecifyKind(new DateTime(2023, 06, 10), DateTimeKind.Utc)
        });
        Customers.Add(new Customer {
            Id = 3,
            FirstName = "Carlos",
            LastName = "Gomez",
            Email = "carlos.gomez@example.com",
            Region = "South America",
            RegistrationDate = DateTime.SpecifyKind(new DateTime(2023, 07, 22), DateTimeKind.Utc)
        });
        SaveChanges();
    }
}