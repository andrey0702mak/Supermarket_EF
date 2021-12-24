using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Supermarket_EF.Supermarket;
using System.Threading;

namespace Supermarket_EF
{
    class Program
    {
        static int ITERATIONS = 500;
        static CountdownEvent done = new CountdownEvent(ITERATIONS);
        static DateTime startTime = DateTime.Now;
        static TimeSpan totalLatency = TimeSpan.FromSeconds(0);
        static int x = 0;
        static object locker = new object();
        static TimeSpan summ = TimeSpan.FromSeconds(0);
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            Console.WriteLine("\n Task Test");
            TestTask();
         //   TestThread();
            Thread.Sleep(2000);
            TimeSpan taskSumm = summ;

            Console.WriteLine("\n Thread Test");
            TestThread();
        //    TestTask();
            Thread.Sleep(2000);
            Console.WriteLine($"\n Сумарний час виконання TestThread Locker: {summ}");
            Console.WriteLine($"\n Сумарний час виконання TestTask AddValues: {taskSumm}");
        }
        public static void TestTask()
        {
            AddValues();
            Thread.Sleep(1000);
        }
        public static async void AddValues()
        {
            await Task.Run(() => Locker());
        }
        public static void TestThread()
        {

            Thread Generate = new Thread(Locker);

            summ = TimeSpan.Zero;
            Console.WriteLine($"Обнуляем {summ}");
            Generate.Start();

            Thread.Sleep(2000);

            Console.WriteLine("\n");
            Thread readEmployees = new Thread(ReadEmployees);
            readEmployees.Start();

            Thread.Sleep(500);
            Console.WriteLine("\n");
            Thread readCategory = new Thread(ReadCategory);
            readCategory.Start();

        }
        public static async void ReadCategory()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Category> category = await db.Categories.ToListAsync();
                Console.WriteLine("Category");
                foreach (Category c in category)
                {
                    Console.WriteLine($"{c.Id} {c.Name} {c.Name}");
                }
            }
        }

        public static async void ReadEmployees()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Employee> employees = await db.Employees.ToListAsync();

                Console.WriteLine("Employees");
                foreach (Employee emp in employees)
                {
                    Console.WriteLine($"{emp.Id}. {emp.Name} {emp.Soname}");
                }
            }
        }
        public static void Locker()
        {
            lock (locker)
            {
                Thread.Sleep(100);
                x = 1;
                for (int i = 1; i < 9; i++)
                {
                    var queueTime = DateTime.Now;
                    ThreadPool.QueueUserWorkItem((o) => {
                        OnTaskStart(i, queueTime);
                        Thread.Sleep(100);
                        OnTaskEnd(i, queueTime);
                    });
                    x++;
                    Thread.Sleep(200);
                }
            }
        }
        static void Log(int id, DateTime queueTime, string action)
        {
            var now = DateTime.Now;
            var timestamp = now - startTime;
            var latency = now - queueTime;

            summ += latency;
            Console.WriteLine(latency);
        }
        static void OnTaskStart(int id, DateTime queueTime)
        {
            var latency = DateTime.Now - queueTime;
            lock (done) totalLatency += latency;
            TestValuesGenerator(x);
        }
        static void OnTaskEnd(int id, DateTime queueTime)
        {
            Log(id, queueTime, "Finished");
            done.Signal();
        }
        public static void TestValuesGenerator(int x)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string nameEmployee = "Iмя" + x;
                string surname = "Прiзвище" + x;
                Employee employee = new Employee { Name = nameEmployee, Soname = surname };

                string nameCategory = "Назва" + x;
                string title = "Опис" + x;
                Category category = new Category { Name = nameCategory, Title = title };

                db.AddAsync(employee);
                db.AddAsync(category);

                db.SaveChanges();
            }
        }
        public static void TestValuesAdd()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine("Adding location1...");
                Location location1 = new Location
                {
                    City = "Kyiv",
                    Country = "Kyiv oblast",
                    BranchName = "AA001",
                    Address = "Politehnic 1b"
                };
                db.Locations.Add(location1);

                Console.WriteLine("Adding Department products_department...");
                Console.WriteLine("Adding Department chancellery...");
                Console.WriteLine("Adding Department alco_drinks...");
                Console.WriteLine("Adding Department sport...");
                Department products_department = new Department { Name = "Products", Location = location1 };
                Department chancellery = new Department { Name = "Chancellery", Location = location1 };
                Department alco_drinks = new Department { Name = "Alcohol drinks", Location = location1 };
                Department sport = new Department { Name = "Sport", Location = location1 };
                db.Departments.AddRange(products_department, chancellery, alco_drinks, sport);

                Console.WriteLine("Adding Category smartphones...");
                Console.WriteLine("Adding Category laptop...");
                Console.WriteLine("Adding Category pc...");
                Category smartphones = new Category { Name = "Smarphones", Title = "Mobile phones" };
                Category laptop = new Category { Name = "Laptop", Title = "Good thing" };
                Category pc = new Category { Name = "PC", Title = "Everyone must have" };
                db.Categories.AddRange(smartphones, laptop, pc);

                Console.WriteLine("Adding Product product_macbook_pro_13...");
                Product product_macbook_pro_13 = new Product
                {
                    Name = "MacBook Pro 13",
                    Price = 35000,
                    Brand = "Apple",
                    Category = laptop,
                    OtherDetails = "Apple is a reliable, reliable laptop at a reasonable price",
                    SKU = "ABCD-1634"
                };

                Console.WriteLine("Adding Product product_macbook_pro_15_63...");
                Product product_macbook_pro_15_6 = new Product
                {
                    Name = "MacBook Pro 15",
                    Price = 53000,
                    Brand = "Apple",
                    Category = laptop,
                    OtherDetails = "Apple is a reliable, reliable laptop at a reasonable price",
                    SKU = "ABCD-1674"
                };

                Console.WriteLine("Adding Product product_iphone11...");
                Product product_iphone11 = new Product
                {
                    Name = "iPhone 11 128 GB",
                    Price = 19499,
                    Brand = "Apple",
                    Category = smartphones,
                    OtherDetails = "Apple is a reliable, reliable smartphones at a reasonable price",
                    SKU = "ABCD-1334"
                };

                Console.WriteLine("Adding Product product_iphone12...");
                Product product_iphone12 = new Product
                {
                    Name = "iPhone 12 128 GB",
                    Price = 24499,
                    Brand = "Apple",
                    Category = smartphones,
                    OtherDetails = "Apple is a reliable, reliable smartphones at a reasonable price",
                    SKU = "ABCD-1664"
                };
                Console.WriteLine("Adding Product product_iphone11_64...");
                Product product_iphone11_64 = new Product
                {
                    Name = "iPhone 11 64 GB",
                    Price = 17499,
                    Brand = "Apple",
                    Category = smartphones,
                    OtherDetails = "Apple is a reliable, reliable smartphones at a reasonable price",
                    SKU = "ABCD-1364"
                };

                Console.WriteLine("Adding Product product_samsung_a71...");
                Product product_samsung_a71 = new Product
                {
                    Name = "Galaxy A71 128 GB",
                    Price = 7499,
                    Brand = "SAMSUNG",
                    Category = smartphones,
                    OtherDetails = "SAMSUNG - the best",
                    SKU = "ABDE-1364"
                };
                Console.WriteLine("Adding Product product_samsung_a50...");
                Product product_samsung_a50 = new Product
                {
                    Name = "Galaxy A50 64 GB",
                    Price = 4500,
                    Brand = "SAMSUNG",
                    Category = smartphones,
                    OtherDetails = "SAMSUNG - the best",
                    SKU = "ABDE-1464"
                };
                db.Products.AddRange(product_iphone11, product_iphone11_64, product_iphone12, product_macbook_pro_13, 
                    product_macbook_pro_15_6, product_samsung_a50, product_samsung_a71);

                Console.WriteLine("Adding SpecificProduct specificProduct1...");
                Console.WriteLine("Adding SpecificProduct specificProduct2...");
                Console.WriteLine("Adding SpecificProduct specificProduct3...");
                Console.WriteLine("Adding SpecificProduct specificProduct4...");
                SpecificProduct specifi_product_iphone11 = new SpecificProduct { Product = product_iphone11, Quantity = 10 };
                SpecificProduct specific_product_iphone11_64 = new SpecificProduct { Product = product_iphone11_64, Quantity = 10 };
                SpecificProduct specific_product_iphone12 = new SpecificProduct { Product = product_iphone12, Quantity = 10 };
                SpecificProduct specific_product_macbook_pro_13 = new SpecificProduct { Product = product_macbook_pro_13, Quantity = 10 };
                db.SpacificProducts.AddRange(specifi_product_iphone11, specific_product_iphone11_64, specific_product_iphone12, specific_product_macbook_pro_13);

                Console.WriteLine("Adding Payment payment1...");
                Payment payment1 = new Payment() { Brand = "Visa", Type = "Card" };

                Console.WriteLine("Adding Employee employeeInokentiy...");
                Employee employeeInokentiy = new Employee()
                {
                    Name = "Inokentiy",
                    Soname = "Best"
                };
                db.Employees.Add(employeeInokentiy);

                Console.WriteLine("Adding Customer customer...");
                Customer customer = new Customer()
                {
                    Name = "Andriy",
                    Soname = "Makarevych"
                };
                db.Customers.Add(customer);

                Console.WriteLine("\n\n\nAdding Ticket ticket1...");
                Ticket ticket1 = new Ticket() { Customer = customer, Employee = employeeInokentiy, Location = location1, Payment = payment1, Date = DateTime.Parse("2021/11/23") };
                db.Tickets.Add(ticket1);

                Console.WriteLine("\nAdding Sales SpecificProduct = specifi_product_iphone11, Quantity = 10 Date 2021/11/23...");
                Sales sales1 = new Sales() { Ticket = ticket1, SpecificProduct = specifi_product_iphone11, Quantity = 10 };
                db.Sales.Add(sales1);

                Console.WriteLine("Adding Ticket ticket2...");
                Ticket ticket2 = new Ticket() { Customer = customer, Employee = employeeInokentiy, Location = location1, Payment = payment1, Date = DateTime.Parse("2021/11/22") };
                db.Tickets.Add(ticket2);

                Console.WriteLine("\nAdding Sales SpecificProduct = specific_product_iphone11_64, Quantity = 20 Date 2021/11/22...");
                Sales sales2 = new Sales() { Ticket = ticket2, SpecificProduct = specific_product_iphone11_64, Quantity = 40 };
                db.Sales.Add(sales2);

                Console.WriteLine("Adding Ticket ticket3...");
                Ticket ticket3 = new Ticket() { Customer = customer, Employee = employeeInokentiy, Location = location1, Payment = payment1, Date = DateTime.Parse("2021/11/22") };
                db.Tickets.Add(ticket3);

                Console.WriteLine("\nAdding Sales SpecificProduct = specific_product_iphone12, Quantity = 30 Date 2021/11/22...");
                Sales sales3 = new Sales() { Ticket = ticket3, SpecificProduct = specific_product_iphone12, Quantity = 30 };
                db.Sales.Add(sales3);

                Console.WriteLine("Adding Ticket ticket4...");
                Ticket ticket4 = new Ticket() { Customer = customer, Employee = employeeInokentiy, Location = location1, Payment = payment1, Date = DateTime.Parse("2021/11/22") };
                db.Tickets.Add(ticket4);

                Console.WriteLine("\nAdding Sales SpecificProduct = specific_product_iphone11_64, Quantity = 10 Date 2021/11/22...");
                Sales sales4 = new Sales() { Ticket = ticket4, SpecificProduct = specific_product_iphone11_64, Quantity = 10 };
                db.Sales.Add(sales4);

                db.SaveChanges();
            }
        }
        // Написати товар якій продавався найкраще за кожен день
        public static void MVPBeetwenDates(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                MVPOfTheDay(day);
        }
        public static void MVPOfTheDay(DateTime date)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var query = (from sp in db.SpacificProducts.Include(s => s.Product)
                             let totalQuantity = (from s in db.Sales
                                                  join t in db.Tickets on s.Ticket_Id equals t.Id
                                                  where s.SpecificProduct_Id == sp.Id && t.Date == date
                                                  select s.Quantity).Sum()
                             where totalQuantity > 0
                             orderby totalQuantity descending
                             select sp);

                var max = query.Max(p => p.Quantity);

                var query2 = query.Where(p => p.Quantity == max);

                foreach (var q in query2)
                {
                    Console.WriteLine($"Найбiльш продаваємим товаром {date.ToShortDateString()} є {q.Product.Name}");
                }

            }
        }
        public static void TestMethods()
        {
            Console.WriteLine("\n\nStarting public static void TestMethods() \n");
            using (ApplicationContext db = new ApplicationContext())
            {
                var products = (from product in db.Products.Include(p => p.Category)
                                where product.Category_Id == 1
                                select product).ToList();

                foreach (var product in products)
                    Console.WriteLine($"[{product.Category.Name}] {product.Brand} {product.Name} \t {product.Price}");
            }

            Console.WriteLine("\n Products.Where Apple");
            using (ApplicationContext db = new ApplicationContext())
            {
                var apple = db.Products.Where(p => p.Brand == "Apple");
  
                foreach (var a in apple)
                {
                    Console.WriteLine($"{a.Brand} {a.Name} \t {a.Price}");
                }
            }

            Console.WriteLine("\n Like SUMS");
            using (ApplicationContext db = new ApplicationContext())
            {
                var samsung = db.Products.Where(s => EF.Functions.Like(s.Brand, "%AMS%"));

                foreach (var s in samsung)
                {
                    Console.WriteLine($"{s.Brand} {s.Name} \t {s.Price}");
                }
            }
            using (ApplicationContext db = new ApplicationContext())
            {
                var a = db.Products.Find(1);
                Console.WriteLine("\n Products.Find(1)");
                Console.WriteLine($"{a.Brand} {a.Name} \t {a.Price}");
            };

            Console.WriteLine("\n\tAgregate functions");
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Count Apple Brand {db.Products.Count(p => p.Brand == "Apple")}");
                Console.WriteLine($"Sum price {db.Products.Sum(p => p.Price)}");
                Console.WriteLine($"Avg price {db.Products.Average(p => p.Price)}");
                Console.WriteLine($"Min price {db.Products.Min(p => p.Price)}");
                Console.WriteLine($"Max price {db.Products.Max(p => p.Price)}");
                var maxPrice = (from product in db.Products select product).OrderByDescending(item => item.Price).First();
                Console.WriteLine($"Max Price by order by: {maxPrice.Brand} {maxPrice.Name} \t {maxPrice.Price}");
            }

            Console.WriteLine("\n\tAny");
            using (ApplicationContext db = new ApplicationContext())
            {
                bool result = db.Products.Any(p => p.Brand == "Apple");
                if (result)
                    Console.WriteLine("We have Apple products");
                else
                    Console.WriteLine("We don`t have Apple products");
            };

            Console.WriteLine("\n\tAll");
            using (ApplicationContext db = new ApplicationContext())
            {
                bool result = db.Products.All(p => p.Price > 5000);
                if (result)
                    Console.WriteLine("All Products have price bigger than 5000");
                else
                    Console.WriteLine("NOT All Products have price bigger than 5000");
            };

            Console.WriteLine("\n\tGroupBy Category");
            using (ApplicationContext db = new ApplicationContext())
            {
                var groups = db.Products.GroupBy(u => u.Category.Name).Select(g => new
                {
                    g.Key,
                    Count = g.Count()
                });
                foreach (var group in groups)
                {
                    Console.WriteLine($"{group.Key} - {group.Count}");
                }
            };

            Console.WriteLine("\n\tUnion");
            using (ApplicationContext db = new ApplicationContext())
            {
                var products = db.Products.Where(u => u.Price < 23000)
                    .Union(db.Products.Where(u => u.Name.Contains("iPhone 11")));
                foreach (var p in products)
                    Console.WriteLine(p.Name);
            }

            Console.WriteLine("\n\tIntersect");
            using (ApplicationContext db = new ApplicationContext())
            {
                var products = db.Products.Where(u => u.Price > 19000)
                    .Intersect(db.Products.Where(u => u.Name.Contains("iPhone 11")));
                foreach (var p in products)
                    Console.WriteLine(p.Name);
            }

            Console.WriteLine("\n\tExcept");
            using (ApplicationContext db = new ApplicationContext())
            {
                var selector1 = db.Products.Where(u => u.Price > 19000);
                var selector2 = db.Products.Where(u => u.Name.Contains("iPhone 11"));
                var iphones = selector1.Except(selector2);

                foreach (var p in iphones)
                    Console.WriteLine(p.Name);
                
            }

            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    Microsoft.Data.SqlClient.SqlParameter param = new Microsoft.Data.SqlClient.SqlParameter("@name", "Smarphones");
            //    var products = db.Products.FromSqlRaw("GetProductsByCategory @name", param).ToList();
            //    foreach (var p in products)
            //        Console.WriteLine($"{p.Name} - {p.Price}");
            //}


            Console.WriteLine("\n\tProcedure where we get product with max price");
            using (ApplicationContext db = new ApplicationContext())
            {
                var param = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@userName",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 50
                };
                db.Database.ExecuteSqlRaw("GetProductWithMaxPrice @userName OUT", param);
                Console.WriteLine(param.Value);
            }

        }
        public static void Test()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Employee employee1 = new Employee { Name = "Andriy", Soname = "Makarevych", IsVaccinated = false };
                Employee employee2 = new Employee { Name = "Alexandr", Soname = "Brooks", IsVaccinated = false };

                db.Employees.Add(employee1);
                db.Employees.Add(employee2);
                db.SaveChanges();
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                var employees = db.Employees.ToList();
                Console.WriteLine("Данi пiсля додавання:");
                foreach (Employee emp in employees)
                {
                    Console.WriteLine($"{emp.Id}. {emp.Name} {emp.Soname} (Вакцинований: {emp.IsVaccinated})");
                }
            }

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

            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    Employee e = db.Employees.Find(1);
            //    if (e != null)
            //    {
            //        db.Employees.Remove(e);
            //        db.SaveChanges();
            //    }
            //    Console.WriteLine("\nДанi пiсля видалення:");
            //    var employees = db.Employees.ToList();
            //    foreach (Employee emp in employees)
            //    {
            //        Console.WriteLine($"{emp.Id}. {emp.Name} {emp.Soname} (Вакцинований: {emp.IsVaccinated})");
            //    }
            //}
            Console.Read();
        }
    }
}
