using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoctailsGuideWebApplication
{
    public partial class DBCoctailsGuideContext : DbContext
    {
        public DBCoctailsGuideContext()
        {
        }

        public DBCoctailsGuideContext(DbContextOptions<DBCoctailsGuideContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Coctails> Coctails { get; set; }
        public virtual DbSet<Compounds> Compounds { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Glass> Glass { get; set; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<Strengths> Strengths { get; set; }
        public virtual DbSet<Techniques> Techniques { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-BQP6TDD8\\SQLEXPRESS;Database=DBCoctailsGuide;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Coctails>(entity =>
            {
                entity.Property(e => e.CountryofCreationId).HasColumnName("CountryofCreationID");

                entity.Property(e => e.GlassId).HasColumnName("GlassID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StrengthId).HasColumnName("StrengthID");

                entity.Property(e => e.TechniqueId).HasColumnName("TechniqueID");

                entity.Property(e => e.YearofCreation).HasMaxLength(50);

                entity.HasOne(d => d.CountryofCreation)
                    .WithMany(p => p.Coctails)
                    .HasForeignKey(d => d.CountryofCreationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coctails_Country");

                entity.HasOne(d => d.Glass)
                    .WithMany(p => p.Coctails)
                    .HasForeignKey(d => d.GlassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coctails_Glass");

                entity.HasOne(d => d.Strength)
                    .WithMany(p => p.Coctails)
                    .HasForeignKey(d => d.StrengthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coctails_Strengths");

                entity.HasOne(d => d.Technique)
                    .WithMany(p => p.Coctails)
                    .HasForeignKey(d => d.TechniqueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coctails_Techniques");
            });

            modelBuilder.Entity<Compounds>(entity =>
            {
                entity.Property(e => e.CoctailId).HasColumnName("CoctailID");

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.HasOne(d => d.Coctail)
                    .WithMany(p => p.Compounds)
                    .HasForeignKey(d => d.CoctailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Compounds_Coctails");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.Compounds)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Compounds_Ingredients");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Glass>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredients_Categories");
            });

            modelBuilder.Entity<Strengths>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Techniques>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
