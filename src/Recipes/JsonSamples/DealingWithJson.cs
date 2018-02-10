using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Recipes.JsonSamples
{
    public class JsonNet
    {
        class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }


        class Friend : Person
        {
            public List<Friend> Friends { get; set; } = new List<Friend>();

            public Friend(Person person)
            {
                Id = person.Id;
                Name = person.Name;
                Surname = person.Surname;
            }
        }


        class PersonRepository
        {
            public static PersonRepository Instance => new PersonRepository();

            public IEnumerable<Person> GetAll()
                => new List<Person>
                {
                    new Person { Id = 1, Name = "Adam", Surname = "Kowalski" },
                    new Person { Id = 2, Name = "Paweł", Surname = "Maliszewski" },
                    new Person { Id = 3, Name = "Stasiek", Surname = "Nowak" },
                    new Person { Id = 4, Name = "Jakub", Surname = "Wisniewski" },
                    new Person { Id = 5, Name = "Adam", Surname = "Malinowski" },
                    new Person { Id = 6, Name = "Kamil", Surname = "Kraszewski" },
                    new Person { Id = 7, Name = "Nikodem", Surname = "Poziomkowski" },
                    new Person { Id = 8, Name = "Adrian", Surname = "Stefaniuk" },
                    new Person { Id = 9, Name = "Karol", Surname = "Kodorski" }
                };
        }


        public static void SimpleSerializeDeserialize()
        {
            string jsonData = 
            @"{ 
                ""Id"":""1"",
                ""Name"":""Adam"",
                ""Surname"":""Kowalski""
            }";

            // Deserialization
            var person = JsonConvert.DeserializeObject<Person>(jsonData);
            // Serialization
            string jsonMinified = JsonConvert.SerializeObject(person);
            string jsonIndented = JsonConvert.SerializeObject(person, Formatting.Indented);
        }


        public static void ObjectReferenceSerialization()
        {
            IEnumerable<Person> people = PersonRepository.Instance.GetAll();

            Friend adam = new Friend(people.First(x => x.Name == "Adam"));
            Friend kamil = new Friend(people.First(x => x.Name == "Kamil"));
            Friend adrian = new Friend(people.First(x => x.Name == "Adrian"));
            Friend karol = new Friend(people.First(x => x.Name == "Karol"));

            adam.Friends.AddRange(new List<Friend> { kamil, adrian, adam });        // circular dependency

            // ERROR!! Self referencing loop detected with type 'Learning.Notes.JsonNet+Friend'. Path 'Friends'.
            // string adamJson = JsonConvert.SerializeObject(adam);

            string adamJson = JsonConvert.SerializeObject(adam, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });
            // OK
        }


        public static void WorkingWithExpandoObjects()
        {
            dynamic personExpando = new ExpandoObject();

            personExpando.FriendlyName = "Xavier Morera";
            personExpando.Courses = new List<string> { "Solr", "dotTrace", "Jira" };
            personExpando.Happy = true;

            string json = JsonConvert.SerializeObject(personExpando, Formatting.Indented);
            dynamic personDeserialized = JsonConvert.DeserializeObject(json);
        }


        public static void DifferentType()
        {
            string jsonData =
            @"{ 
                ""Id"":""1"",
                ""Name"":""Adam"",
                ""Surname"":""Kowalski""
            }";

            var person = JsonConvert.DeserializeObject(jsonData);   // annonymous type
            var personDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            foreach (KeyValuePair<string, string> item in personDictionary)
            {
                Console.WriteLine($"(Key,Value) => {item.Key} : {item.Value}");
            }
        }


        private static void JsonWriterTest()
        {
            // JsonTextReader  - large objects and files non-cached, forward only
            // JsonTextWriteer - control and performance non-cashed, forward only

            IEnumerable<Person> people = PersonRepository.Instance.GetAll();

            var adam = new Friend(people.First(x => x.Name == "Adam"));
            var kamil = new Friend(people.First(x => x.Name == "Kamil"));
            var adrian = new Friend(people.First(x => x.Name == "Adrian"));

            adam.Friends.AddRange(new List<Friend> { kamil, adrian });

            var simpleSerializer = new JsonSerializer();

            using (var sw = new StreamWriter("someJsonFile.json"))
            using (JsonWriter writter = new JsonTextWriter(sw))
            {
                simpleSerializer.Serialize(writter, adam);
            }


            var intendetSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using (var sw = new StreamWriter("someJsonFileIndented.json"))
            using (JsonWriter writter = new JsonTextWriter(sw))
            {
                intendetSerializer.Serialize(writter, adam);
            }

            var noNullSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            using (var sw = new StreamWriter("someJsonFileIgnoreNull.json"))
            using (JsonWriter writter = new JsonTextWriter(sw))
            {
                noNullSerializer.Serialize(writter, adam);
            }

        }


        private static void JsonReaderTest()
        {
            string jsonData =
            @"{ 
                ""Id"":""1"",
                ""Name"":""Adam"",
                ""Surname"":""Kowalski""
            }";

            JsonTextReader jsonReader = new JsonTextReader(new StringReader(jsonData));
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
    }
}
