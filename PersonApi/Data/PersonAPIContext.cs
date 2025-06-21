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
                new PersonType { Id = 1, PersonTypeDescription = "Student" },
                new PersonType { Id = 2, PersonTypeDescription = "Employee" },
                new PersonType { Id = 3, PersonTypeDescription = "Visitor" }
            );

            // Seed Persons
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = 1,
                    PersonName = "Alice",
                    PersonAge = 22,
                    PersonTypeId = 1
                },
                new Person
                {
                    Id = 2,
                    PersonName = "Bob",
                    PersonAge = 30,
                    PersonTypeId = 2
                },
                new Person
                {
                    Id = 3,
                    PersonName = "Charlie",
                    PersonAge = 40,
                    PersonTypeId = 3
                }
            );


            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PersonAge)
                    .IsRequired();

                entity.Property(e => e.PersonTypeId)
                    .IsRequired();

                entity.HasOne(e => e.PersonType)
                    .WithMany(pt => pt.Persons)
                    .HasForeignKey(e => e.PersonTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PersonType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PersonTypeDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }

    }
}
