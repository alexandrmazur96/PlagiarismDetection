using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlagiarismDetector.Types;
using PlagiarismDetector.View.About;
using PlagiarismDetector.View.Settings;

namespace PlagiarismDetector.Model
{
    public class MainModel : BaseModel
    {
        #region Fields

        private ObservableCollection<string> _unchackedFiles;
        private ObservableCollection<PlagiarismDetectResult> _handledFiles;

        #region Enabled Buttons Indicators
       
        private bool _isStopBtnEnabled;
        private bool _isStartBtnEnabled;
        private bool _isPrintResultBtnEnabled;
        private bool _isSaveToFileBtnEnabled;
        private bool _isChooseFileBtnEnabled;
        private bool _isClearResultBtnEnabled;

        #endregion

        #endregion

        #region Properties

        #region Enabled Buttons Indicators Properties

        /// <summary>
        /// Показывает, включена ли кнопка "Старт".
        /// </summary>
        public bool IsStartBtnEnabled
        {
            get { return _isStartBtnEnabled; }
            set
            {
                _isStartBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, включена ли кнопка "Стоп".
        /// </summary>
        public bool IsStopBtnEnabled
        {
            get { return _isStopBtnEnabled;}
            set
            {
                _isStopBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, включена ли кнопка "Печать результата".
        /// </summary>
        public bool IsPrintResultBtnEnabled
        {
            get
            {
                return _isPrintResultBtnEnabled;
            }
            set
            {
                _isPrintResultBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, включена ли кнопка "Сохранения в файл".
        /// </summary>
        public bool IsSaveToFileBtnEnabled
        {
            get { return _isSaveToFileBtnEnabled; }
            set
            {
                _isSaveToFileBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, включена ли кнопка "Выбора файлов для обработки".
        /// </summary>
        public bool IsChooseFileBtnEnabled
        {
            get { return _isChooseFileBtnEnabled; }
            set
            {
                _isChooseFileBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Показывает, включена ли кнопка "Очистки результата".
        /// </summary>
        public bool IsClearResultBtnEnabled
        {
            get
            {
                return _isClearResultBtnEnabled;
            }
            set
            {
                _isClearResultBtnEnabled = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Коллекция не проверенных файлов.
        /// </summary>
        public ObservableCollection<string> UncheckedFiles
        {
            get { return _unchackedFiles; }
            set
            {
                _unchackedFiles = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция обработанных файлов.
        /// </summary>
        public ObservableCollection<PlagiarismDetectResult> HandledFiles
        {
            get
            {
                return _handledFiles;
            }
            set
            {
                _handledFiles = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Окно "Об авторе".
        /// </summary>
        public AboutAuthor AboutAuthorWindow { get; set; }

        /// <summary>
        /// Окно "О программе".
        /// </summary>
        public AboutProgramm AboutProgrammWindow { get; set; }

        /// <summary>
        /// Окно "Настройки".
        /// </summary>
        public Settings SettingsWindow { get; set; }
        #endregion        

        public MainModel()
        {
            _unchackedFiles = new ObservableCollection<string>();
            _handledFiles = new ObservableCollection<PlagiarismDetectResult>();

            _isStartBtnEnabled = false;
            _isStopBtnEnabled = false;
            _isSaveToFileBtnEnabled = false;
            _isPrintResultBtnEnabled = false;
            _isChooseFileBtnEnabled = true;
            _isClearResultBtnEnabled = false;
        }
    }
}