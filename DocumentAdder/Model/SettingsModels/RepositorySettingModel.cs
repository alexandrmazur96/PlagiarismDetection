using DocumentAdder.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Model.SettingsModels
{
    [DataContract]
    public class RepositorySettingModel : BaseModel
    {
        #region Fields
        private string _pathToDirectory;
        private string _fileTypes;
        private uint _scanTimeout;
        private ObservableCollection<RepositoryPath> _collectionPaths;
        #endregion

        #region Properties

        /// <summary>
        /// Представляет собой путь к папке с файлами, которые следует обработать
        /// </summary>
        /// <value>PathToDirectory свойство задает/возвращает значение типа string поля, _pathToDirectory</value>
        [DataMember]
        public string PathToDirectory
        {
            get
            {
                return _pathToDirectory;
            }
            set
            {
                _pathToDirectory = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Предоставляет данные о типах текстовых файлов, которые необходимо сканировать и обрабатывать
        /// <value>FileTypes свойство возвращает значения типа string поля, _fileTypes</value>
        /// </summary>       
        [DataMember]
        public string FileTypes
        {
            get
            {
                return _fileTypes;
            }
            private set
            {
                _fileTypes = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Предоставляет время обновления сканирования файлов в указаных директориях. Не может быть меньше 1. По умолчанию 1.
        /// <value>ScanTimeout свойство возвращает значения типа uint поля, _scanTimeout</value>
        /// </summary>
        [DataMember]
        public uint ScanTimeout
        {
            get
            {
                return _scanTimeout;
            }
            set
            {
                _scanTimeout = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Возвращает коллекцию всех путей, с которых нужно обработать файлы
        /// </summary>
        /// <value>CollectionPaths свойство возвращает значение типа ObservableCollection<string> поля, _collectionPaths</value>
        [DataMember]
        public ObservableCollection<RepositoryPath> CollectionPaths
        {
            get
            {
                return _collectionPaths;
            }
            private set
            {
                _collectionPaths = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// Создает модель настроек, 
        /// инициализирует коллекцию путей к директориям, где лежат файлы,
        /// задает возможные форматы файлов для чтения.
        /// </summary>
        public RepositorySettingModel()
        {
            _fileTypes = "*.txt, *.doc, *.docx, *.rtf, *.otd, *.pdf";
            _collectionPaths = new ObservableCollection<RepositoryPath>();
        }        
    }
}
