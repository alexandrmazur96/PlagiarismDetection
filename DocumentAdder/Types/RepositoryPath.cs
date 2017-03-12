using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Types
{
    [DataContract]
    public class RepositoryPath
    {
        #region Fields
        private InternalStorageType _storageType;
        private string _storagePath;
        private string _ftpLogin;
        private string _ftpPassword;
        #endregion

        #region Properties

        /// <summary>
        /// Предоставляет информацию о типе хранилища.
        /// <value>StorageType свойство позволяет задать/получить поле типа InternalStorageType (enum), _storageType</value>
        /// </summary>
        [DataMember]
        public InternalStorageType StorageType
        {
            get
            {
                return _storageType;
            }
            set
            {
                _storageType = value; 
            }
        }

        /// <summary>
        /// Предоставляет путь к хранилищу
        /// <value>StoragePath свойство позволяет задать/получить поле типа string, _storagePath</value>
        /// </summary>
        [DataMember]
        public string StoragePath
        {
            get
            {
                return _storagePath;
            }
            set
            {
                _storagePath = value;
            }
        }

        /// <summary>
        /// Предоставляет логин для доступа к ftp-хранилищу. Необязательный параметр
        /// <value>FTPLogin свойство позволяет задать/получить поле типа string, _ftpLogin</value>
        /// </summary>
        [DataMember]
        public string FTPLogin
        {
            get
            {
                return _ftpLogin;
            }
            set
            {
                _ftpLogin = value;                
            }
        }

        /// <summary>
        /// Предоставляет пароль для доступа к ftp-хранилищу. Необязательный параметр
        /// <value>FTPPassword свойство позволяет задать/получить поле типа string, _ftpPassword</value>
        /// </summary>
        [DataMember]
        public string FTPPassword
        {
            get
            {
                return _ftpPassword;
            }
            set
            {
                _ftpPassword = value;
            }
        }
        #endregion

        /// <summary>
        /// Создает путь к репозиторию (внутреннему хранилищу)
        /// </summary>
        /// <param name="type">Обозначает тип внутреннего хранилища (FTP или директория), тип - InternalStorageType (enum)</param>
        /// <param name="storagePath">Обозначает путь к внутреннему хранилищу, тип - string</param>
        /// <param name="ftpLogin">Обозначает логин, для доступа к FTP-хранилищу, тип - string</param>
        /// <param name="ftpPassword">Обозначает пароль, для доступа к FTP-хранилищу, тип - string</param>
        public RepositoryPath(InternalStorageType type, string storagePath, string ftpLogin = null, string ftpPassword = null)
        {
            this._storagePath = storagePath;
            this._storageType = type;

            //Информация о логине и пароле нужна только для доступа к FTP - хранилищу.
            if (type == InternalStorageType.FTP)
            {
                this._ftpLogin = ftpLogin;
                this._ftpPassword = ftpPassword;
            }
            else
            {
                this._ftpLogin = null;
                this._ftpPassword = null;
            }
        }
    }
}
