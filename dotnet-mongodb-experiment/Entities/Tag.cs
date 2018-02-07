using MongoDB.Bson;

namespace dotnet_mongodb_experiment.Entities
{
    public class Tag
    {
        public ObjectId Id { get; set; }
        public string TagName { get; set; }
        public User User { get; set; }
        // has many notes
    }
}