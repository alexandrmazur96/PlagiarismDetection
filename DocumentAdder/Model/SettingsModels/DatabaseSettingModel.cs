using DocumentAdder.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Model.SettingsModels
{
    [DataContract]
    public class DatabaseSettingModel : BaseModel
    {
        #region Fields
        private string _connectionString;
        private string _login;
        private string _password;
        private string _databaseName;
        #endregion

        #region Properties
        /// <summary>
        /// Строка подключения к базе данных MongoDB.
        /// <value>ConnectionString свойство задает/возвращает значения типа string, поля _connectionString.</value>
        /// </summary>
        [DataMember]
        public string ConnectionString
        {
            get
            {
                //return _connectionString;
                return ProgramSettings.ConnectionString;
            }
            set
            {
                //_connectionString = value;
                ProgramSettings.ConnectionString = value;
                NotifyPropertyChanged();                
            }
        }

        /// <summary>
        /// Логин от базы данных MongoDB
        /// <value>Login свойство задает/возвращает значение типа string, поля _login</value>
        /// </summary>
        [DataMember]
        public string Login
        {
            get
            {
                //return _login;
                return ProgramSettings.Login;
            }
            set
            {
                //_login = value;
                ProgramSettings.Login = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Пароль от базы данных MongoDB
        /// <value>Password свойство задает/возвращает значения типа SecureString, поля _password</value>
        /// </summary>
        [DataMember]
        public string Password
        {
            get
            {
                //return _password;
                return ProgramSettings.Password;
            }
            set
            {
                //_password = value;
                ProgramSettings.Password = value;
                NotifyPropertyChanged();
            }
        }        

        /// <summary>
        /// Названия базы данных, которая участвует в работе.
        /// <value>DatabaseName свойство задает/возвращает значения типа string, поля _databaseName</value>
        /// </summary>
        [DataMember]
        public string DatabaseName
        {
            get
            {
                //return _databaseName;
                return ProgramSettings.DataBaseName;
            }
            set
            {
                //_databaseName = value;
                ProgramSettings.DataBaseName = value;
                NotifyPropertyChanged();
            }
        }

        public DatabaseSettingModel()
        {
        }
        #endregion
    }
}
