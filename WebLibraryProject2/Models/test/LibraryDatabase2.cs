namespace WebLibraryProject2.Models.test
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LibraryDatabase2: DbContext
    {
        public LibraryDatabase2()
            : base("name=LibraryDatabase2")
        {
        }

        public virtual DbSet<Author> Authors
        {
            get; set;
        }
        public virtual DbSet<BookLocation> BookLocations
        {
            get; set;
        }
        public virtual DbSet<Course> Courses
        {
            get; set;
        }
        public virtual DbSet<Discipline> Disciplines
        {
            get; set;
        }
        public virtual DbSet<Publication> Publications
        {
            get; set;
        }
        public virtual DbSet<Reader> Readers
        {
            get; set;
        }
        public virtual DbSet<Stat> Stats
        {
            get; set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(e => e.Publications)
                .WithMany(e => e.Authors)
                .Map(m => m.ToTable("PublicationAuthor").MapLeftKey("Authors_Id").MapRightKey("Publications_Id"));

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Publications)
                .WithMany(e => e.Courses)
                .Map(m => m.ToTable("PublicationCourse"));

            modelBuilder.Entity<Discipline>()
                .HasMany(e => e.Publications)
                .WithMany(e => e.Disciplines)
                .Map(m => m.ToTable("DisciplinePublication"));

            modelBuilder.Entity<Publication>()
                .HasMany(e => e.BookLocations)
                .WithRequired(e => e.Publication)
                .HasForeignKey(e => e.Publication_Id);

            modelBuilder.Entity<Publication>()
                .HasMany(e => e.Stats)
                .WithRequired(e => e.Publication)
                .HasForeignKey(e => e.Publication_Id);

            modelBuilder.Entity<Reader>()
                .HasMany(e => e.BookLocations)
                .WithOptional(e => e.Reader)
                .HasForeignKey(e => e.Reader_Id);
        }
    }
}
