using System.Collections.Generic;
using MongoDB.Bson;

namespace dotnet_mongodb_experiment.Entities
{
    public class Note
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}