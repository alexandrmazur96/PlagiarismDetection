using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace DocumentAdder.Types
{
    [DataContract]
    public class ProgramSettings
    {
        #region Properties
        #region DataBase Settings
        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string DataBaseName { get; set; }

        #endregion
        #region Repository Settings        

        [DataMember]
        public ObservableCollection<RepositoryPath> CollectionsPaths { get; set; }

        #endregion
        #region Other Text Settings
        [DataMember]
        public bool IsDoc { get; set; }

        [DataMember]
        public bool IsDocx { get; set; }

        [DataMember]
        public bool IsRtf { get; set; }

        [DataMember]
        public bool IsOtd { get; set; }

        [DataMember]
        public bool IsPdf { get; set; }

        [DataMember]
        public bool IsTxt { get; set; }

        [DataMember]
        public string FileTypes { get; set; }

        [DataMember]
        public string ReplacePath { get; set; }

        [DataMember]
        public int ThreadCount { get; set; }
        #endregion
        #endregion

        private static ProgramSettings _instanceSettings;

        private ProgramSettings()
        {
            CollectionsPaths = new ObservableCollection<RepositoryPath>();
            FileTypes = "";
        }

        /// <summary>
        /// Возвращает единственный экземпляр класса ProgramSettings. Необходим для паттерна Singleton
        /// </summary>
        /// <returns>ProgramSettings instance single object</returns>
        public static ProgramSettings GetInstance()
        {
            if (_instanceSettings == null)
            {
                _instanceSettings = new ProgramSettings();
                return _instanceSettings;
            }
            return _instanceSettings;
        }

        /// <summary>
        /// Инициализирует (загружает) настройки.
        /// </summary>
        /// <param name="loadedProgramSettings">Объект с настройками.</param>
        public static void LoadSettings(ProgramSettings loadedProgramSettings)
        {
            //other settings
            GetInstance().IsDoc = loadedProgramSettings.IsDoc;
            GetInstance().IsDocx = loadedProgramSettings.IsDocx;
            GetInstance().IsRtf = loadedProgramSettings.IsRtf;
            GetInstance().IsOtd = loadedProgramSettings.IsOtd;
            GetInstance().IsPdf = loadedProgramSettings.IsPdf;
            GetInstance().IsTxt = loadedProgramSettings.IsTxt;
            GetInstance().FileTypes = loadedProgramSettings.FileTypes;
            GetInstance().ReplacePath = loadedProgramSettings.ReplacePath;
            GetInstance().ThreadCount = loadedProgramSettings.ThreadCount;

            //database and repo
            GetInstance().CollectionsPaths = loadedProgramSettings.CollectionsPaths;
            GetInstance().ConnectionString = loadedProgramSettings.ConnectionString;
            GetInstance().DataBaseName = loadedProgramSettings.DataBaseName;
            GetInstance().Login = loadedProgramSettings.Login;
            GetInstance().Password = loadedProgramSettings.Password;
            
        }
    }
}
