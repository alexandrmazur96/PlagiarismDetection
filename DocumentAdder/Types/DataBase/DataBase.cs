using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::DocumentAdder.ViewModel.SettingsViewModels;
using System.Security.Cryptography;
using MongoDB.Driver;

namespace DocumentAdder.Types.DataBase
{
    public class DataBase
    {
        #region Fields
        private string _connectionString;
        private string _login;
        private string _password;

        //private fields for class working        
        private MongoUrlBuilder _mongoUrl;        
        private MongoClientSettings _clientSettings;
        private MongoClient _client;
        private IMongoDatabase _database;
        #endregion

        #region Properties

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                SetClient();
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                SetClient();
            }
        }

        /// <summary>
        /// Возвращает/задает строку подключения к MongoDB
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                _mongoUrl = new MongoUrlBuilder(_connectionString);
                SetClient();
            }
        }
        #endregion

        #region Methods

        #region Private methods

        /// <summary>
        /// Устанавливает новый объект MongoClient, 
        /// при переустановке новой строки подключения, логина, пароля или при начальной инициализации объекта
        /// </summary>
        private void SetClient()
        {
            //если поле логина пустое
            if (string.IsNullOrWhiteSpace(_login))
            {
                try
                {
                    //создаем настройки только со строкой сервера
                    _clientSettings = new MongoClientSettings
                    {                    
                        Server = _mongoUrl.Server,                        
                    };

                    //инициализируем MongoClient с этими настройками
                    _client = new MongoClient(_clientSettings);

                    //получаем выбранную базу данных
                    _database = _client.GetDatabase(DatabaseSettingViewModel.DatabaseModel.DatabaseName);

                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Source + " " + e.Message);
                }
            }
            else
            {
                try
                {
                    _clientSettings = new MongoClientSettings
                    {
                        Credentials = new[]
                        {
                        MongoCredential.CreateCredential(DatabaseSettingViewModel.DatabaseModel.DatabaseName,
                        _login, _password)
                    },
                        Server = _mongoUrl.Server
                    };

                    _client = new MongoClient(_clientSettings);

                    _database = _client.GetDatabase(DatabaseSettingViewModel.DatabaseModel.DatabaseName);

                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Source + " " + e.Message);
                }
            }    
                    
        }
        
        #endregion

        #region Public methods

        public void InsertDocument()
        {
            //gets bytes data from file
            byte[] fileByte = new byte[256];

            //using sha384 algorythm to hashing the file and insert this into db
            SHA384 hashObj = new SHA384Managed();
            byte[] fileHash = hashObj.ComputeHash(fileByte);
        }

        /// <summary>
        /// Асинхронно получает данные с определенным условием выборки
        /// </summary>
        /// <param name="_predicate">Услови для выборки, необязательный параметр</param>
        public void GetDataAsync(string _predicate = null)
        {
            
        }

        /// <summary>
        /// Получает данные с определенным условием выборки
        /// </summary>
        /// <param name="_predicate">Услови для выборки, необязательный параметр</param>
        public void GetData(string _predicate = null)
        {

        }

        public void ChooseDatabase(string _databaseName)
        {
            _database = _client.GetDatabase(_databaseName);
        }

        #endregion
        #endregion

        /// <summary>
        /// Создает объект datebase.
        /// </summary>
        public DataBase()
        {
            _connectionString = DatabaseSettingViewModel.DatabaseModel.ConnectionString;
            _mongoUrl = new MongoUrlBuilder(_connectionString);
            _login = DatabaseSettingViewModel.DatabaseModel.Login;
            _password = DatabaseSettingViewModel.DatabaseModel.Password ?? "";
            SetClient();
        }
    }
}
