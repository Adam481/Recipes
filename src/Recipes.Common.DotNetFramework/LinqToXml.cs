using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Recipes.Common.DotNetFramework
{
    internal class LinqToXml
    {
        //private static void QuerySqlServer()
        //{
        //    // MovieReviewsDataContext dc = new MovieReviewsDataContext();
        //    //IEnumerable<Movie> topMovies =
        //    //    from m in dc.Movies
        //    //    where m.ReleaseData.Year > 2006
        //    //    orderby m.Reviews.Average(r => r.Rating) descending
        //    //    select m;

        //    //foreach (int id in topMovies)
        //    //{
        //    //    Console.WriteLine(movie.Reviews);
        //    //}
        //}

        private static void QueryXml()
        {
            XDocument doc = new XDocument(
                new XElement("Processes",
                    from p in Process.GetProcesses()
                    orderby p.ProcessName ascending
                    select new XElement("Process",
                        new XAttribute("Name", p.ProcessName),
                        new XAttribute("PID", p.Id))));

            IEnumerable<int> pids = //from e in doc.Element("Processes").Elements("Process")
                from e in doc.Descendants("Process")
                where e.Attribute("Name").Value == "devenv"
                orderby (int)e.Attribute("PID") ascending
                select (int)e.Attribute("PID");

            foreach (int id in pids)
            {
                Console.WriteLine(id);
            }
        }


        private static void QueryEmployeesList()
        {
            Console.WriteLine("\nQuery employees method");

            List<Employee> employees = new List<Employee>()
            {
                new Employee { ID=1, Name="Scott", HireDate=new DateTime(2002, 3, 5)},
                new Employee { ID=2, Name="Poonam", HireDate=new DateTime(2002, 10, 15)},
                new Employee { ID=3, Name="Paul", HireDate=new DateTime(2007, 10, 11)}
            };

            IEnumerable<Employee> query =       // query will be executed in foreach loop
                from e in employees
                where e.HireDate.Year < 2005
                orderby e.Name
                select e;

            employees.Add(new Employee() { Name = "Linda", ID = 4, HireDate = new DateTime(2003, 6, 11) });

            foreach (Employee e in query)
            {
                Console.WriteLine(e.Name);
            }

        }


        private static void QueryEmployees()
        {
            Console.WriteLine("\nQuery employees method");

            IEnumerable<Employee> employees = new List<Employee>()
            {
                new Employee { ID=1, Name="Scott", HireDate=new DateTime(2002, 3, 5)},
                new Employee { ID=2, Name="Poonam", HireDate=new DateTime(2002, 10, 15)},
                new Employee { ID=3, Name="Paul", HireDate=new DateTime(2007, 10, 11)}

            };

            IEnumerable<Employee> query =
                from e in employees
                where e.HireDate.Year < 2005
                orderby e.Name
                select e;

            foreach (Employee e in query)
            {
                Console.WriteLine(e.Name);
            }
        }

        class Employee
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime HireDate { get; set; }
        }
    }
}
