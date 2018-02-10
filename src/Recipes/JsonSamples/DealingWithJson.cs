using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Recipes.JsonSamples
{
    public class JsonNet
    {
        private static readonly string fileFolder = @"sourcefile";

        private static string FilePath(string fileName)
        {
            return Path.Combine(fileFolder, fileName);
        }

        public static void Run()
        {
            //SimpleSerializeDeserialize();
            //ObjectReferenceSerialization();
            //WorkingWithExpandoObjects();
            //DifferentType();
            //JsonWriterTest();
            //JsonReaderTest();
        }


        private static void SimpleSerializeDeserialize()
        {
            string jsonData = @"{
                                    'Id':'1',
                                    'Name':'Adam',
                                    'Surname':'Kowalski'
                                }";

            // Deserialization
            var person = JsonConvert.DeserializeObject<Person>(jsonData);

            Console.WriteLine($"{person.Id} {person.Name} {person.Surname}" + "\n");


            // Serialization
            string jsonDeserialized = string.Empty;

            jsonDeserialized = JsonConvert.SerializeObject(person);
            Console.WriteLine(jsonDeserialized + "\n");

            jsonDeserialized = JsonConvert.SerializeObject(person, Formatting.Indented);
            Console.WriteLine(jsonDeserialized + "\n");
        }


        private static void ObjectReferenceSerialization()
        {
            IEnumerable<Person> people = PersonRepository.GetAll();

            Friend adam = new Friend(people.First(x => x.Name == "Adam"));
            Friend kamil = new Friend(people.First(x => x.Name == "Kamil"));
            Friend adrian = new Friend(people.First(x => x.Name == "Adrian"));
            Friend karol = new Friend(people.First(x => x.Name == "Karol"));

            adam.FavoriteFriends.AddRange(new List<Friend> { kamil, adrian, adam });        // circular dependency

            // ERROR!!
            // string adamJson = JsonConvert.SerializeObject(adam);     // Self referencing loop detected with type 'Learning.Notes.JsonNet+Friend'. Path 'FavoriteFriends'.

            string adamJson = JsonConvert.SerializeObject(adam, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });

            Console.WriteLine(adamJson);
        }


        private static void WorkingWithExpandoObjects()
        {
            dynamic personExpando = new ExpandoObject();

            personExpando.FriendlyName = "Xavier Morera";
            personExpando.Courses = new List<string> { "Solr", "dotTrace", "Jira" };
            personExpando.Happy = true;

            string jsonDynamic = JsonConvert.SerializeObject(personExpando, Formatting.Indented);
            Console.WriteLine(jsonDynamic);

            dynamic personDeserialized = JsonConvert.DeserializeObject(jsonDynamic);
            Console.WriteLine(personDeserialized.FriendlyName);

        }


        private static void DifferentType()
        {
            string jsonData = @"
                                {
                                    'Id':'1',
                                    'Name':'Adam',
                                    'Surname':'Maliszewski'
                                }
                               ";

            var person = JsonConvert.DeserializeObject(jsonData);   // annonymous type

            Dictionary<string, string> personDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            foreach (KeyValuePair<string, string> item in personDictionary)
            {
                Console.WriteLine($"(Key,Value) => {item.Key} : {item.Value}");
            }


            //var anonymousPerson = new
            //{
            //    author = string.Empty,
            //    happy = true,
            //    courses = 0,
            //    anotherproperty = string.Empty
            //};

            //var anotherAnonymous = JsonConvert.DeserializeAnonymousType(jsonData, anonymousPerson);

            //Console.WriteLine(anotherAnonymous.author);
        }


        private static void JsonWriterTest()
        {
            // JsonTextReader  - large objects and files non-cached, forward only
            // JsonTextWriteer - control andperformance non-cashed, forward only

            IEnumerable<Person> people = PersonRepository.GetAll();

            Friend adam = new Friend(people.First(x => x.Name == "Adam"));
            Friend kamil = new Friend(people.First(x => x.Name == "Kamil"));
            Friend adrian = new Friend(people.First(x => x.Name == "Adrian"));

            adam.FavoriteFriends.AddRange(new List<Friend> { kamil, adrian });

            JsonSerializer serializer;

            serializer = new JsonSerializer();

            using (var sw = new StreamWriter(FilePath("first.json")))
            {
                using (JsonWriter writter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writter, adam);
                }
            }


            serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            using (var sw = new StreamWriter(FilePath("first-indented.json")))
            {
                using (JsonWriter writter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writter, adam);
                }
            }

            serializer = new JsonSerializer();

            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(FilePath("first-ignorenull.json")))
            {
                using (JsonWriter writter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writter, adam);
                }

            }

        }


        private static void JsonReaderTest()
        {
            // JsonNet uses reflection to read/write data. We can avoid it using JsonTextReader|Writer

            string jsonText = @"
                                {
                                    'Id':'1',
                                    'Name':'Adam',
                                    'Surname':'Maliszewski'
                                }
                               ";

            JsonTextReader jsonReader = new JsonTextReader(new StringReader(jsonText));
            while (jsonReader.Read())
            {
                if (jsonReader.Value != null)
                {
                    Console.WriteLine("Token: {0}, Value: {1}", jsonReader.TokenType, jsonReader.Value);
                }
                else
                {
                    Console.WriteLine("Token: {0}", jsonReader.TokenType);
                }
            }

        }







        public class Friend : Person
        {
            public List<Friend> FavoriteFriends { get; set; }


            public Friend()
            {
                FavoriteFriends = new List<Friend>();
            }

            public Friend(Person person) : this()
            {
                Id = person.Id;
                Name = person.Name;
                Surname = person.Surname;
            }

        }
    }
}
