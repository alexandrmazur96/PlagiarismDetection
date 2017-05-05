using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DocumentAdder.Types;
using static DocumentAdder.Types.FileActionType;

namespace DocumentAdder.Actions
{
    public static class FileActions
    {

        /// <summary>
        /// Хеширует указанный файл и возвращает хеш-сумму в строковом (string) представлении. 
        /// Используется алгоритм SHA384.
        /// </summary>
        /// <param name="filePath">Путь к файлу, из которого нужно получить хеш-сумму.</param>
        /// <returns>Хеш-сумма указанного файла или null, в случае когда файл не найден.</returns>
        public static string FileHash(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            byte[] fileByte = File.ReadAllBytes(filePath);
            //using sha384 algorythm to hashing the file.
            SHA384 hashObj = new SHA384Managed();
            var fileHash = hashObj.ComputeHash(fileByte);
            return Convert.ToBase64String(fileHash);
        }

        /// <summary>
        /// Создает массив "соли", для хеш-функций.
        /// </summary>
        /// <param name="iterationCount">Количество итераций (соответственно количество элементов массива).</param>
        /// <returns>Возвращает массив байтов "соли".</returns>
        public static byte[] MakeSaltByteArr(int iterationCount)
        {
            var rn = new Random();
            var saltArrBytes = new byte[iterationCount];
            for (int i = 0; i < iterationCount; i++)
            {
                saltArrBytes[i] = (byte)rn.Next(0, 255);
            }
            return saltArrBytes; ;
        }

        /// <summary>
        /// Хеширует указанную строку и возвращает хеш-сумму в строковом (string) представлении.
        /// </summary>
        /// <param name="input">Входящая строка, которую нужно захешировать.</param>
        /// <returns>Возвращает хеш-сумму строки.</returns>
        public static string StringHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var saltBytes = MakeSaltByteArr(128);
            var strWithSaltBytes = new byte[inputBytes.Length + saltBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                strWithSaltBytes[i] = inputBytes[i];
            }
            for (int i = 0; i < saltBytes.Length; i++)
            {
                strWithSaltBytes[inputBytes.Length + i] = saltBytes[i];
            }
            var hashObj = new SHA384Managed();
            var hashBytes = hashObj.ComputeHash(strWithSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Выполняет одно из трех действий с указанным файлом, в зависимости от выбраного действия.
        /// Перемещение, удаление, переиминование и перемещение.
        /// </summary>
        /// <param name="filePath">Путь к файлу, с которым нужно выполнить действие.</param>
        /// <param name="action">Возможное действие с файлом. Объект enum-типа FileActionType.</param>
        public static string FileMoveOrDelete(string filePath, FileActionType action)
        {
            switch (action)
            {
                case Delete:
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        return null;
                    }
                    return null;
                case Move:
                    if (File.Exists(filePath))
                    {
                        var newFilePath = ProgramSettings.GetInstance().ReplacePath + Path.GetFileName(filePath);
                        File.Move(filePath, newFilePath);
                        File.Delete(filePath);
                        return newFilePath;
                    }
                    return null;
                case RenameAndMove:
                    if (File.Exists(filePath))
                    {
                        var newFilePath = MakeNewFilePath(filePath);
                        File.Move(filePath, newFilePath);
                        File.Delete(filePath);
                        return newFilePath;
                    }
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        /// <summary>
        /// Создает новый путь к файлу. 
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Строку с новым путем файла. Имя файла - результат хеш-функции SHA-384.</returns>
        private static string MakeNewFilePath(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string newFilePath = ProgramSettings.GetInstance().ReplacePath + @"\";
            string newFileName = StringHash(fileName).Replace('\\', '_');
            newFileName = newFileName.Replace('/', '_');
            return newFilePath + newFileName + fileExtension;
        }
    }
}