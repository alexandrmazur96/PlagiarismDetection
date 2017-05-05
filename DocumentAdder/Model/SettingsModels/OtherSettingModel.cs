using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Helpers;
using DocumentAdder.Types;

namespace DocumentAdder.Model.SettingsModels
{
    public class OtherSettingModel : BaseModel
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
                ProgramSettings.FileTypes = Transformation.FlagToExtensions();
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
                ProgramSettings.FileTypes = Transformation.FlagToExtensions();
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
                ProgramSettings.FileTypes = Transformation.FlagToExtensions();
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
                ProgramSettings.FileTypes = Transformation.FlagToExtensions();
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
                Console.WriteLine(Transformation.FlagToExtensions());
                ProgramSettings.FileTypes = Transformation.FlagToExtensions();
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
                ProgramSettings.FileTypes = Transformation.FlagToExtensions();
            }
        }

        /// <summary>
        /// Показывает путь, куда следует перемещать обработанные файлы
        /// </summary>
        public string ReplacePath
        {
            get { return ProgramSettings.ReplacePath; }
            set
            {
                ProgramSettings.ReplacePath = value;
                NotifyPropertyChanged();                
            }
        }

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

        #endregion

        public OtherSettingModel()
        {
            if (string.IsNullOrWhiteSpace(ReplacePath))
            {
                ReplacePath = "Директория не выбрана, выберите, пожалуйста, директорию!";
            }
        }
    }
}
