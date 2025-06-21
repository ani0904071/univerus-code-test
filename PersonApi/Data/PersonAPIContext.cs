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
