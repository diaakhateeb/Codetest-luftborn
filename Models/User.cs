using Models.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text;


namespace Models
{
    [Serializable]
    public class User : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string Name { get; set; }


        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string UserName { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string Email { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string PhoneNumber { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string Address { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public DateTime? CreatedAt { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public DateTime? AddedAt { get; set; }

        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public DateTime? ModifiedAt { get; set; }

        public string Token { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(this.GetType());
            stringBuilder.Append(Environment.NewLine);
            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                stringBuilder.Append($"\t{propertyInfo.Name}: {propertyInfo.GetValue(this, null)}{Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }
    }
}
