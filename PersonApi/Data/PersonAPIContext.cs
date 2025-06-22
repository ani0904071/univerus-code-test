using Microsoft.EntityFrameworkCore;
using PersonApi.Models;

namespace PersonApi.Data
{
    public class PersonAPIContext : DbContext
    {
        public PersonAPIContext(DbContextOptions<PersonAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonType> PersonTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed PersonTypes first (FK target)
            modelBuilder.Entity<PersonType>().HasData(
                new PersonType { Id = 1, Description = "Student" },
                new PersonType { Id = 2, Description = "Employee" },
                new PersonType { Id = 3, Description = "Visitor" }
            );

            // Seed Persons
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = 1,
                    Name = "Alice",
                    Age = 22,
                    PersonTypeId = 1
                },
                new Person
                {
                    Id = 2,
                    Name = "Bob",
                    Age = 30,
                    PersonTypeId = 2
                },
                new Person
                {
                    Id = 3,
                    Name = "Charlie",
                    Age = 40,
                    PersonTypeId = 3
                }
            );


            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Age)
                    .IsRequired();

                entity.Property(e => e.PersonTypeId)
                    .IsRequired();

                entity.HasOne(pt => pt.PersonType)
                    .WithMany(pt => pt.Persons)
                    .HasForeignKey(pt => pt.PersonTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PersonType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(30)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // 👈 Case-insensitive collation

                entity.HasIndex(e => e.Description)
                    .IsUnique();
            });
        }

    }
}
