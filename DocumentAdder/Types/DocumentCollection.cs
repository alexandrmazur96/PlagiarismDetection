using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentAdder.Types
{
    public class DocumentCollection
    {
        #region Properties
        /// <summary>
        /// ID документа
        /// </summary>
        [BsonId]
        public ObjectId DocumentId { get; set; }

        /// <summary>
        /// Хеш-сумма документа
        /// </summary>
        [BsonElement("DocumentHash")]
        public string DocumentHash { get; set; }

        /// <summary>
        /// Автор документа
        /// </summary>
        [BsonElement("DocumentAuthor")]
        public string DocumentAuthor { get; set; }

        /// <summary>
        /// Группа автора документа
        /// </summary>
        [BsonElement("DocumentAuthorGroup")]
        public string DocumentAuthorGroup { get; set; }

        /// <summary>
        /// Канонизированный текст документа (все слова)
        /// </summary>
        [BsonElement("DocumentTokens")]
        public string[] DocumentTokens { get; set; }

        /// <summary>
        /// TF-вектор документа
        /// </summary>
        [BsonElement("DocumentTfVector")]
        public Dictionary<string, double> DocumentTfVector { get; set; }

        /// <summary>
        /// Дата добавления файла
        /// </summary>
        [BsonElement("DocumentAddTime")]
        public DateTime DocumentAddTime { get; set; }               

        /// <summary>
        /// Представляет собой ссылку на прикрепленный файл (файл, относящийся к этому документу)
        /// </summary>
        [BsonElement("FileId")]
        public object FileId { get; set; }
        #endregion
    }
}
