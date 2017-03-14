using DocumentAdder.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Model.SettingsModels
{
    public class RepositorySettingModel : BaseModel
    {
        #region Fields
        private string _pathToDirectory;
        private uint _scanTimeout;
        private ObservableCollection<RepositoryPath> _collectionPaths;
        #endregion

        #region Properties

        /// <summary>
        /// Представляет собой путь к папке с файлами, которые следует обработать
        /// </summary>
        /// <value>PathToDirectory свойство задает/возвращает значение типа string поля, _pathToDirectory</value>
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
        /// Предоставляет время обновления сканирования файлов в указаных директориях. Не может быть меньше 1. По умолчанию 1.
        /// <value>ScanTimeout свойство возвращает значения типа uint поля, _scanTimeout</value>
        /// </summary>
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
        /// <value>CollectionPaths свойство возвращает значение типа ObservableCollection<RepositoryPath> поля, _collectionPaths</value>
        public ObservableCollection<RepositoryPath> CollectionPaths
        {
            get
            {
                //return _collectionPaths;
                return ProgramSettings.CollectionsPaths;
            }
            private set
            {
                //_collectionPaths = value;
                ProgramSettings.CollectionsPaths = value;
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
            _collectionPaths = new ObservableCollection<RepositoryPath>();
        }        
    }
}
