using System;
using NUnit.Framework;
using Newtonsoft.Json;
using Noesis.Javascript;

namespace JavaScriptDotNetExperiment
{
    public class JavaScriptTests
    {
        [Test]
        public void ExpressionEvaluationShouldWork()
        {
            using (var context = new JavascriptContext())
            {
                var result = Convert.ToInt32(context.Run("1 + 2"));
                Assert.AreEqual(3, result);
            }
        }

        [Test]
        public void CanDefineFunctionAndThenCallIt()
        {
            using (var context = new JavascriptContext())
            {
                context.Run("function addNumbers(a, b) { return a + b; };");
                var result = Convert.ToInt32(context.Run("addNumbers(1, 2)"));
                Assert.AreEqual(3, result);
            }
        }

        [Test]
        public void CanPassDTO()
        {
            using (var context = new JavascriptContext())
            {
                context.Run("function getId(person) { return person.id; };");
                context.Run("function getName(person) { return person.name; };");

                var person = new Person
                    {
                        Id = 123,
                        Name = "loki2302"
                    };
                var personJson = JsonConvert.SerializeObject(person);

                var id = Convert.ToInt32(context.Run(string.Format("getId({0})", personJson)));
                Assert.AreEqual(123, id);

                var name = Convert.ToString(context.Run(string.Format("getName({0})", personJson)));
                Assert.AreEqual("loki2302", name);
            }
        }

        [Test]
        public void CanGetDTO()
        {
            using (var context = new JavascriptContext())
            {
                context.Run("function getPerson() { return {id: 123, name: 'loki2302'}; };");

                var result = context.Run("getPerson()");
                var personJson = JsonConvert.SerializeObject(result); // WEIRD APPROACH!!!
                var person = JsonConvert.DeserializeObject<Person>(personJson);
                
                Assert.AreEqual(123, person.Id);
                Assert.AreEqual("loki2302", person.Name);
            }
        }
    }

    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
