using System.Linq;
using Generator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WebLibraryProject2.Models
{
    [Table("Reader")]
    public partial class Reader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reader()
        {
            BookLocations = new HashSet<BookLocation>();
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

        public byte AccessLevel { get; set; }

        [Required]
        [StringLength(9)]
        public string Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookLocation> BookLocations { get; set; }



        public eAccessLevel toEnumAL => (eAccessLevel)AccessLevel;

        public Reader(string First, string Last, string Patronimic) : this()
        {
            this.First = First;
            this.Last = Last;
            this.Patronimic = Patronimic;
            this.AccessLevel = AccessLevel;
        }
        public Reader(string First, string Last, string Patronimic, string Group) : this(First, Last, Patronimic)
        {
            AccessLevel = eAccessLevel.Student.e();
            this.Group = Group;
        }

        private static readonly string[] GroupNames = new[] { "ю", "ч", "ъ", "ф", "╗", "ох" };
        public static Reader FillBlanks() => FillBlanks((Gender)NewValue.Int(2));
        public static Reader FillBlanks(Gender gender)
        {
            var p = NewPerson.FillBlanks(gender);
            var b = new Reader(p.First, p.Last, p.Patronimic, $"{GroupNames[NewValue.Int(GroupNames.Length)] }-{NewValue.Int(15, 18)}-{NewValue.Int(1, 3)}") { AccessLevel = (byte)NewValue.Int(2) };

            return b;
        }

        public override string ToString() => $"{Last} {First[0]}. {Patronimic[0]}.";
        public override int GetHashCode() => ToString().GetHashCode();
        public override bool Equals(object obj)
        {
            var o = obj as Reader;
            using (var db = new LibraryDatabase())
            {
                return db.Readers.Any(d => d.Id == o.Id &&
                                               d.First == o.First &&
                                               d.Last == o.Last &&
                                               d.Patronimic == o.Patronimic &&
                                               d.Group == o.Group);
            }
        }
    }
}
