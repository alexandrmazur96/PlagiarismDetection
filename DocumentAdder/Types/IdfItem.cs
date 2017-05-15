using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentAdder.Types
{
    public class IdfItem
    {

        #region Properties
        /// <summary>
        /// ID элемента IDF-вектора в MongoDB.
        /// </summary>
        [BsonId]
        public ObjectId IdfId { get; set; }

        /// <summary>
        /// Слово в MongoDb.
        /// </summary>
        [BsonElement("Token")]
        public string Token { get; set; }
        
        /// <summary>
        /// IDF-значения слова в MongoDb.
        /// </summary>
        [BsonElement("IdfValue")]
        public double IdfValue { get; set; }

        #endregion
    }
}