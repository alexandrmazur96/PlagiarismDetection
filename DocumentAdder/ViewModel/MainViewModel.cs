using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using DocumentAdder.Actions;
using DocumentAdder.Actions.DocumentAction;
using DocumentAdder.Helpers;
using DocumentAdder.Model;
using DocumentAdder.Types;
using DocumentAdder.Types.DataBase;
using DocumentAdder.Types.Exceptions;

namespace DocumentAdder.ViewModel
{
    public class MainViewModel
    {
        /// <summary>
        /// Количество поток, работающих в данный момент.
        /// </summary>
        private static int _threadCount;

        /// <summary>
        /// DispatcherTimer, нужен для non-stop работы приложения. Инициализируется в статическом 
        /// конструкторе, в едином экземпляре.
        /// </summary>
        private static readonly DispatcherTimer AutoWorkTimer;

        /// <summary>
        /// MainModel, модель данной части. Название походит от названия приложения.
        /// </summary>
        public static MainModel DocumentAdderModel { get; }

        /// <summary>
        /// Модель настроек.
        /// </summary>
        public static MainSettingModel SettingModel { get; set; }

        /// <summary>
        /// Модель логов.
        /// </summary>
        public static LogViewModel LogVm { get; set; }

        #region Commands
        //main programm commands
        /// <summary>
        /// Команда старта программы.
        /// </summary>
        public ICommand StartProgrammCommand { get; private set; }

        /// <summary>
        /// Команда остановки программы.
        /// </summary>
        public ICommand StopProgrammCommand { get; private set; }
        #endregion

        #region Methods

        //main programm methods        

        /// <summary>
        /// Старт программы.
        /// </summary>
        private async void StartProgramm()
        {
            //Если по старту программы, сервер MongoDb не работает или не доступен
            //то, прерываем программу
            if (!await DataBase.GetInstance().CheckMongoConnection())
            {
                StopProgramm(true);
                return;
            }

            //Обработчик тика "главного" таймера (AutoWorkTimer). Вызывается раз в час 
            //и добавляет данные.
            EventHandler autoWorkHandler = (sender, args) =>
            {
                //При нажатии кнопки "старт" сама кнопка блокируется, а кнопка "стоп" становится доступной
                DocumentAdderModel.IsStartBtnEnabled = false;
                DocumentAdderModel.IsStopBtnEnabled = true;

                //Получаем пути файлов из репозитория.
                var filePaths = DocumentActions.GetFilePaths();

                //Получаем энумератор, для асинхронного (параллельного) "прохождения" по коллекции файлов.
                if (filePaths == null) return;
                var filePathEnumerator = filePaths.GetEnumerator();

                //Поскольку программа работает параллельно, то выполнение потоков нужно отслеживать,
                //для этого создаем еще один таймер.
                //Смотрится экономнее в сравнении с циклом (while(true))
                var makeWorkTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(15) };

                //Обработчик "локального" таймера. Отслеживает количество потоков и количество оставшихся файлов.                
                EventHandler makeWorkHandler = async (mwSender, mwArgs) =>
                {
                    //Если текущее количество выполняемых потоков больше определенного количества
                    //(указывается в настройках), то текущий "тик" можно пропустить.
                    if (_threadCount >= ProgramSettings.GetInstance().ThreadCount) return;

                    //Цикл из количества доступных потоков, в которым идет обработка данных
                    for (; _threadCount < ProgramSettings.GetInstance().ThreadCount;)
                    {
                        //Проходимся по коллекции, если в ней еще есть файлы.
                        if (filePathEnumerator.MoveNext())
                        {
                            //Обрабатываем файлы                            
                            await MakeWork2Async(filePathEnumerator.Current);
                        }
                        //Если файлов не осталось - останавливаем "локальный" таймер и выходим из цикла.
                        else
                        {
                            makeWorkTimer.Stop();
                            break;
                        }
                    }
                };

                //Запускаем "локальный" таймер, привязывая к нему обработчик.
                makeWorkTimer.Tick += makeWorkHandler;
                makeWorkTimer.Start();
                makeWorkHandler(this, EventArgs.Empty);
            };

