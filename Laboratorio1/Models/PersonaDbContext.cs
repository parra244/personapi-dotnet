using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models
{
    public class PersonaDbContext : DbContext
    {
        public PersonaDbContext(DbContextOptions<PersonaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persona>   Personas   { get; set; }
        public DbSet<Profesion> Profesiones { get; set; }
        public DbSet<Estudio>   Estudios   { get; set; }
        public DbSet<Telefono>  Telefonos  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite primary key for Estudio
            modelBuilder.Entity<Estudio>()
                .HasKey(e => new { e.IdProf, e.CcPer });

            // Estudio -> Profesion relationship
            modelBuilder.Entity<Estudio>()
                .HasOne(e => e.Profesion)
                .WithMany(p => p.Estudios)
                .HasForeignKey(e => e.IdProf)
                .OnDelete(DeleteBehavior.Restrict);

            // Estudio -> Persona relationship
            modelBuilder.Entity<Estudio>()
                .HasOne(e => e.Persona)
                .WithMany(p => p.Estudios)
                .HasForeignKey(e => e.CcPer)
                .OnDelete(DeleteBehavior.Restrict);

            // Telefono -> Persona relationship
            modelBuilder.Entity<Telefono>()
                .HasOne(t => t.Persona)
                .WithMany(p => p.Telefonos)
                .HasForeignKey(t => t.Duenio)
                .OnDelete(DeleteBehavior.Restrict);

            // Genero check constraint
            modelBuilder.Entity<Persona>()
                .HasCheckConstraint("CK_persona_genero", "genero IN ('M', 'F')");

            base.OnModelCreating(modelBuilder);
        }
    }
}
