using System.Runtime.Serialization;

namespace PlagiarismDetector.Types
{
    [DataContract]
    public class ProgramSettings
    {
        private static ProgramSettings _instanceSettings;

        #region Properties
                   
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
        public int ThreadCount { get; set; }

        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string DataBaseName { get; set; }
        #endregion

        private ProgramSettings()
        {
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

        public static void LoadSettings(ProgramSettings loadedProgramSettings)
        {
            //other settings
            ProgramSettings.GetInstance().IsDoc = loadedProgramSettings.IsDoc;
            ProgramSettings.GetInstance().IsDocx = loadedProgramSettings.IsDocx;
            ProgramSettings.GetInstance().IsRtf = loadedProgramSettings.IsRtf;
            ProgramSettings.GetInstance().IsOtd = loadedProgramSettings.IsOtd;
            ProgramSettings.GetInstance().IsPdf = loadedProgramSettings.IsPdf;
            ProgramSettings.GetInstance().IsTxt = loadedProgramSettings.IsTxt;
            ProgramSettings.GetInstance().FileTypes = loadedProgramSettings.FileTypes;
            ProgramSettings.GetInstance().ThreadCount = loadedProgramSettings.ThreadCount;

            //database and repo
            ProgramSettings.GetInstance().ConnectionString = loadedProgramSettings.ConnectionString;
            ProgramSettings.GetInstance().DataBaseName = loadedProgramSettings.DataBaseName;
            ProgramSettings.GetInstance().Login = loadedProgramSettings.Login;
            ProgramSettings.GetInstance().Password = loadedProgramSettings.Password;
        }
    }
}