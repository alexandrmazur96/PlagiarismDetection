using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using DocumentAdder.Helpers;
using DocumentAdder.Model;
using DocumentAdder.Types;
using DocumentAdder.Types.DataBase;
using MongoDB.Bson;
using Clipboard = System.Windows.Clipboard;

namespace DocumentAdder.ViewModel
{
    public class LogViewModel
    {
        private static int _logInRam;
        private static int _logAddingIndex;
        private static int _logLastPush;

        public LogViewModel()
        {
            LogModelInstance = new LogModel();
            SaveToClipboardCommand = new DelegateCommand(action => SaveToClipboard());
            SaveToFileCommand = new DelegateCommand(action => SaveToFile());
            SaveAllToFileCommand = new DelegateCommand(action => SaveAllToFile());
            LogClearCommand = new DelegateCommand(action => LogClear());
        }

        #region Commands

        public ICommand SaveToClipboardCommand { get; set; }
        public ICommand SaveToFileCommand { get; set; }
        public ICommand SaveAllToFileCommand { get; set; }
        public ICommand LogClearCommand { get; set; }

        #endregion

        public static LogModel LogModelInstance { get; set; }

        #region Methods

        /// <summary>
        /// Сохраняет логи за текущую сессию в буффер обмена
        /// </summary>
        private void SaveToClipboard()
        {
            var textToClipboard = new StringBuilder();
            foreach (var log in LogModelInstance.LogMessages)
            {
                textToClipboard.Append(log.Date.ToUniversalTime() + " " + log.Message + Environment.NewLine);
            }
            Clipboard.SetText(textToClipboard.ToString());
        }

        /// <summary>
        /// Сохраняет логи за текущую сессию в выбранный файл
        /// </summary>
        private void SaveToFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                ShowReadOnly = false,
                Filter = @"Text File (*.txt)|*.txt",
                Title = @"Выберите файл, куда необходимо сохранить логи"
            };

            StreamWriter writer = null;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                writer = new StreamWriter(openFileDialog.FileName);
            }

            if (writer != null)
            {
                var textToFile = new StringBuilder();
                foreach (var log in LogModelInstance.LogMessages)
                {
                    textToFile.Append(log.Date.ToUniversalTime() + " " + log.Message + Environment.NewLine);
                }
                writer.Write(textToFile.ToString());
                writer.Close();
            }
        }

        /// <summary>
        /// Сохраняет все логи из базы данных в выбранный файл
        /// </summary>
        private void SaveAllToFile()
        {

            if (LogModelInstance.LogMessages.Count != 0)
            {
                PushLogToDb();
            }

            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                ShowReadOnly = false,
                Filter = @"Text File (*.txt)|*.txt",
                Title = @"Выберите файл, куда необходимо сохранить логи"
            };

            StreamWriter writer = null;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                writer = new StreamWriter(openFileDialog.FileName);
            }

            if (writer != null)
            {
                var textToFile = new StringBuilder();
                foreach (var log in DataBase.GetInstance().GetAllLogs())
                {
                    textToFile.Append(log.Message + " " + log.Date + Environment.NewLine);
                }
                writer.Write(textToFile.ToString());
                writer.Close();
            }
        }

        /// <summary>
        /// Помещает логи за эту сессию в БД, а потом очищает текущие логи
        /// </summary>
        private void LogClear()
        {
            if (LogModelInstance.LogMessages.Count == 0) return;
            PushLogToDb();
            _logAddingIndex = 0;
            _logLastPush = 0;
            LogModelInstance.LogMessages.Clear();
        }

        /// <summary>
        /// Помещает логи за текущую сессию в MongoDb.
        /// </summary>
        public static void PushLogToDb()
        {
            var neededLogs = new List<Log>();
            for (var i = _logLastPush; i < LogModelInstance.LogMessages.Count; i++)
            {
                neededLogs.Add(LogModelInstance.LogMessages[i]);
            }
            _logLastPush = _logAddingIndex;
            DataBase.GetInstance().InsertLog(neededLogs);
        }

        /// <summary>
        /// Асинхронно помещает логи за текущую сессию в MongoDb.
        /// </summary>
        /// <returns></returns>
        public static async Task PushLogToDbAsync()
        {
            var dbInstance = DataBase.GetInstance();
            var neededLogs = new List<Log>();
            for (var i = _logAddingIndex; i < LogModelInstance.LogMessages.Count; i++)
            {
                neededLogs.Add(LogModelInstance.LogMessages[i]);
            }
            await dbInstance.InsertLogAsync(neededLogs);
        }

        /// <summary>
        /// Добавляет указанный лог в текущую сессию. Если добавленных логов > 30, помещает все в БД.
        /// </summary>
        /// <param name="message">Сообщение лога.</param>
        /// <param name="date">Дата добавления</param>
        /// <param name="logType">Тип сообщения (ошибка, информация, просто сообщение).</param>
        public static void AddNewLog(string message, DateTime date, LogType logType = LogType.Message)
        {
            LogModelInstance.LogMessages.Insert(_logAddingIndex, new Log(message, date, logType));
            _logInRam++;
            _logAddingIndex++;
            if (_logInRam >= 30)
            {
                PushLogToDb();
                _logInRam = 0;
            }
        }

        #endregion
    }
}