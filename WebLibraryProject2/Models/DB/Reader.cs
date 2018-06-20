using System.Linq;
using Generator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WebLibraryProject2.Models
{
    public partial class Reader
    {
        public eAccessLevel toEnumAL
        {
            get => (eAccessLevel) AccessLevel;
            set => AccessLevel = (byte) value;
        }

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
