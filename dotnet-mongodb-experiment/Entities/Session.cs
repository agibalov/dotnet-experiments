using MongoDB.Bson;

namespace dotnet_mongodb_experiment.Entities
{
    public class Session
    {
        public ObjectId Id { get; set; }
        public User User { get; set; } // use MongoDBRef here?
    }
}