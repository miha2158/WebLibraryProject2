using System;
using System.Collections.Generic;
using System.Linq;

namespace WebLibraryProject2.Models
{
    public enum eAccessLevel
    {
        Student,
        Teacher,
        Admin
    }

    public enum eWriterType
    {
        HseTeacher,
        Other
    }

    public enum eBookPublication
    {
        None,
        Book,
        Publication, 
    }

    public enum ePublicationType
    {
        None,
        Educational,
        Scientific,
    }

    public static partial class Ex
    {
        public static byte e(this eAccessLevel o) => (byte)o;
        public static byte e(this eWriterType o) => (byte)o;
        public static byte e(this eBookPublication o) => (byte)o;
        public static byte e(this ePublicationType o) => (byte)o;

        public static string ToString(this ICollection<Author> o) => o.Count.ToString();
        public static string ToString(this ICollection<Reader> o) => o.Count.ToString();
        public static string ToString(this ICollection<Publication> o) => o.Count.ToString();
        public static string ToString(this ICollection<Course> o) => o.Aggregate(string.Empty, (p, d) => p += $"{d.CourseNumber} ");
        public static string ToString(this ICollection<Stats> o) => o.Count.ToString();
        public static string ToString(this ICollection<Discipline> o) => o.Aggregate(string.Empty, (p, d) => p += $"{d.Name}, ");
        public static string ToString(this ICollection<BookLocation> o) => o.Count(d => !d.IsTaken).ToString();

        public static string ToNiceDate(this DateTime o) => $"{o.Year}-{o.Month}-{o.Day}";
    }
}