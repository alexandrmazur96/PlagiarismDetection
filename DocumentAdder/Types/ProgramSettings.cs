using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        //надо будет зашифровать!!!
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
        #endregion
        #endregion

        private static ProgramSettings _instanceSettings;

        public override string ToString()
        {
            return "ConnectionString = " + ConnectionString + "\n"
                + "Login = " + Login + "\n"
                + "Password = " + Password + "\n"
                + "DataBaseName = " + DataBaseName + "\n"
                + "FileTypes = " + FileTypes + "\n"
                + " " + IsDoc + " " + IsDocx + " " + IsOtd;
        }

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
            else
            {
                return _instanceSettings;
            }
        }

        public static void LoadSettings(ProgramSettings loadedProgramSettings)
        {
            ProgramSettings.GetInstance().IsDoc = loadedProgramSettings.IsDoc;
            ProgramSettings.GetInstance().IsDocx = loadedProgramSettings.IsDocx;
            ProgramSettings.GetInstance().IsRtf = loadedProgramSettings.IsRtf;
            ProgramSettings.GetInstance().IsOtd = loadedProgramSettings.IsOtd;
            ProgramSettings.GetInstance().IsPdf = loadedProgramSettings.IsPdf;
            ProgramSettings.GetInstance().IsTxt = loadedProgramSettings.IsTxt;
            ProgramSettings.GetInstance().FileTypes = loadedProgramSettings.FileTypes;
            ProgramSettings.GetInstance().CollectionsPaths = loadedProgramSettings.CollectionsPaths;
            ProgramSettings.GetInstance().ConnectionString = loadedProgramSettings.ConnectionString;
            ProgramSettings.GetInstance().DataBaseName = loadedProgramSettings.DataBaseName;
            ProgramSettings.GetInstance().Login = loadedProgramSettings.Login;
            ProgramSettings.GetInstance().Password = loadedProgramSettings.Password;
        }
    }
}
