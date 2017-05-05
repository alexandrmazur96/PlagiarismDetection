namespace DocumentAdder.Types
{
    public enum LogType
    {
        /// <summary>
        /// Информативное сообщение (будет выделено желтым цветом)
        /// </summary>
        Information,
        /// <summary>
        /// Сообщает об ошибке (будет выделено красным цветом)
        /// </summary>
        Error,
        /// <summary>
        /// Обычное сообщение (будет выделено синим или черным)
        /// </summary>
        Message
    }
}