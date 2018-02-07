using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using NUnit.Framework;

namespace WcfExperiment
{
    public class RestApiTest
    {
        [Test]
        public void CanTalkToRestApiWithWcfClient()
        {
            WithServer(() =>
            {
                using (var channelFactory = new ChannelFactory<INoteService>(new WebHttpBinding(), "http://localhost:2302/"))
                {
                    channelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());

                    var client = channelFactory.CreateChannel();
                    var note = client.GetNote("123");
                    Assert.AreEqual("123", note.Id);
                    Assert.AreEqual("Text for note #123", note.Text);
                }
            });
        }

        [Test]
        public void CanTalkToRestApiWithHttpClient()
        {
            WithServer(() =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:2302")
                };

                var response = httpClient.GetAsync("/notes/123").Result;
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                var json = response.Content.ReadAsStringAsync().Result;
                Assert.AreEqual("{\"Id\":\"123\",\"Text\":\"Text for note #123\"}", json);
            });
        }

        private static void WithServer(Action whenServerIsUp)
        {
            var noteService = new NoteService();
            using (var serviceHost = new ServiceHost(noteService))
            {
                var endpoint = serviceHost.AddServiceEndpoint(typeof(INoteService), new WebHttpBinding(), "http://localhost:2302/");
                endpoint.Behaviors.Add(new WebHttpBehavior());

                serviceHost.Open();

                whenServerIsUp();
            }
        }

        [DataContract]
        public class Note
        {
            [DataMember]
            public string Id { get; set; }

            [DataMember]
            public string Text { get; set; }
        }

        [ServiceContract]
        public interface INoteService
        {
            [WebInvoke(
                Method = "GET", UriTemplate = "notes/{id}", 
                RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
            Note GetNote(string id);
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class NoteService : INoteService
        {
            public Note GetNote(string id)
            {
                return new Note
                {
                    Id = id,
                    Text = string.Format("Text for note #{0}", id)
                };
            }
        }
    }
}