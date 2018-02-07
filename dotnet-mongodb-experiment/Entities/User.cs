using MongoDB.Bson;

namespace dotnet_mongodb_experiment.Entities
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
    }
}