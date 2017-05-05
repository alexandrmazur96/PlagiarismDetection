using System;
using System.Windows.Media;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentAdder.Types
{
    public class Log
    {
        #region Properties
        [BsonId]
        public ObjectId LogId { get; set; }

        [BsonElement]
        public string Message { get; set; }

        [BsonElement]
        public DateTime Date { get; set; }

        [BsonElement]
        public Brush MessageColor { get; private set; }

        [BsonElement]
        public LogType LogType { get; private set; }

        public Log(string message, DateTime date, LogType logType = LogType.Message, ObjectId _id = default(ObjectId))
        {
            this.LogId = _id;
            Message = message;
            Date = date;
            LogType = logType;
            switch (logType)
            {
                case LogType.Error:
                    MessageColor = new SolidColorBrush(Colors.Red);
                    break;
                case LogType.Information:
                    MessageColor = new SolidColorBrush(Colors.DarkOrange);
                    break;
                case LogType.Message:
                    MessageColor = new SolidColorBrush(Colors.Black);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}