using System;
using System.Text.RegularExpressions;
using DocumentAdder.Types;

namespace DocumentAdder.Helpers
{
    public static class Verifications
    {
        /// <summary>
        /// Проверяет имя файла на соответствие шаблону [ФИО Студента_Группа_Название работы].
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Логическое значение соответствия (да/нет - true/false).</returns>
        public static bool IsFileNameValid(string fileName)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я -]+_[a-zA-Zа-яА-Я0-9 -]+_([a-zA-Zа-яА-Я0-9– -])+$");
            return regex.IsMatch(fileName);
        }

        /// <summary>
        /// Возвращает тип шрифта (кириллица/латиница/цифры/другой).
        /// </summary>
        /// <param name="word">Слово, которое нужно проверить.</param>
        /// <returns>Тип шрифта.</returns>
        public static FontType GetFontType(string word)
        {
            var hasDigit = new Regex(@"[0-9]+");
            var regexLatin = new Regex(@"[a-zA-Z]+");
            var regexCyrillic = new Regex(@"[а-яА-Я]+");
            if (hasDigit.IsMatch(word)) return FontType.Numbers;
            if (regexLatin.IsMatch(word))
            {
                return FontType.Latin;
            }
            return regexCyrillic.IsMatch(word) ? FontType.Cyrillic : FontType.Other;
        }
    }
}
