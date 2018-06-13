using System.Collections.Generic;
using System.IO;
using Generator;

namespace WebLibraryProject2.Models
{
    public class NewPerson: Person
    {
        public int ID { get; set; }
        public string Patronimic { get; set; } = string.Empty;

        public NewPerson() { }
        public NewPerson(string First, string Last, string Patronimic)
        {
            this.First = First;
            this.Last = Last;
            this.Patronimic = Patronimic;
        }

        public static string path = $@"C:\Users\Михаил\Downloads\Documents\namesnamesnames\";

        public new static List<string> MaleFirstNames => maleFirstNames ?? (maleFirstNames = new List<string>(File.ReadAllLines($@"{path}malefirstnames.txt")));
        public new static List<string> MaleLastNames => maleLastNames ?? (maleLastNames = new List<string>(File.ReadAllLines($@"{path}malelastnames.txt")));
        public new static List<string> FemaleFirstNames => femaleFirstNames ?? (femaleFirstNames = new List<string>(File.ReadAllLines($@"{path}femalefirstnames.txt")));
        public new static List<string> FemaleLastNames => femaleLastNames ?? (femaleLastNames = new List<string>(File.ReadAllLines($@"{path}femalelastnames.txt")));
        public static List<string> FemalePatronymics => femalePatronymics ?? (femalePatronymics = new List<string>(File.ReadAllLines($@"{path}femalepatronymics.txt")));
        public static List<string> MalePatronymics => malePatronymics ?? (malePatronymics = new List<string>(File.ReadAllLines($@"{path}malepatronymics.txt")));

        private static List<string> maleFirstNames;
        private static List<string> maleLastNames;
        private static List<string> femaleFirstNames;
        private static List<string> femaleLastNames;
        private static List<string> malePatronymics;
        private static List<string> femalePatronymics;

        public new static NewPerson FillBlanks(Gender gender)
        {
            var p = new NewPerson();
            switch (gender)
            {
                case Gender.Male:
                {
                    p.First = MaleFirstNames[NewValue.Int(MaleFirstNames.Count)];
                    p.Last = MaleLastNames[NewValue.Int(MaleLastNames.Count)];
                    p.Patronimic = MalePatronymics[NewValue.Int(MalePatronymics.Count)];
                    break;
                }

                case Gender.Female:
                {
                    p.First = FemaleFirstNames[NewValue.Int(FemaleFirstNames.Count)];
                    p.Last = FemaleLastNames[NewValue.Int(FemaleLastNames.Count)];
                    p.Patronimic = FemalePatronymics[NewValue.Int(FemalePatronymics.Count)];
                    break;
                }
            }
            return p;
        }
    }
}