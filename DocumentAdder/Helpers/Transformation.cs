﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Types;

namespace DocumentAdder.Helpers
{
    public static class Transformation
    {
        public static string TransformFileFilter(string extStr)
        {
            string result = "";

            var arrExtension = extStr.Split(',');

            return result;
        }

        /// <summary>
        /// Трансформирует флаги (тип bool) с расширениями (из класса ProgramSettings) в строку, для дальнейшей работы.
        /// </summary>
        /// <returns>Трансформированную строку с расширениями файлов, которые нужно обрабатывать</returns>
        public static string FlagToExtensions()
        {
            //private string _fileTypes = "*.txt, *.doc, *.docx, *.rtf, *.otd, *.pdf";

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
