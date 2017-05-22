using System;

namespace PlagiarismDetector.Types
{
    public class PlagiarismDetectExpandedResult
    {
        private double _resultPercent;

        /// <summary>
        /// Путь к файлу, у которого обнаружено сходство.
        /// </summary>
        public string ResultFilePath { get; set; }

        /// <summary>
        /// Название файла, у которого обнаружено сходство.
        /// </summary>
        public string ResultFileName { get; set; }

        /// <summary>
        /// Результат в процентном виде.
        /// </summary>
        public double ResultPercent
        {
            get
            {
                return _resultPercent * 100;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                _resultPercent = value;
            }
        }

        public PlagiarismDetectExpandedResult(string resultFilePath, string resultFileName, double resultPercent)
        {
            ResultFileName = resultFileName;
            ResultFilePath = resultFilePath;
            ResultPercent = resultPercent;
        }
    }
}