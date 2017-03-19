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
        private uint _scanTimeout;
        #endregion

        #region Properties       
        
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
                return ProgramSettings.CollectionsPaths;
            }
            set
            {
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
        }        
    }
}
