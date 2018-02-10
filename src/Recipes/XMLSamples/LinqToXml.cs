using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Recipes.Common.DotNetFramework
{
    /*  XML short characterization
        * it has tree structure (one root element + parent-child relationship)  (xml can contain only one root)
        * it is build from the tags (no predefined tags | each element must habe closing tag -case sensitive | self-clossing tag are allowed)
        * first line the XML version and character encoding
        * all elements can have text content and attributes
        * comments like in html
        * white spaces are truncated into one single white-space        
        * avoid using '-' as a delimeter
        * xml schema - define the structure of an XML using xml schema language (XML Schema Definition - XSD)
        
        LINQ to XML   (System.Xml.Linq

        XNode - base clase for all X... classes. IT provide us with common api for any part of the XML document.
        XDocument
        XDeclaration
        XElemenet
        XName
        XAttribute
        XComment


    */


    public class LinqToXML
    {
        private static readonly string fileFolder = @"C:\Users\Adam Maliszewski\Desktop\C#_leraning\Learning\Learning\Notes\Files\";

        private static string FilePath(string fileName)
        {
            return Path.Combine(fileFolder, fileName);
        }


        public static void Run()
        {
            //CreatePeopleXML();
            //QueryPeopleXML();
            //CreateStudentXML();
            //QueryStudentXML();
            //AddStudentElementAtTheEnd();
            //AddStudentElementAtTheBegining();
            //AddStudentElementInSpecificLocation();
            //ModifyStudentElement();
            //RemoveStudentElement();
            //TransformStudentXMLToCSV();
            //TransformStudentToHtmlTable();
            //ValidatingXMLAgainstXSD();
        }


        public static void CreatePeopleXML()
        {
            // All string are automatically converted into XNames. XElement can have multiple xml arguments. Right structure is constructed automatically.

            var xmlDocument = new XDocument();
            var xmlPeople = new XElement("People",

                PersonRepository.GetAll()
                    .Select(x => new XElement("Person",
                                        new XAttribute("Id", x.Id),
                                        new XElement("Name", x.Name),
                                        new XElement("Surname", x.Surname)
                                        ))
            );

            xmlDocument.Add(xmlPeople);
            xmlDocument.Save(FilePath("people.xml"));
        }


        public static void QueryPeopleXML()
        {
            // if you have large xml file it is recommended to use old XMLReader caouse it is able to stream through all file. 
            // XMLDocument just loads the entire file into the memory

            var document = XDocument.Load(FilePath("people.xml"));

            // get the first element with particular name | get a collection of child element | or directly get all Person element document.Descendants("Person")

            var s = document.Element("People")
                            .Elements()
                            .Where(x => (int)x.Attribute("Id") > 5)
                            .Select(
                                x => x.Attribute("Id").Value + " " + string.Concat(x.Elements()
                                                                                    .Select(e => e.Value + " ")));

            s.ToList().ForEach(x => Console.WriteLine(x));
        }


        public static void CreateStudentXML()
        {
            var xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Creating an XML Tree using LINQ to XML"),
                new XElement("Students",
                    new XElement("Student", new XAttribute("Id", 1),
                        new XElement("Name", "Mark"),
                        new XElement("Gender", "Male"),
                        new XElement("TotalMarks", "800")),
                    new XElement("Student", new XAttribute("Id", 2),
                        new XElement("Name", "Rosy"),
                        new XElement("Gender", "Female"),
                        new XElement("TotalMarks", "900")),
                    new XElement("Student", new XAttribute("Id", 3),
                        new XElement("Name", "Pam"),
                        new XElement("Gender", "Female"),
                        new XElement("TotalMarks", "850")),
                    new XElement("Student", new XAttribute("Id", 4),
                        new XElement("Name", "John"),
                        new XElement("Gender", "Male"),
                        new XElement("TotalMarks", "950"))
                        ));

            xmlDocument.Save(FilePath("students.xml"));

        }


        public static void QueryStudentXML()
        {
            IEnumerable<string> names = XDocument.Load(FilePath("students.xml"))
                                                 .Descendants("Student")
                                                 .Where(x => (int)x.Element("TotalMarks") > 800)
                                                 .OrderByDescending(x => (int)x.Element("TotalMarks"))
                                                 .ThenBy(x => x.Element("Name"))
                                                 .Select(x => x.Element("Name").Value);

            names.ToList().ForEach(x => Console.WriteLine(x));
        }


        private static void AddStudentElementAtTheBegining()
        {
            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            xmlDocument.Root.AddFirst(
                    new XElement("Student", new XAttribute("Id", 99),
                        new XElement("Name", "John"),
                        new XElement("Gender", "Male"),
                        new XElement("TotalMarks", "940")));

            xmlDocument.Save(FilePath("students.xml"));

        }

        private static void AddStudentElementAtTheEnd()
        {
            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            xmlDocument.Root.Add(
                    new XElement("Student", new XAttribute("Id", 105),
                        new XElement("Name", "Todd"),
                        new XElement("Gender", "Male"),
                        new XElement("TotalMarks", "980")));

            xmlDocument.Save("students.xml");
        }


        private static void AddStudentElementInSpecificLocation()
        {
            // AddAfterSelf | AddBeforeSelf
            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            xmlDocument.Element("Students")
                       .Elements()
                       .Where(x => (int)x.Attribute("Id") == 1).FirstOrDefault()
                       .AddAfterSelf(
                          new XElement("Student", new XAttribute("Id", 105),
                              new XElement("Name", "Alan"),
                              new XElement("Gender", "Male"),
                              new XElement("TotalMarks", "840"))
                       );

            xmlDocument.Save(FilePath("students.xml"));  // , SaveOptions.DisableFormatting    - if you want to get rid of new line (ex. if you send file to remoute host)
        }


        private static void ModifyStudentElement()
        {
            // SetElementValue | SetAttributeValue
            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            xmlDocument.Element("Students")
                       .Elements()
                       .Where(x => (int)x.Attribute("Id") == 105).FirstOrDefault()
                       .SetElementValue("TotalMarks", 999);

            xmlDocument.Save(FilePath("students.xml"));

            //OR
            /* 
            xmlDocument.Element("Students")
                       .Elements()
                       .Where(x => (int)x.Attribute("Id") == 105)
                       .Select(x => x.Element("TotalMarks")).FirstOrDefault().SetValue(999);
            */

            //changing comment value
            /* 
            xmlDocument.Nodes().OfType<XComment>().FirstOrDefault().Value = "Comment Updated";
            xmlDocument.Save(FilePath("students.xml"));
            */
        }


        private static void RemoveStudentElement()
        {
            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            xmlDocument.Root.Elements().Where(x => x.Attribute("Id").Value == "99").Remove();

            xmlDocument.Save(FilePath("students.xml"));


            // Deleting all elements that are present under root node
            //xmlDocument.Root.Elements().Remove();

            // Deleting all comments from the xml document
            //xmlDocument.Nodes().OfType<XComment>().Remove();
        }


        private static void TransformStudentXMLToCSV()
        {
            var sb = new StringBuilder();
            string delimiter = ",";

            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            xmlDocument.Descendants("Student").ToList().ForEach(
                element => sb.Append(element.Attribute("Id").Value + delimiter +
                                     string.Concat(element.Elements().Select(x => x.Value + delimiter)) +
                                     Environment.NewLine));

            var sw = new StreamWriter(FilePath("studentsCSV.csv"));
            sw.WriteLine(sb.ToString());
            sw.Close();
        }


        private static void TransformStudentToHtmlTable()
        {
            var xmlDocument = XDocument.Load(FilePath("students.xml"));

            XDocument result = new XDocument(
                new XElement("table", new XAttribute("border", 1),
                    new XElement("thead",
                        new XElement("tr",
                            xmlDocument.Descendants("Student").FirstOrDefault()
                                .Elements()
                                .Select(x => new XElement("th", x.Name)
                            )),
                        new XElement("tbody",
                             xmlDocument.Descendants("Student")
                                .Select(x => new XElement("tr",
                                    x.Elements()
                                     .Select(e => new XElement("td", e.Value)))
                            ))
                 )));

            result.Save(FilePath("studentsHTML.html"));
        }


        private static void ValidatingXMLAgainstXSD()
        {
            /*
                An XSD (XML Schema Definition Language) file defines the structure of the XML file, 
                i.e which elements in which order, how many times, with attributes, how they are nested,
                etd. Without an XSD, an XML file is relatively free set of elements and attributes.

            ex:
            
                <xsd:element name="Students">
                    <xsd:complexType>
                        <xsd:sequence>
                            <xsd:element name="Student" minOccours="1" maxOccurs="4">
                                <xsd:complexType>
                                    <xsd:sequence>
                                        <xsd:element name="Name" minOccours="1" maxOccurs="1" />
                                        <xsd:element name="Gender" minOccours="1" maxOccurs="1" />
                                        <xsd:element name="TotalMarks" minOccours="1" maxOccurs="1" />
                                    </xsd:sequence>
                                </xsd:complexType>
                            </xsd:element> 
                        </xsd:sequence>
                    </xsd:complexType>
                </xsd:element>

            */

            var xmlSchema = new XmlSchemaSet();

            xmlSchema.Add("", FilePath("Student.xsd"));

            var doc = XDocument.Load(FilePath("students.xml"));

            bool valdationErrors = false;

            doc.Validate(xmlSchema, (s, e) =>
            {
                Console.WriteLine(e.Message);
                valdationErrors = true;
            });

            if (valdationErrors)
            {
                Console.WriteLine("validation failed");
            }
            else
            {
                Console.WriteLine("validation succedded");
            }



        }
    }

    public class PersonRepository
    {
        public static List<Person> GetAll()
        {
            return new List<Person>
            {
                new Person { Id = 1, Name = "Adam", Surname = "Kowalski" },
                new Person { Id = 2, Name = "Paweł", Surname = "Maliszewski" },
                new Person { Id = 3, Name = "Stasiek", Surname = "Nowak" },
                new Person { Id = 4, Name = "Jakub", Surname = "Wisniewski" },
                new Person { Id = 5, Name = "Adam", Surname = "Malinowski" },
                new Person { Id = 6, Name = "Kamil", Surname = "Kraszewski" },
                new Person { Id = 7, Name = "Nikodem", Surname = "Poziomkowski" },
                new Person { Id = 8, Name = "Adrian", Surname = "Stefaniuk" },
                new Person { Id = 9, Name = "Karol", Surname = "Kodorski" },
                new Person { Id = 10, Name = "Zenon", Surname = "Adamski" },
                new Person { Id = 11, Name = "Jaroslaw", Surname = "Kwasniewski" },
                new Person { Id = 12, Name = "Piotr", Surname = "Baranowski" },
                new Person { Id = 13, Name = "Jacek", Surname = "Osowski" },
                new Person { Id = 14, Name = "Adam", Surname = "Twardowski" },
                new Person { Id = 15, Name = "Kondrad", Surname = "Wisniewski" }
            };
        }
    }
}
