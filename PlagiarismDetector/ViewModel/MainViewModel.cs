using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using PlagiarismDetector.Helpers;
using PlagiarismDetector.Model;
using PlagiarismDetector.Types;
using PlagiarismDetector.View.About;
using PlagiarismDetector.View.Settings;
using System.Threading.Tasks;
using System.Windows.Controls;
using DocumentAdder.Actions.DocumentAction;
using DocumentAdder.Types.DataBase;
using FontStyle = System.Drawing.FontStyle;

namespace PlagiarismDetector.ViewModel
{
    public class MainViewModel
    {
        private static int _threadCount = 0;

        public MainModel M_Model { get; }

        private DispatcherTimer AutoWorkTimer { get; }

        private static DataBase _dbInstance;

        #region Commands

        public ICommand ChooseFilesCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public ICommand SaveToFileCommand { get; private set; }

        public ICommand PrintResultCommand { get; private set; }

        public ICommand ClearResultCommand { get; private set; }

        public ICommand ExitCommand { get; private set; }

        public ICommand ShowAboutProgrammCommand { get; private set; }

        public ICommand ShowAboutAuthorCommand { get; private set; }

        public ICommand ShowSettingsCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Выбирает файлы для проверки.
        /// </summary>
        private void ChooseFiles()
        {
            var cofd = new CommonOpenFileDialog
            {
                Title = "Выберите файлы для проверки.",
                Multiselect = true
            };

            if (cofd.ShowDialog() != CommonFileDialogResult.Ok) return;

            foreach (var fileName in cofd.FileNames)
            {
                M_Model.UncheckedFiles.Add(fileName);
            }

            CheckButtonsEnabled();
        }


        /// <summary>
        /// Старт проверки.
        /// </summary>
        private async void StartProgramm()
        {
            if (_dbInstance == null)
            {
                ReinitDbInstance();
            }

            if (_dbInstance == null)
            {
                StopProgramm();
                return;
            }

            M_Model.IsStartBtnEnabled = false;
            M_Model.IsStopBtnEnabled = true;
            M_Model.IsChooseFileBtnEnabled = false;

            if (_dbInstance != null && !await _dbInstance.CheckMongoConnection())
            {
                StopProgramm();
                return;
            }

            var fileLists = new List<string>(M_Model.UncheckedFiles.ToList());

            var uncheckedFilesEnumerator = fileLists.GetEnumerator();

            var makeWorkTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };

