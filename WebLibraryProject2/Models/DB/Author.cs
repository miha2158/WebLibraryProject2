using System.Linq;
using Generator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WebLibraryProject2.Models
{

    public partial class Author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Author()
        {
            Publications = new HashSet<Publication>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string First { get; set; }

        [Required]
        [StringLength(15)]
        public string Last { get; set; }

        [Required]
        [StringLength(15)]
        public string Patronimic { get; set; }

        public byte WriterType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Publication> Publications { get; set; }

        public eWriterType toEnumWT => (eWriterType)WriterType;

        public Author(string First, string Last, string Patronimic, eWriterType WriterType) : this()
        {
            this.First = First;
            this.Last = Last;
            this.Patronimic = Patronimic;
            this.WriterType = WriterType.e();
        }

        public static Author FillBlanks() => FillBlanks((Gender)NewValue.Int(2));
        public static Author FillBlanks(Gender gender)
        {
            var p = NewPerson.FillBlanks(gender);
            var b = new Author(p.First, p.Last, p.Patronimic, (eWriterType)NewValue.Int(2));

            using (var db = new LibraryDatabase())
                db.Authors.Add(b);

            return b;
        }

        public override string ToString() => $"{Last} {First[0]}.{Patronimic[0]}.";
        public override int GetHashCode() => ToString().GetHashCode();
        public override bool Equals(object obj)
        {
            var o = obj as Author;
            using (var db = new LibraryDatabase())
            {
                return db.Authors.Any(d => d.Id == o.Id &&
                                                d.First == o.First &&
                                                d.Last == o.Last &&
                                                d.Patronimic == o.Patronimic &&
                                                d.WriterType == o.WriterType);
            }
        }
    }
}