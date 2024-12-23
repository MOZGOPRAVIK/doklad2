using System;
using System.Collections.Generic;

namespace doklad_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Алгоритм Гэйла - Шепли для задачи стабильных бракосочетаний");
            Console.WriteLine("------------------------------------------------------");

            // Ввод количества мужчин и женщин
            Console.Write("Введите количество мужчин и женщин: ");
            int n = int.Parse(Console.ReadLine());

            // Инициализация списков мужчин и женщин
            List<Person> men = new List<Person>();
            List<Person> women = new List<Person>();

            // Ввод имен мужчин
            Console.WriteLine("\nВведите имена мужчин:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Мужчина {i + 1}: ");
                men.Add(new Person(Console.ReadLine()));
            }

            // Ввод имен женщин
            Console.WriteLine("\nВведите имена женщин:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Женщина {i + 1}: ");
                women.Add(new Person(Console.ReadLine()));
            }

            // Ввод предпочтений мужчин
            Console.WriteLine("\nВведите предпочтения мужчин (через запятую):");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{men[i].Name}: ");
                string[] preferences = Console.ReadLine().Split(',');
                foreach (var pref in preferences)
                {
                    men[i].Preferences.Add(women.Find(w => w.Name == pref.Trim()));
                }
            }

            // Ввод предпочтений женщин
            Console.WriteLine("\nВведите предпочтения женщин (через запятую):");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{women[i].Name}: ");
                string[] preferences = Console.ReadLine().Split(',');
                foreach (var pref in preferences)
                {
                    women[i].Preferences.Add(men.Find(m => m.Name == pref.Trim()));
                }
            }

            // Выполнение алгоритма Гэйла - Шепли
            GaleShapley.Match(men, women);

            // Вывод результатов
            Console.WriteLine("\nРезультаты стабильных бракосочетаний:");
            foreach (var man in men)
            {
                Console.WriteLine($"{man.Name} сочетается с {man.Partner.Name}");
            }
        }
    }

    // Класс для представления мужчин и женщин
    public class Person
    {
        public string Name { get; set; }
        public List<Person> Preferences { get; set; }
        public Person Partner { get; set; }

        public Person(string name)
        {
            Name = name;
            Preferences = new List<Person>();
            Partner = null;
        }
    }

    // Класс для реализации алгоритма Гэйла - Шепли
    public class GaleShapley
    {
        public static void Match(List<Person> men, List<Person> women)
        {
            var freeMen = new Queue<Person>(men);
            while (freeMen.Count > 0)
            {
                var man = freeMen.Dequeue();
                if (man.Preferences.Count == 0) continue;

                var woman = man.Preferences[0];
                man.Preferences.RemoveAt(0);

                if (woman.Partner == null)
                {
                    woman.Partner = man;
                    man.Partner = woman;
                }
                else if (woman.Preferences.IndexOf(man) < woman.Preferences.IndexOf(woman.Partner))
                {
                    woman.Partner.Partner = null;
                    freeMen.Enqueue(woman.Partner);
                    woman.Partner = man;
                    man.Partner = woman;
                }
                else
                {
                    freeMen.Enqueue(man);
                }
            }
        }
    }
}