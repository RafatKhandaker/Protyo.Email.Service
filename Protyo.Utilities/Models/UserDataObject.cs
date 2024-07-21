using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Protyo.Utilities.Models
{
    public class UserDataObject
    {
        [BsonId]
        public ObjectId _Id { get; set; }

        public string? microsoftId { get; set; }
        public string? googleId { get; set; }

        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }

        public string? subscription { get; set; }
        public string? picture { get; set; }

        public FormData formInput { get; set; }

    }
}
