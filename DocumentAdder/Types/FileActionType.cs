namespace DocumentAdder.Types
{
    public enum FileActionType
    {
        /// <summary>
        /// Показывает, что нужно переместить файл с тем же именем
        /// </summary>
        Move,
        /// <summary>
        /// Показывает, что нужно удалить файл
        /// </summary>
        Delete,
        /// <summary>
        /// Показывает, что нужно переименовать файл и тогда переместить
        /// </summary>
        RenameAndMove
    }
}