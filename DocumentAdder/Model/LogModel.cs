using System;
using System.Collections.ObjectModel;
using DocumentAdder.Types;

namespace DocumentAdder.Model
{
    public class LogModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// Коллекция с логами приложения.
        /// </summary>
        public ObservableCollection<Log> LogMessages { get; set; }

        public LogModel()
        {
            LogMessages = new ObservableCollection<Log>();
        }        

        #endregion
    }
}