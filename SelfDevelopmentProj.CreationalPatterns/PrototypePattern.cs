using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.CreationalPatterns
{
    public class PrototypePattern
    {
        public static void Start()
        { 
            var p1 = new Person();
            p1.Name = "Sve Bozh";
            p1.Age = 36;
            p1.BirthDate = Convert.ToDateTime("1987-05-06");
            p1.IdInfo = new IdInfo(8787);

            var p2 = p1.ShallowCopy;

            var p3 = p1.DeepCopy();

            Console.WriteLine("Original values of p1, p2, p3:");
            Console.WriteLine("   p1 instance values: ");
            DisplayValues(p1);
            Console.WriteLine("   p2 instance values:");
            DisplayValues(p2);
            Console.WriteLine("   p3 instance values:");
            DisplayValues(p3);

            p1.Age = 32;
            p1.BirthDate = Convert.ToDateTime("1900-01-01");
            p1.Name = "Frank";
            p1.IdInfo.IdNumber = 7878;
            Console.WriteLine("\nValues of p1, p2 and p3 after changes to p1:");
            Console.WriteLine("   p1 instance values: ");
            DisplayValues(p1);
            Console.WriteLine("   p2 instance values (reference values have changed):");
            DisplayValues(p2);
            Console.WriteLine("   p3 instance values (everything was kept the same):");
            DisplayValues(p3);
        }

        private static void DisplayValues(Person p)
        {
            Console.WriteLine("      Name: {0:s}, Age: {1:d}, BirthDate: {2:MM/dd/yy}",
                p.Name, p.Age, p.BirthDate);
            Console.WriteLine("      ID#: {0:d}", p.IdInfo.IdNumber);
        }

        class Person
        {
            public int Age;
            public DateTime BirthDate;
            public string Name;
            public IdInfo IdInfo;

            public Person ShallowCopy
                => (Person)this.MemberwiseClone();

            public Person DeepCopy()
            { 
                Person clone = (Person)this.MemberwiseClone();
                clone.IdInfo = new IdInfo(this.IdInfo.IdNumber);
                clone.Name = this.Name;

                return clone;
            }
        }

        class IdInfo
        {
            public int IdNumber;

            public IdInfo(int idNumber)
            {
                this.IdNumber = idNumber;
            }
        }
    }
}
