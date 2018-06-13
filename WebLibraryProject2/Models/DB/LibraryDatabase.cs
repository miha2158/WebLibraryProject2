using System.Collections.Generic;

namespace WebLibraryProject2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LibraryDatabase: DbContext
    {
        public LibraryDatabase(): base("name=LDB2") { }

        public virtual DbSet<Author> Authors  { get; set; }
        public virtual DbSet<BookLocation> BookLocations { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Discipline> Disciplines { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Reader> Readers { get; set; }
        public virtual DbSet<Stats> Stats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                        .HasMany(e => e.Publications)
                        .WithMany(e => e.Authors)
                        .Map(m => m.ToTable("PublicationAuthor"));

            modelBuilder.Entity<Courses>()
                        .HasMany(e => e.Publications)
                        .WithMany(e => e.Courses)
                        .Map(m => m.ToTable("PublicationCourse").MapLeftKey("Course_Id").MapRightKey("Publication_Id"));

            modelBuilder.Entity<Publication>()
                        .HasMany(e => e.BookLocations)
                        .WithRequired(e => e.Publication)
                        .HasForeignKey(e => e.Publications);

            modelBuilder.Entity<Publication>()
                        .HasMany(e => e.Stats)
                        .WithRequired(e => e.Publication)
                        .HasForeignKey(e => e.Publications);

            modelBuilder.Entity<Publication>()
                        .HasMany(e => e.Disciplines)
                        .WithMany(e => e.Publications)
                        .Map(m => m.ToTable("DisciplinePublication").MapLeftKey("Publication_Id").MapRightKey("Discipline_Id"));

            modelBuilder.Entity<Reader>()
                        .HasMany(e => e.BookLocations)
                        .WithOptional(e => e.Reader)
                        .HasForeignKey(e => e.Readers);
        }
    }
}
