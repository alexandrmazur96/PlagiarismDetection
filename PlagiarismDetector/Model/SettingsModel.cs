using PlagiarismDetector.Helpers;
using PlagiarismDetector.Types;

namespace PlagiarismDetector.Model
{
    public class SettingsModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// Указывает, выбраны ли файлы с расширением .doc для анализа и обработки.
        /// </summary>
        public bool IsDoc
        {
            get { return ProgramSettings.IsDoc; }
            set
            {
                ProgramSettings.IsDoc = value;
                ProgramSettings.FileTypes = Transformations.FlagToExtensions();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Указывает, выбраны ли файлы с расширением .docx для анализа и обработки.
        /// </summary>
        public bool IsDocx
        {
            get { return ProgramSettings.IsDocx; }
            set
            {
                ProgramSettings.IsDocx = value;
                NotifyPropertyChanged();
                ProgramSettings.FileTypes = Transformations.FlagToExtensions();
            }
        }

        /// <summary>
        /// Указывает, выбраны ли файлы с расширением .rtf для анализа и обработки.
        /// </summary>
        public bool IsRtf
        {
            get { return ProgramSettings.IsRtf; }
            set
            {
                ProgramSettings.IsRtf = value;
                NotifyPropertyChanged();
                ProgramSettings.FileTypes = Transformations.FlagToExtensions();
            }
        }

        /// <summary>
        /// Указывает, выбраны ли файлы с расширением .otd для анализа и обработки.
        /// </summary>
        public bool IsOtd
        {
            get { return ProgramSettings.IsOtd; }
            set
            {
                ProgramSettings.IsOtd = value;
                NotifyPropertyChanged();
                ProgramSettings.FileTypes = Transformations.FlagToExtensions();
            }
        }

        /// <summary>
        /// Указывает, выбраны ли файлы с расширением .pdf для анализа и обработки.
        /// </summary>
        public bool IsPdf
        {
            get { return ProgramSettings.IsPdf; }
            set
            {
                ProgramSettings.IsPdf = value;
                NotifyPropertyChanged();
                ProgramSettings.FileTypes = Transformations.FlagToExtensions();
            }
        }

        /// <summary>
        /// Указывает, выбраны ли файлы с расширением .txt для анализа и обработки.
        /// </summary>
        public bool IsTxt
        {
            get { return ProgramSettings.IsTxt; }
            set
            {
                ProgramSettings.IsTxt = value;
                NotifyPropertyChanged();
                ProgramSettings.FileTypes = Transformations.FlagToExtensions();
            }
        }

        /// <summary>
        /// Показывает количество одновременно выполняемых потоков.
        /// </summary>
        public int ThreadCount
        {
            get { return ProgramSettings.ThreadCount; }
            set
            {
                if (value <= 0)
                {
                    ProgramSettings.ThreadCount = 1;
                    NotifyPropertyChanged();
                }
                else
                {
                    ProgramSettings.ThreadCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Показывает логин для подключения к MongoDb.
        /// </summary>
        public string Login
        {
            get { return ProgramSettings.Login; }
            set
            {
                ProgramSettings.Login = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает пароль для подключения к MongoDb.
        /// </summary>
        public string Password
        {
            get
            {
                return ProgramSettings.Password;
            }
            set
            {
                ProgramSettings.Password = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает строку подключения к серверу MongoDb.
        /// </summary>
        public string ConnectionString
        {
            get { return ProgramSettings.ConnectionString; }
            set
            {
                ProgramSettings.ConnectionString = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает имя БД, в которой хранятся данные.
        /// </summary>
        public string DataBaseName
        {
            get { return ProgramSettings.DataBaseName; }
            set
            {
                ProgramSettings.DataBaseName = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}