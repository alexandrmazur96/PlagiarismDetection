namespace PlagiarismDetector.Types
{
    public class PlagiarismDetectResult
    {
        /// <summary>
        /// Задает/возвращает имя обработанного файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Значение схожести с другими документами для данного файла.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Создает объект результата.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="value">Результат.</param>
        public PlagiarismDetectResult(string fileName, float value)
        {
            FileName = fileName;
            Value = value;
        }
    }
}