using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;

namespace WebLibraryProject2.Models
{
    public partial class Publication
    {
        private Publication(string Name, ePublicationType PublicationType, eBookPublication BookPublication, DateTime DatePublished, string Publisher)
        {
            this.Name = Name;
            this.PublicationType = PublicationType.e();
            this.BookPublication = BookPublication.e();
            this.DatePublished = DatePublished;
            this.Publisher = Publisher;
        }
        public Publication(string Name, Author Author, ePublicationType PublicationType, eBookPublication BookPublication, DateTime DatePublished, string Publisher) :
            this(Name, PublicationType, BookPublication, DatePublished, Publisher)
        {
            Authors = new[] { Author };
        }
        public Publication(string Name, IEnumerable<Author> Authors, ePublicationType PublicationType, eBookPublication BookPublication, DateTime DatePublished, string Publisher) :
            this(Name, PublicationType, BookPublication, DatePublished, Publisher)
        {
            this.Authors = Authors.ToArray();
        }

        public override bool Equals(object obj)
        {
            using (var db = new LibraryDatabase())
            {

                var publication = obj as Publication;

                return db.Publications.Any(e => e.Name == publication.Name &&
                                                     e.DatePublished == publication.DatePublished &&
                                                     e.Publisher == publication.Publisher &&
                                                     e.PublicationType == publication.PublicationType &&
                                                     e.BookPublication == publication.BookPublication);

            }
        }
        public override string ToString() => $"{Name}, {DatePublished}, {Publisher}";
        public override int GetHashCode() => ToString().GetHashCode();


        public static List<Publication> All
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return Enumerable.ToList<Publication>(db.Publications);
                }
            }
        }
        public static IEnumerable<string> AllPublishers => All.Select(e => e.Publisher).Distinct();
        public static List<Discipline> AllDisciplines
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Disciplines.Where(d => d != null).ToList();
                }
            }
        }

        public string DDate => DatePublished.ToNiceDate();
        public string DAuthors
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.Authors.Aggregate(string.Empty, (c, d) => c += $"{d}, ");
                }
            }
        }
        public int DCount
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.BookLocations.Count ?? 0;
                }
            }
        }
        public int DNowTakenCount
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.BookLocations.Count(e => e.IsTaken) ?? 0;
                }
            }
        }
        public int DTotalTakenCount
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.Stats.Count ?? 0;
                }
            }
        }
        public string DCourses
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.Courses.Aggregate(string.Empty, (c, d) => c += $"{d.Course}, ");
                }
            }
        }
        public string DDisciplines
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.Disciplines.Aggregate(string.Empty, (p, d) => p += $"{d.Name}, ");
                }
            }
        }
        public ePublicationType toEnumPT => (ePublicationType)PublicationType;
        public eBookPublication toEnumBP => (eBookPublication)BookPublication;


        public IEnumerable<Reader> Readers
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.BookLocations.Where(e => e.IsTaken).Select(e => e.Reader).Distinct();
                }
            }
        }
        public IEnumerable<BookLocation> Locations
        {
            get
            {
                using (var db = new LibraryDatabase())
                {
                    return db.Publications.Find(Id)?.BookLocations.Where(e => !e.IsTaken).Distinct();
                }
            }
        }


    }
}
