using System;
using System.Text;
using MongoDB.Driver;
using dotnet_mongodb_experiment.Entities;

namespace dotnet_mongodb_experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://win7dev-home");
            var server = client.GetServer();
            var database = server.GetDatabase("dotnet");
            
            var userCollection = database.GetCollection<User>("users");
            var sessionCollection = database.GetCollection<Session>("sessions");
            var tagCollection = database.GetCollection<Tag>("tags");
            var noteCollection = database.GetCollection<Note>("notes");
            var service = new RenoteService(
                userCollection, 
                sessionCollection, 
                tagCollection, 
                noteCollection);

            service.RemoveEverything();
            var authentication = service.Authenticate("loki2302");
            Console.WriteLine("{0} {1}", authentication.SessionToken, authentication.UserName);
        }
    }
}
