using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Types
{
    public enum InternalStorageType
    {
        /// <summary>
        /// Тип хранилища - локальная директория
        /// </summary>
        Directory,

        /// <summary>
        /// Тип хранилища - удаленный FTP
        /// </summary>
        FTP
    }
}
