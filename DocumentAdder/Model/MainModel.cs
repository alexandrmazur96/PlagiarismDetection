using DocumentAdder.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Types;

namespace DocumentAdder.Model
{
    public class MainModel : BaseModel
    {
        #region Fields
        private ObservableCollection<Document> _documentCollections;
        private bool _isStartBtnEnabled;
        private bool _isStopBtnEnabled;
        #endregion

        #region Properties

        /// <summary>
        /// Возвращает коллекцию документов.
        /// <value>DocumentCollections свойство возвращает значение типа ObservableCollection<Document> поля, _documentCollections</value>
        /// </summary>
        public ObservableCollection<Document> DocumentCollections
        {
            get
            {
                return _documentCollections;
            }

            private set
            {
                _documentCollections = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, доступна ли кнопка "Старт" в данный момент
        /// <value>IsStartBtnEnabled свойство возвращает значение типа bool поля, _isStartBtnEnabled</value>
        /// </summary>
        public bool IsStartBtnEnabled
        {
            get
            {
                return _isStartBtnEnabled;
            }
            set
            {
                _isStartBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, доступна ли кнопка "Стоп" в данный момент
        /// <value>IsStopBtnEnabled свойство возвращает значение типа bool поля, _isStopBtnEnabled</value>
        /// </summary>
        public bool IsStopBtnEnabled
        {
            get
            {
                return _isStopBtnEnabled;
            }
            set
            {
                _isStopBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// Создает модель и инициализируем коллекцию документов
        /// </summary>
        public MainModel()
        {
            _documentCollections = new ObservableCollection<Document>();
            _isStartBtnEnabled = true;
            _isStopBtnEnabled = false;
        }

    }
}