            //По нажатию кнопки старт мы запускаем "главный" таймер и программа начинает выполнять свою работу
            //сразу же.
            AutoWorkTimer.Tick += autoWorkHandler;
            AutoWorkTimer.Start();
            autoWorkHandler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Остановка программы.
        /// </summary>
        /// <param name="forceStop"></param>
        public static void StopProgramm(bool forceStop = false)
        {
            AutoWorkTimer.Stop();
            DocumentAdderModel.IsStartBtnEnabled = true;
            DocumentAdderModel.IsStopBtnEnabled = false;
            if (forceStop)
            {
                Console.Error.WriteLineAsync("Подключение к MongoDb отсутствует!");
                System.Windows.MessageBox.Show("Не удалось подключиться к серверу MongoDb!\n" +
                                               "Повторите попытку через некоторое время!", "Ошибка подключения к серверу MongoDb!");
                LogViewModel.AddNewLog("Не удалось подключиться к серверу MongoDb!", DateTime.Now, LogType.Error);
            }
        }

        /// <summary>
        /// Обработка файла по указанному пути.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns></returns>
        private static async Task MakeWork2Async(string filePath)
        {
            //Проверяем состояние сервера MongoDb.
            if (await DataBase.GetInstance().CheckMongoConnection())
            {
                //Инкрементируем к-ство выполняемых потоков.
                ++_threadCount;
                if (!await DataBase.GetInstance().IsFileInDbAsync(FileActions.FileHash(filePath)))
                {
                    var canonedTokens = DocumentActions.GetWordCanonedTokens(filePath);
                    DocumentActions.Cyrillify(ref canonedTokens);
                    var extension = Path.GetExtension(filePath);
                    //эта переменная будет использоваться для занесения в базу пути файла и ее производных
                    var newFilePath = filePath;
                    //поскольку формат *.doc пересохраняется в *.docx после вызова метода GetWordCanonedTokens,
                    //то при вставке документа в БД используется новый формат *.docx
                    if (extension != null && extension.Equals(".doc"))
                    {
                        newFilePath += "x";
                    }
                    newFilePath = FileActions.FileMoveOrDelete(newFilePath, FileActionType.RenameAndMove);
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    //Здесь будут хранится данные из имени файла.
                    string[] fileNameData;
                    try
                    {
                        fileNameData = DocumentActions.SplitFileName(fileName);
                    }
                    catch (FileNameFormatException ex)
                    {
                        LogViewModel.AddNewLog(ex.Message + " " + fileName, DateTime.Now, LogType.Information);
                        fileNameData = new[]
                        {
                            "Undefined", //ФИО-автора
                            "Undefined", //Группа автора
                            fileName //Название работы
                        };
                    }
                    //создаем новый документ <Document> для вставки в коллекцию
                    var insertDoc = new Document(
                        fileNameData[2], //Здесь хранится имя документа
                        fileNameData[0], //Здесь хранится ФИО автора документа
                        fileNameData[1], //Здесь хранится группа автора документа
                        newFilePath,
                        Path.GetExtension(newFilePath),
                        FileActions.FileHash(newFilePath),
                        canonedTokens.ToArray(),
                        DocumentActions.MakeTfVector(canonedTokens),
                        File.GetLastWriteTime(newFilePath));
                    //вставляем в БД
                    await DataBase.GetInstance().InsertDocumentAsync(insertDoc);
                }
                else
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    LogViewModel.AddNewLog("Файл " + fileName + " уже есть в базе данных!", DateTime.Now, LogType.Information);
                    FileActions.FileMoveOrDelete(filePath, FileActionType.MoveDuplicate);
                }
                --_threadCount;
            }
            else
            {
                StopProgramm(true);
            }
        }
        //settings methods

        //other methods
        
        #endregion

        static MainViewModel()
        {
            if (AutoWorkTimer == null)
            {
                var single = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3600) };
                AutoWorkTimer = single;
            }

            if (SettingModel == null)
            {
                var single = new MainSettingModel();
                SettingModel = single;
            }
            if (LogVm == null)
            {
                var single = new LogViewModel();
                LogVm = single;
            }
            if (DocumentAdderModel == null)
            {
                var single = new MainModel();
                DocumentAdderModel = single;
            }
        }

        public MainViewModel()
        {
            StartProgrammCommand = new DelegateCommand(action => StartProgramm());
            StopProgrammCommand = new DelegateCommand(action => StopProgramm());
        }
    }
}
