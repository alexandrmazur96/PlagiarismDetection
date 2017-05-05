using System;
using DocumentAdder.ViewModel;

namespace DocumentAdder.Types.Exceptions
{
    public class FileNameFormatException : Exception
    {
        public FileNameFormatException() : base()
        {
            //LogViewModel.AddNewLog("Неправильный формат файла!", DateTime.Now, LogType.Error);
        }

        public FileNameFormatException(string message) : base(message)
        {
            //LogViewModel.AddNewLog("Неправильный формат файла!", DateTime.Now, LogType.Error);
        }

        public FileNameFormatException(string message, System.Exception inner) : base(message, inner)
        {
            //LogViewModel.AddNewLog("Неправильный формат файла!", DateTime.Now, LogType.Error);
        }

    }
}