            EventHandler autoWorkHandler = (sender, args) =>
            {
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
                        if (uncheckedFilesEnumerator.MoveNext())
                        {
                            //Обрабатываем файлы                            
                            await MakeWorkAsync(uncheckedFilesEnumerator.Current);
                            //После обработки, удаляем его                            
                            M_Model.UncheckedFiles.Remove(uncheckedFilesEnumerator.Current);
                        }
                        //Если файлов не осталось - останавливаем все таймеры и выходим из цикла.
                        else
                        {
                            makeWorkTimer.Stop();
                            StopProgramm();
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
        /// Остановка проверки.
        /// </summary>
        private void StopProgramm()
        {
            M_Model.IsStopBtnEnabled = false;
            M_Model.IsStartBtnEnabled = true;
            M_Model.IsChooseFileBtnEnabled = true;
            CheckButtonsEnabled();
            AutoWorkTimer.Stop();
        }

        /// <summary>
        /// Асинхронная запись результатов в файл.
        /// </summary>
        private async void SaveToFile()
        {
            var cofd = new CommonOpenFileDialog
            {
                Title = "Выберите файл для сохранения",
                Multiselect = false,
                IsFolderPicker = false
            };

            if (cofd.ShowDialog() != CommonFileDialogResult.Ok) return;

            var result = new StringBuilder();

            result.Append("Creating date - ").Append(DateTime.Now.ToLongDateString()).AppendLine();
            result.AppendLine($"{"File Name:",-50}{"Plagiarism Result:",10}");


            foreach (var handledFile in M_Model.HandledFiles)
            {
                result.AppendLine($"{handledFile.FileName,-50}{handledFile.Value.ToString(CultureInfo.CurrentCulture),10}");
            }

            var resultFile = new FileInfo(cofd.FileName);
            if (!resultFile.Exists)
            {
                resultFile.Create();
            }

            if (resultFile.IsReadOnly)
            {
                resultFile.IsReadOnly = false;
            }

            using (var fs = resultFile.OpenWrite())
            {
                using (var writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(result.ToString());
                }
            }
        }

        /// <summary>
        /// Печать результата.
        /// </summary>
        private void PrintResult()
        {
            var pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                var doc = new PrintDocument();
                doc.DocumentName = "Результаты";

                PrintPageEventHandler ppeh = (sender, e) =>
                {
                    var result = new StringBuilder();

                    result.Append("Creating date - ").Append(DateTime.Now.ToLongDateString()).AppendLine();
                    result.AppendLine($"{"File Name:",-50}{"Plagiarism Result:",10}");


                    foreach (var handledFile in M_Model.HandledFiles)
                    {
                        result.AppendLine($"{handledFile.FileName,-50}{handledFile.Value.ToString(CultureInfo.CurrentCulture),10}");
                    }

                    int y;
                    var myFont = new Font("Times New Roman", 14, FontStyle.Regular);
                    y = e.MarginBounds.Y;
                    e.Graphics.DrawString(result.ToString(), myFont, Brushes.Black, e.MarginBounds.X, y);
                };

                doc.PrintPage += ppeh;

            }

            throw new NotImplementedException();

        }

        /// <summary>
        /// Очищает результаты работы приложения.
        /// </summary>
        private void ClearResult()
        {
            var acceptClear = MessageBox.Show("Вы подтверждаете очистку всех результатов проверки?",
                "Подтверждение очистки",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (acceptClear == MessageBoxResult.Yes)
            {
                M_Model.HandledFiles.Clear();
                CheckButtonsEnabled();
            }
        }

        /// <summary>
        /// Выход из программы.
        /// </summary>
        private void Exit()
        {
            var acceptClear = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (acceptClear == MessageBoxResult.Yes)
            {
                StopProgramm();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Показать настройки.
        /// </summary>
        private void ShowSettings()
        {
            M_Model.SettingsWindow = new Settings();
            M_Model.SettingsWindow.ShowDialog();
        }

        /// <summary>
        /// Показать "о программе".
        /// </summary>
        private void ShowAboutProgramm()
        {
            M_Model.AboutProgrammWindow = new AboutProgramm();
            M_Model.AboutProgrammWindow.ShowDialog();
        }

        /// <summary>
        /// Показать "об авторе".
        /// </summary>
        private void ShowAboutAuthor()
        {
            M_Model.AboutAuthorWindow = new AboutAuthor();
            M_Model.AboutAuthorWindow.ShowDialog();
        }


        private async Task MakeWorkAsync(string filePath)
        {
            _threadCount++;
            try
            {
                if (await _dbInstance.CheckMongoConnection())
                {
                    var documentTfIdfDict = await DocumentActions.MakeTfIdfVectorAsync(
                        DocumentActions.MakeTfVector(DocumentActions.GetWordCanonedTokens(filePath)));

                    var allDocuments = await _dbInstance.GetAllDocumentsAsync();

                    foreach (var document in allDocuments)
                    {
                        var currentDocumentTfIdfDict = await DocumentActions.MakeTfIdfVectorAsync(
                            document.DocumentTfVector);

                        var vectors = MakeVectorsForCompare(currentDocumentTfIdfDict, documentTfIdfDict);

                        var cosineSimilarity = Cosine_similarity(vectors.Item1, vectors.Item2);

                        Console.WriteLine(cosineSimilarity.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                await Console.Error.WriteLineAsync("Не удалось подключиться к серверу MongoDb! \nВыполнение дальнейшей работы невозможно!\n" +
                    e.Message + "\n" + e.ToString());
                StopProgramm();
            }
            _threadCount--;
        }

        /// <summary>
        /// Находит косинусное сходство между двумя векторами.
        /// </summary>
        /// <param name="vector1">Вектор 1.</param>
        /// <param name="vector2">Вектор 2.</param>
        /// <returns>Значения сходство косинуса.</returns>
        private static double Cosine_similarity(IReadOnlyList<double> vector1, IReadOnlyList<double> vector2)
        {
            if (vector1.Count != vector2.Count) throw new Exception("Размеры векторов разные!");

            double summ_xx = 0, summ_yy = 0, summ_xy = 0;
            for (int i = 0; i < vector1.Count; i++)
            {
                var x = vector1[i];
                var y = vector2[i];
                summ_xx += x * x;
                summ_yy += y * y;
                summ_xy += x * y;
            }
            var result = summ_xy / Math.Sqrt(summ_xx * summ_yy);
            return double.IsNaN(result) ? 0 : result;
        }

        /// <summary5>
        /// Создает два вектора одинаковых размеров (равные Length) на основании двух словарей TF*IDF значений.
        /// </summary>
        /// <param name="doc1">Документ 1.</param>
        /// <param name="doc2">Документ 2.</param>
        /// <returns>Два вектора одинаковых размеров со значениями TF*IDF.</returns>
        private static Tuple<double[], double[]> MakeVectorsForCompare(IDictionary<string, double> doc1,
            IDictionary<string, double> doc2)
        {
            var vectorsLength = Math.Max(doc1.Count, doc2.Count);
            var dict1 = new Dictionary<string, double>(doc1);
            var vector1 = new double[vectorsLength]; //for doc1
            var vector2 = new double[vectorsLength]; //for doc2
            var iter = 0;
            foreach (var item in dict1)
            {
                if (doc2.ContainsKey(item.Key))
                {
                    vector1[iter] = item.Value;
                    vector2[iter] = doc2[item.Key];
                    doc2.Remove(item.Key);
                    doc1.Remove(item.Key);
                }
                else
                {
                    vector1[iter] = item.Value;
                    vector2[iter] = 0;
                    doc1.Remove(item.Key);
                }
                iter++;
            }

            //if (doc2.Count <= 0) return Tuple.Create(vector1, vector2);

            //foreach (var item in doc2)
            //{
            //    vector1[iter] = 0;
            //    vector2[iter] = item.Value;
            //    iter++;
            //}

            return Tuple.Create(vector1, vector2);
        }

        /// <summary>
        /// Меняет флаги "включения" на кнопках, в зависимости от результатов работы.
        /// </summary>
        private void CheckButtonsEnabled()
        {
            if (M_Model.HandledFiles.Count > 0)
            {
                M_Model.IsPrintResultBtnEnabled = true;
                M_Model.IsClearResultBtnEnabled = true;
                M_Model.IsSaveToFileBtnEnabled = true;
            }
            else
            {
                M_Model.IsPrintResultBtnEnabled = false;
                M_Model.IsClearResultBtnEnabled = false;
                M_Model.IsSaveToFileBtnEnabled = false;
            }

            M_Model.IsStartBtnEnabled = M_Model.UncheckedFiles.Count > 0;
        }

        private DataBase ReinitDbInstance()
        {
            try
            {
                _dbInstance = DataBase.GetInstance(ProgramSettings.GetInstance().ConnectionString,
                    ProgramSettings.GetInstance().DataBaseName,
                    ProgramSettings.GetInstance().Login,
                    ProgramSettings.GetInstance().Password);
                return _dbInstance;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Скорее всего, не удалось подключиться к серверу MongoDb.\n" + e.Message);
                _dbInstance = null;
            }
            return _dbInstance;
        }
        #endregion

        public MainViewModel()
        {
            M_Model = new MainModel();

            ChooseFilesCommand = new DelegateCommand(action => ChooseFiles());
            ExitCommand = new DelegateCommand(action => Exit());
            PrintResultCommand = new DelegateCommand(action => PrintResult());
            SaveToFileCommand = new DelegateCommand(action => SaveToFile());
            ShowAboutAuthorCommand = new DelegateCommand(action => ShowAboutAuthor());
            ShowAboutProgrammCommand = new DelegateCommand(action => ShowAboutProgramm());
            ShowSettingsCommand = new DelegateCommand(action => ShowSettings());
            StartCommand = new DelegateCommand(action => StartProgramm());
            StopCommand = new DelegateCommand(action => StopProgramm());
            ClearResultCommand = new DelegateCommand(action => ClearResult());

            AutoWorkTimer = new DispatcherTimer { Interval = TimeSpan.FromHours(1) };

            try
            {
                _dbInstance = DataBase.GetInstance(ProgramSettings.GetInstance().ConnectionString,
                    ProgramSettings.GetInstance().DataBaseName,
                    ProgramSettings.GetInstance().Login,
                    ProgramSettings.GetInstance().Password);
            }
            catch (Exception e)
            {
                _dbInstance = null;
                Console.Error.WriteLine("Скорее всего, не удалось подключиться к серверу MongoDb.\n" + e.Message);
            }
        }
    }
}