using System;
using System.Collections;

namespace _06112020 // індексатор, структури
{
    class Numbers : IEnumerable, IEnumerator
    {
        //int a = 10, b = 20, c = 30;
        int a = 10;
        double b = 20.99;
        string c = "Hello";

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext() // викликатиме foreach при кожній ітерації
        {
            if (pos >= 3)
            {
                Reset();
                return false;
            }
            else
            {
                pos++;
                return true;
            }
        }

        public void Reset() //встановлює в початкові умови наш обєкт, що б можна було його повторно передати в колекцію
        {
            pos = 0;
        }

        private int pos = 0;

        public object Current
        {
            get
            {
                return pos switch
                {
                    1 => a,
                    2 => b,
                    3 => c,
                    _ => throw new ArgumentOutOfRangeException(), //default switch
                };
            }
        }
    }

    class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birthday { get; set; }
        public override string ToString()
        {
            return $"| {LastName,-15} | {FirstName,-15} | {Birthday.ToShortDateString(),-12}";
        }
    }

    class Group
    {
        private Student[] students =
        {
        new Student{ LastName = "Ivanov", FirstName = "Ivan", Birthday = new DateTime(2002, 10, 22)},
        new Student{ LastName = "Petrenko", FirstName = "Petro", Birthday = new DateTime(2001, 11, 15)},
        new Student{ LastName = "Stepanenko", FirstName = "Stepan", Birthday = new DateTime(1999, 09, 28)}
        };

        public int Length
        {
            get
            {
                return students.Length;
            }
        }

        public Student this[int i]
        {
            get
            {
                if (i >= 0 && i < Length)
                    return students[i];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (i >= 0 && i < Length)
                    students[i] = value;
                else throw new IndexOutOfRangeException();
            }
        }

        int FindByLastName(string LN)
        {
            for (int i = 0; i < Length; i++)
                if (String.Equals(students[i].LastName, LN, StringComparison.CurrentCultureIgnoreCase))
                    return i;
            return -1;
        }

        int FindByLastNameAndFirstName(string LN, string FN)
        {
            for (int i = 0; i < Length; i++)
                if (String.Equals(students[i].LastName, LN, StringComparison.CurrentCultureIgnoreCase))
                    if (String.Equals(students[i].FirstName, FN, StringComparison.CurrentCultureIgnoreCase))
                        return i;
            return -1;
        }

        public string this[string text]
        {
            get
            {
                int i = FindByLastName(text);
                if (i > 0)
                    return students[i].ToString();
                throw new IndexOutOfRangeException();
            }
        }

        public DateTime this[String LN, String FN] // багатомірний індексатор
        {
            get
            {
                int i = FindByLastNameAndFirstName(LN, FN);
                if (i >= 0)
                    return students[i].Birthday;
                throw new IndexOutOfRangeException();
            }
            set
            {
                int i = FindByLastNameAndFirstName(LN, FN);
                if (i >= 0)
                    students[i].Birthday = value;
                else throw new IndexOutOfRangeException();
            }
        }
    }

    class PC : IEquatable<PC>, ICloneable
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Name,-15} {Price,10}";
        }

        public bool Equals(PC other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Price.Equals(other.Price);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PC)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Price.GetHashCode();
            }
        }

        public object Clone()
        {
            return new PC { Name = this.Name, Price = this.Price };
        }
    }

    struct User : ICloneable
    {
        public static User CreateInstance(string LN, string FN)
        {
            return new User(LN, FN);
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birthday { get; set; }

        public PC Comp { get; set; }

        public User(string LN, string FN, DateTime BD, PC pc)
        {
            LastName = LN;
            FirstName = FN;
            Birthday = BD;
            Comp = (PC)pc?.Clone();
        }

        public User(string LN, string FN, DateTime BD) : this(LN, FN, BD, null)
        {
        }

        public User(string LN, string FN) : this(LN, FN, DateTime.Now)
        {
        }

        public User(DateTime BD) : this()
        {
            Birthday = BD;
        }

        public override string ToString()
        {
            return $"| {LastName,-15} | {FirstName,-15} | {Birthday.ToShortDateString(),-12} | {Comp} |";
        }

        public object Clone()
        {
            User temp = this;
            temp.Comp = (PC)this.Comp?.Clone();
            return temp;
        }
    }

    class Program
    {
        static void Test1()
        {
            Numbers ob = new Numbers();
            foreach (var item in ob)
            {
                Console.Write(item + "\t");
            }
            Console.WriteLine();
            foreach (var item in ob)
            {
                Console.Write(item + "\t");
            }
        }

        static void Test2()
        {
            Group PE911 = new Group();
            for (int i = 0; i < PE911.Length; i++)
                Console.WriteLine(PE911[i]);
            //PE911[1] = new Student { LastName = "Nuton", FirstName = "Inna" };
            //for (int i = 0; i < PE911.Length; i++)
            //    Console.WriteLine(PE911[i]);
            //Console.WriteLine(PE911[0]);
            //Console.WriteLine(PE911[1]);
            //Console.WriteLine(PE911[2]);
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(PE911["Petrenko"]);
            Console.WriteLine(PE911["Petrenko", "Petro"]);
            PE911["Petrenko", "Petro"] = new DateTime(1988, 05, 26);
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(PE911["Petrenko"]);
        }

        static void Test3()
        {
            User user1 = new User();
            Console.WriteLine(user1);

            User user2 = new User("Petrenko", "Petro", new DateTime(1996, 10, 23));
            Console.WriteLine(user2);

            User user3 = User.CreateInstance("Ivanenko", "Ivan");
            Console.WriteLine(user3);

            User user4 = new User(DateTime.Now);
            Console.WriteLine(user4);

            User user5 = new User("Pesyk", "Petro", new DateTime(1996, 10, 23), new PC { Name = "Lenovo", Price = 9999.99 });
            Console.WriteLine(user5);

            //User user6 = user5;
            User user6 = (User)user5.Clone(); // клон структури
            user6.LastName = "Harik";
            user6.Comp.Name = "Fujitsu";
            Console.WriteLine("----------------------------------");
            Console.WriteLine(user5);
            Console.WriteLine(user6);

        }

        static void Main(string[] args)
        {
            //Test1();
            //Test2();
            Test3();

            Console.WriteLine("\nThat's the End Folks :) ");
        }
    }
}