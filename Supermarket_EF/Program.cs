using System;
using System.Linq;
using Supermarket_EF.Supermarket;

namespace Supermarket_EF
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }
        
        public static void Test()
        {
            // Додавання
            using (ApplicationContext db = new ApplicationContext())
            {
                Employee employee1 = new Employee { Name = "Andriy", Soname = "Makarevych", IsVaccinated = false };
                Employee employee2 = new Employee { Name = "Alexandr", Soname = "Brooks", IsVaccinated = false };

                db.Employees.Add(employee1);
                db.Employees.Add(employee2);
                db.SaveChanges();
            }

            // Отримання
            using (ApplicationContext db = new ApplicationContext())
            {
                var employees = db.Employees.ToList();
                Console.WriteLine("Данi пiсля додавання:");
                foreach (Employee emp in employees)
                {
                    Console.WriteLine($"{emp.Id}. {emp.Name} {emp.Soname} (Вакцинований: {emp.IsVaccinated})");
                }
            }

            // Редагування
            using (ApplicationContext db = new ApplicationContext())
            {
                Employee e = db.Employees.Find(2);
                if (e != null)
                {
                    e.IsVaccinated = true;
                    db.Employees.Update(e);
                    db.SaveChanges();
                }
                Console.WriteLine("\nДанi пiсля редагування:");
                var employees = db.Employees.ToList();
                foreach (Employee emp in employees)
                {
                    Console.WriteLine($"{emp.Id}. {emp.Name} {emp.Soname} (Вакцинований: {emp.IsVaccinated})");
                }
            }

            // Видаляємо
            using (ApplicationContext db = new ApplicationContext())
            {
                Employee e = db.Employees.Find(1);
                if (e != null)
                {
                    db.Employees.Remove(e);
                    db.SaveChanges();
                }
                Console.WriteLine("\nДанi пiсля видалення:");
                var employees = db.Employees.ToList();
                foreach (Employee emp in employees)
                {
                    Console.WriteLine($"{emp.Id}. {emp.Name} {emp.Soname} (Вакцинований: {emp.IsVaccinated})");
                }
            }
            Console.Read();
        }
    }
}
