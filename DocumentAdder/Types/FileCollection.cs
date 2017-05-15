using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentAdder.Types
{
    public class FileCollection
    {
        #region Properties
        /// <summary>
        /// ID файла в MongoDb.
        /// </summary>
        [BsonId]
        public ObjectId FileId { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        [BsonElement("FileName")]
        public string FileName { get; set; }

        ///<summary>
        /// Путь к файлу.
        /// </summary>
        [BsonElement("FilePath")]
        public string FilePath { get; set; }

        /// <summary>
        /// Тип файла.
        /// </summary>
        [BsonElement("FileType")]
        public string FileType { get; set; }
        
        #endregion
    }
}
