namespace DocumentAdder.Types
{
    /// <summary>
    /// Тип шрифта - кириллический или латиница.
    /// </summary>
    public enum FontType
    {
        /// <summary>
        /// Представляет буквы кириллического алфавита.
        /// </summary>
        Cyrillic,
        /// <summary>
        /// Представляет буквы латинского алфавита.
        /// </summary>
        Latin,
        /// <summary>
        /// Представляет буквы других алфавитов, которые не подходя для стемминга.
        /// </summary>
        Other,
        /// <summary>
        /// Слово содержит цифры.
        /// </summary>
        Numbers
    }
}