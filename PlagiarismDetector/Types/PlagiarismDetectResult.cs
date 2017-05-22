using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentAdder.Types;

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
        public double Value { get; set; }

        /// <summary>
        /// Информация о похожих документах.
        /// </summary>
        public ObservableCollection<PlagiarismDetectExpandedResult> SimilarityDocuments { get; set; }

        /// <summary>
        /// Создает объект результата.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="value">Результат.</param>
        /// <param name="similarityDocuments">Похожие документы.</param>
        public PlagiarismDetectResult(string fileName, double value, IEnumerable<PlagiarismDetectExpandedResult> similarityDocuments = null)
        {
            FileName = fileName;
            Value = value;
            if (similarityDocuments != null) SimilarityDocuments = new ObservableCollection<PlagiarismDetectExpandedResult>(similarityDocuments);
        }
    }
}