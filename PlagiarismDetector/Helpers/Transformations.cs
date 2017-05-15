using PlagiarismDetector.Types;

namespace PlagiarismDetector.Helpers
{
    public class Transformations
    {
        /// <summary>
        /// Трансформирует флаги (тип bool) с расширениями (из класса ProgramSettings) в строку, для дальнейшей работы.
        /// </summary>
        /// <returns>Трансформированную строку с расширениями файлов, которые нужно обрабатывать</returns>
        public static string FlagToExtensions()
        {
            string neededFileExtensions = "";
            if (ProgramSettings.GetInstance().IsDoc)
            {
                neededFileExtensions += "*.doc, ";
            }
            if (ProgramSettings.GetInstance().IsDocx)
            {
                neededFileExtensions += "*.docx, ";
            }
            if (ProgramSettings.GetInstance().IsOtd)
            {
                neededFileExtensions += "*.otd, ";
            }
            if (ProgramSettings.GetInstance().IsRtf)
            {
                neededFileExtensions += "*.rtf, ";
            }
            if (ProgramSettings.GetInstance().IsPdf)
            {
                neededFileExtensions += "*.pdf, ";
            }
            if (ProgramSettings.GetInstance().IsTxt)
            {
                neededFileExtensions += "*.txt, ";
            }
            neededFileExtensions = neededFileExtensions.Trim();
            return neededFileExtensions.Substring(0, neededFileExtensions.Length - 1);
        }
    }
}