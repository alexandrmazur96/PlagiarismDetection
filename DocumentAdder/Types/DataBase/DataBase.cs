using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DocumentAdder.Actions.DocumentAction;
using DocumentAdder.ViewModel;
using DocumentAdder.ViewModel.SettingsViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

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

        /// <summary>
        /// Возвращает/задает логин для подключения к MongoDb.
        /// </summary>
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

        /// <summary>
        /// Возвращает/задает пароль для подключения к MongoDb.
        /// </summary>
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
        /// Возвращает/задает строку подключения к MongoDb.
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
                        ServerSelectionTimeout = TimeSpan.FromSeconds(5)
                    };
                    //инициализируем MongoClient с этими настройками
                    _client = new MongoClient(_clientSettings);
                    //получаем выбранную базу данных
                    var dbName = string.IsNullOrWhiteSpace(DatabaseSettingViewModel.DatabaseModel.DatabaseName) ? "test" : DatabaseSettingViewModel.DatabaseModel.DatabaseName;
                    _database = _client.GetDatabase(dbName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Source + " " + e.Message);
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
                        Server = _mongoUrl.Server,
                        ServerSelectionTimeout = TimeSpan.FromSeconds(5)
                    };
                    _client = new MongoClient(_clientSettings);
                    var dbName = string.IsNullOrWhiteSpace(DatabaseSettingViewModel.DatabaseModel.DatabaseName) ? "test" : DatabaseSettingViewModel.DatabaseModel.DatabaseName;
                    _database = _client.GetDatabase(dbName);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Source + " " + e.Message);
                }
            }

        }        

        /// <summary>
        /// Асинхронно проверяет наличие подключения к MongoDb.
        /// </summary>
        /// <returns>Состояние подключения</returns>
        public async Task<bool> CheckMongoConnection()
        {
            try
            {
                var databases = _client.ListDatabasesAsync().Result;
                await databases.MoveNextAsync();
                return _client.Cluster.Description.State == ClusterState.Connected;
            }
            catch (Exception mce)
            {
                await Console.Error.WriteLineAsync(mce.Message);
                return false;
            }
        }

        /// <summary>
        /// Вставка данных о файле в MongoDB.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="fileExtension">Тип файла.</param>
        private void InsertFileData(string fileName, string filePath, string fileExtension)
        {
            //получаем коллекцию с файлами.
            var fileCollection = _database.GetCollection<FileCollection>("filesCollection");

            var fileData = new FileCollection
            {
                FileName = fileName,
                FilePath = filePath,
                FileType = fileExtension
            };

            fileCollection.InsertOne(fileData);
        }

        /// <summary>
        /// Асинхронная вставка данных о файле в MongoDB.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="fileExtension">Тип файла.</param>
        /// <returns></returns>
        private async Task InsertFileDataAsync(string fileName, string filePath, string fileExtension)
        {
            //получаем коллекцию с файлами.
            var fileCollection = _database.GetCollection<FileCollection>("filesCollection");

            var fileData = new FileCollection
            {
                FileName = fileName,
                FilePath = filePath,
                FileType = fileExtension
            };

            await fileCollection.InsertOneAsync(fileData);
        }

        /// <summary>
        /// Вставка данных о документе в MongoDB.
        /// </summary>
        /// <param name="documentHash">Хеш-сумма документа.</param>
        /// <param name="documentAuthorGroup">Группа автора документа.</param>
        /// <param name="documentTfVector">TF-вектор документа.</param>
        /// <param name="documentAddTime">Дата добавления документа.</param>
        /// <param name="documentTokens">Все очищенные слова документа.</param>
        /// <param name="documentAuthor">Автор документа.</param>
        private void InsertDocumentData(string documentHash, string documentAuthor, string documentAuthorGroup, Dictionary<string, double> documentTfVector, DateTime documentAddTime, string[] documentTokens)
        {
            //Для связывания коллекций нам нужно хранить Id из одной коллекции в другой 
            //как в реляционных СУБД, только без жесткого контроля за этим.
            var fileCollection = _database.GetCollection<FileCollection>("filesCollection");

            //получаем последний добавленый файл (файл в коллекцию вставляется ПЕРЕД вызовом этого метода)
            var filesData = fileCollection.Find(new BsonDocument()).ToList();
            var lastFile = filesData[filesData.Count - 1];

            var documentCollection = _database.GetCollection<DocumentCollection>("documentCollection");

            var documentData = new DocumentCollection
            {
                DocumentHash = documentHash,
                DocumentAddTime = documentAddTime,
                DocumentTfVector = documentTfVector,
                DocumentTokens = documentTokens,
                DocumentAuthor = documentAuthor,
                DocumentAuthorGroup = documentAuthorGroup,
                FileId = lastFile.FileId
            };

            documentCollection.InsertOne(documentData);
        }

        /// <summary>
        /// Асинхронная вставка данных о документе в MongoDB.
        /// </summary>
        /// <param name="documentHash">Хеш-сумма документа.</param>
        /// <param name="documentAuthorGroup">Группа автора документа.</param>
        /// <param name="documentTfVector">TF-вектор документа.</param>
        /// <param name="documentAddTime">Дата добавления документа.</param>
        /// <param name="documentTokens">Все очищенные слова документа.</param>
        /// <param name="documentAuthor">Автор документа.</param>
        /// <returns>Task, для асинхронности.</returns>
        private async Task InsertDocumentDataAsync(string documentHash, string documentAuthor, string documentAuthorGroup, Dictionary<string, double> documentTfVector, DateTime documentAddTime, string[] documentTokens)
        {
            //Для связывания коллекций нам нужно хранить Id из одной коллекции в другой 
            //как в реляционных СУБД, только без жесткого контроля за этим.
            var fileCollection = _database.GetCollection<FileCollection>("filesCollection");
            //получаем последний добавленый файл (файл в коллекцию вставляется ПЕРЕД вызовом этого метода)
            var filesData = await fileCollection.Find(new BsonDocument()).ToListAsync();
            var lastFile = filesData[filesData.Count - 1];

            var documentCollection = _database.GetCollection<DocumentCollection>("documentCollection");

            var documentData = new DocumentCollection
            {
                DocumentHash = documentHash,
                DocumentAddTime = documentAddTime,
                DocumentTfVector = documentTfVector,
                DocumentTokens = documentTokens,
                DocumentAuthor = documentAuthor,
                DocumentAuthorGroup = documentAuthorGroup,
                FileId = lastFile.FileId
            };

            await documentCollection.InsertOneAsync(documentData);
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Вставляет документ в MongoDB. 
        /// </summary>
        /// <param name="document">Документ типа Document.</param>
        public void InsertDocument(Document document)
        {
            if (document != null)
            {
                InsertFileData(document.DocumentName, document.DocumentPath, document.DocumentType);
                InsertDocumentData(document.DocumentHash, document.DocumentAuthor, document.DocumentAuthorGroup,
                    document.DocumentTfVector, document.AddTime, document.DocumentTokens);
                UpdateOrInsertIdfVector(DocumentActions.MakeIdfVector(document.DocumentTfVector));
                LogViewModel.AddNewLog("Документ " + document.DocumentName + " добавлен!", DateTime.Now);
            }
            else
            {
                LogViewModel.AddNewLog("Документ не был передан для вставки!", DateTime.Now, LogType.Error);
            }
        }

        /// <summary>
        /// Асинхронно вставляет документ в MongoDB.
        /// </summary>
        /// <param name="document">Документ типа Document.</param>
        public async Task InsertDocumentAsync(Document document)
        {
            if (document != null)
            {

                await InsertFileDataAsync(document.DocumentName, document.DocumentPath, document.DocumentType);
                await InsertDocumentDataAsync(document.DocumentHash, document.DocumentAuthor, document.DocumentAuthorGroup,
                    document.DocumentTfVector, document.AddTime, document.DocumentTokens);
                await UpdateOrInsertIdfVectorAsync(await DocumentActions.MakeIdfVectorAsync(document.DocumentTfVector));
                LogViewModel.AddNewLog("Документ " + document.DocumentName + " добавлен!", DateTime.Now);
            }
            else
            {
                LogViewModel.AddNewLog("Документ не был передан для вставки!", DateTime.Now, LogType.Error);
            }
        }

        /// <summary>
        /// Асинхронно помещает логи в MongoDB.
        /// </summary>
        /// <param name="logs">Коллекция с логами.</param>
        public async Task InsertLogAsync(List<Log> logs)
        {
            if (await CheckMongoConnection())
            {

                if (logs.Count <= 0)
                {
                    return;
                }

                var logCollection = _database.GetCollection<BsonDocument>("logCollection");

                var bsonLogs = logs.Select(log => new BsonDocument
                {
                    {"Date", log.Date},
                    {"Message", log.Message},
                    {"LogType", log.LogType}
                }).ToList();

                await logCollection.InsertManyAsync(bsonLogs);
            }
            else
            {
                await Console.Error.WriteLineAsync(@"Не удалось вставить лог! Отсутствует подключение к серверу MongoDb!");
                LogViewModel.AddNewLog("Не удалось подключиться к серверу MongoDb! Сохранение логов не производится!", DateTime.Now, LogType.Error);
            }
        }

        /// <summary>
        /// Помещает логи в MongoDB.
        /// </summary>
        /// <param name="logs">Коллекция с логами.</param>
        public void InsertLog(List<Log> logs)
        {
            if (CheckMongoConnection().Result)
            {
                if (logs.Count <= 0)
                {
                    return;
                }
                var logCollection = _database.GetCollection<BsonDocument>("logCollection");

                var bsonLogs = logs.Select(log => new BsonDocument
                {
                    {"Date", log.Date},
                    {"Message", log.Message},
                    {"LogType", log.LogType}
                }).ToList();

                logCollection.InsertMany(bsonLogs);
            }
            else
            {
                Console.Error.WriteLineAsync(@"Не удалось вставить лог! Отсутствует подключение к серверу MongoDb!");
                LogViewModel.AddNewLog("Не удалось подключиться к серверу MongoDb! Сохранение логов не производится!", DateTime.Now, LogType.Error);
            }
        }

        /// <summary>
        /// Асинхронно возвращает данные с определенным условием выборки.
        /// </summary>
        /// <param name="predicateDoc">Предикат поиска для документов.</param>
        /// <param name="predicateFile">Предикат поиска для файлов.</param>
        public async Task<List<Document>> GetAllDocumentsAsync(BsonDocument predicateDoc = null, BsonDocument predicateFile = null)
        {
            if (predicateDoc == null)
            {
                predicateDoc = new BsonDocument();
            }

            if (predicateFile == null)
            {
                predicateFile = new BsonDocument();
            }

            var documentCollection = _database.GetCollection<DocumentCollection>("documentCollection");
            var fileCollection = _database.GetCollection<FileCollection>("filesCollection");

            var documentsData = await documentCollection.Find(predicateDoc).ToListAsync();
            var filesData = await fileCollection.Find(predicateFile).ToListAsync();

            var documentsList = new List<Document>();
            for (int i = 0; i < documentsData.Count; i++)
            {
                documentsList.Insert(i, new Document(
                    filesData[i].FileName,
                    documentsData[i].DocumentAuthor,
                    documentsData[i].DocumentAuthorGroup,
                    filesData[i].FilePath,
                    filesData[i].FileType,
                    documentsData[i].DocumentHash,
                    documentsData[i].DocumentTokens,
                    documentsData[i].DocumentTfVector,
                    documentsData[i].DocumentAddTime));
            }
            return documentsList;
        }

        /// <summary>
        /// Возвращает данные с определенным условием выборки.
        /// </summary>
        /// <param name="predicateDoc">Предикат поиска для документов.</param>
        /// <param name="predicateFile">Предикат поиска для файлов.</param>
        public List<Document> GetAllDocuments(BsonDocument predicateDoc = null, BsonDocument predicateFile = null)
        {
            if (predicateDoc == null)
            {
                predicateDoc = new BsonDocument();
            }

            if (predicateFile == null)
            {
                predicateFile = new BsonDocument();
            }

            var documentCollection = _database.GetCollection<DocumentCollection>("documentCollection");
            var fileCollection = _database.GetCollection<FileCollection>("filesCollection");

            var documentsData = documentCollection.Find(predicateDoc).ToList();
            var filesData = fileCollection.Find(predicateFile).ToList();

            var documentsList = new List<Document>();
            for (int i = 0; i < documentsData.Count; i++)
            {
                documentsList.Insert(i, new Document(
                    filesData[i].FileName,
                    documentsData[i].DocumentAuthor,
                    documentsData[i].DocumentAuthorGroup,
                    filesData[i].FilePath,
                    filesData[i].FileType,
                    documentsData[i].DocumentHash,
                    documentsData[i].DocumentTokens,
                    documentsData[i].DocumentTfVector,
                    documentsData[i].DocumentAddTime));
            }
            return documentsList;
        }


        /// <summary>
        /// Возвращает IDF-вектор из MongoDb.
        /// </summary>
        /// <returns>IDF-вектор.</returns>
        public List<IdfItem> GetIdfVector()
        {
            var idfCollection = _database.GetCollection<IdfItem>("idfVectorCollection");
            return idfCollection.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// Асинхронно возвращает IDF-вектор из MongoDb.
        /// </summary>
        /// <returns>IDF-вектор.</returns>
        public async Task<List<IdfItem>> GetIdfVectorAsync()
        {
            var idfCollection = _database.GetCollection<IdfItem>("idfVectorCollection");
            return await idfCollection.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// Удаляет коллекцию IDF-вектора из MongoDb.
        /// </summary>
        public void UpdateOrInsertIdfVector(List<IdfItem> newItems)
        {
            var idfCollection = _database.GetCollection<IdfItem>("idfVectorCollection");

            foreach (var newItem in newItems)
            {
                var filter = Builders<IdfItem>.Filter.Eq("Token", newItem.Token);
                var updateSet = Builders<IdfItem>.Update.Set("IdfValue", newItem.IdfValue);
                if (idfCollection.Find(filter).Count() != 0)
                {
                    idfCollection.UpdateMany(filter, updateSet);
                }
                else
                {
                    idfCollection.InsertOne(newItem);
                }
            }
        }

        /// <summary>
        /// Асинхронно удаляет коллекцию IDF-вектора из MongoDb.
        /// </summary>
        /// <returns>void</returns>
        public async Task UpdateOrInsertIdfVectorAsync(List<IdfItem> newItems)
        {
            var idfCollection = _database.GetCollection<IdfItem>("idfVectorCollection");

            foreach (var newItem in newItems)
            {
                var filter = Builders<IdfItem>.Filter.Eq("Token", newItem.Token);
                var updateSet = Builders<IdfItem>.Update.Set("IdfValue", newItem.IdfValue);
                if (idfCollection.Find(filter).Any())
                {
                    await idfCollection.UpdateManyAsync(filter, updateSet);
                }
                else
                {
                    await idfCollection.InsertOneAsync(newItem);
                }
            }
        }

        /// <summary>
        /// Вставляет IDF-вектор в MongoDb.
        /// </summary>
        /// <param name="idfVector">IDF-вектор, который нужно вставить.</param>
        public void InsertIdfVector(Dictionary<string, double> idfVector)
        {
            var idfCollection = _database.GetCollection<IdfItem>("idfVectorCollection");
            var idfList = idfVector.Select(idf => new IdfItem
            {
                Token = idf.Key,
                IdfValue = idf.Value,
                IdfId = default(ObjectId)
            }).ToList();
            idfCollection.InsertMany(idfList);
        }

        /// <summary>
        /// Асинхронно вставляет IDF-вектор в MongoDb.
        /// </summary>
        /// <param name="idfVector">IDF-вектор, который нужно вставить.</param>
        /// <returns>void</returns>
        public async Task InsertIdfVectorAsync(Dictionary<string, double> idfVector)
        {
            var idfCollection = _database.GetCollection<IdfItem>("idfVectorCollection");
            var idfList = idfVector.Select(idf => new IdfItem
            {
                Token = idf.Key,
                IdfValue = idf.Value,
                IdfId = default(ObjectId)
            }).ToList();
            await idfCollection.InsertManyAsync(idfList);
        }

        /// <summary>
        /// Получает все логи из MongoDB.
        /// </summary>
        /// <returns>Коллекция с логами приложения за все время.</returns>
        public List<Log> GetAllLogs()
        {
            if (CheckMongoConnection().Result)
            {
                var logCollection = _database.GetCollection<Log>("logCollection");

                var logsFromDb = logCollection.Find(new BsonDocument()).ToList();

                return logsFromDb.ToList();
            }
            LogViewModel.AddNewLog("Не удалось подключиться к серверу MongoDb! Не удалось извлечь логи!", DateTime.Now, LogType.Error);
            return null;
        }

        /// <summary>
        /// Асинхронно получает все логи из MongoDB.
        /// </summary>
        /// <returns>Коллекция с логами приложения за все время.</returns>
        public async Task<List<Log>> GetAllLogsAsync()
        {
            if (await CheckMongoConnection())
            {
                var logCollection = _database.GetCollection<Log>("logCollection");

                var logsFromDb = await logCollection.FindAsync(new BsonDocument());

                return logsFromDb.ToList();
            }
            LogViewModel.AddNewLog("Не удалось подключиться к серверу MongoDb! Не удалось извлечь логи!", DateTime.Now, LogType.Error);
            return null;
        }

        /// <summary>
        /// Смена базы данных MongoDB.
        /// </summary>
        /// <param name="databaseName">Название базы данных в сервере MongoDB.</param>
        public void ChooseDatabase(string databaseName)
        {
            if (CheckMongoConnection().Result)
            {
                _database = _client.GetDatabase(databaseName);
            }
            else
            {
                LogViewModel.AddNewLog("Не удалось подключиться к серверу MongoDb! Не удалось выбрать БД!", DateTime.Now, LogType.Error);                
            }
        }

        /// <summary>
        /// Проверяет, находится ли файл в базе данных по его хеш-сумме.
        /// </summary>
        /// <param name="fileHash">Хеш-сумма файла.</param>
        /// <returns>Логический результат нахождения файла в БД.</returns>
        public bool IsFileInDb(string fileHash)
        {
            var docCollections = _database.GetCollection<BsonDocument>("documentCollection");

            var filter = Builders<BsonDocument>.Filter.Eq("DocumentHash", fileHash);

            return docCollections.Find(filter).Count() > 0;
        }

        /// <summary>
        /// Асинхронно проверяет, находится ли файл в базе данных по его хеш-сумме.
        /// </summary>
        /// <param name="fileHash">Хеш-сумма.</param>
        /// <returns>Логический результат нахождения файла в БД.</returns>
        public async Task<bool> IsFileInDbAsync(string fileHash)
        {
            var docCollections = _database.GetCollection<BsonDocument>("documentCollection");

            var filter = Builders<BsonDocument>.Filter.Eq("DocumentHash", fileHash);

            var cursor = await docCollections.FindAsync(filter);

            return cursor.Any();
        }

        #endregion
        #endregion

        /// <summary>
        /// Создает объект DataBase.
        /// </summary>
        private DataBase()
        {
            _connectionString = DatabaseSettingViewModel.DatabaseModel.ConnectionString;
            _mongoUrl = new MongoUrlBuilder(_connectionString);
            _login = DatabaseSettingViewModel.DatabaseModel.Login ?? "";
            _password = DatabaseSettingViewModel.DatabaseModel.Password ?? "";
            SetClient();
        }

        /// <summary>
        /// Создает объект DataBase с указанными параметрами.
        /// </summary>
        /// <param name="connectionString">Строка подключения к серверу MongoDb.</param>
        /// <param name="dbName">Название базы данных.</param>
        /// <param name="login">Логин для подключения к MongoDb.</param>
        /// <param name="password">Пароль для подключения к MongoDb.</param>
        private DataBase(string connectionString, string dbName, string login = null, string password = null)
        {
            _connectionString = connectionString;
            _mongoUrl = new MongoUrlBuilder(_connectionString);
            _login = login ?? "";
            _password = password ?? "";
            SetClient();           
            ChooseDatabase(dbName);
        }

        private static DataBase _dbInstance;

        /// <summary>
        /// Нужен для реализации паттерна Singleton.
        /// </summary>
        /// <returns>DataBase instance.</returns>
        public static DataBase GetInstance()
        {
            if (_dbInstance != null) return _dbInstance;
            _dbInstance = new DataBase();
            return _dbInstance;
        }

        /// <summary>
        /// Нужен для реализации паттерна Singleton. 
        /// Возвращает объект DataBase с указанными параметрами.
        /// </summary>
        /// <param name="connectionString">Строка подключения к серверу MongoDb.</param>
        /// <param name="dbName">Название базы данных.</param>
        /// <param name="login">Логин для подключения к MongoDb.</param>
        /// <param name="password">Пароль для подключения к MongoDb.</param>
        /// <returns>DataBase instance.</returns>
        public static DataBase GetInstance(string connectionString, string dbName, string login = null, string password = null)
        {
            if (_dbInstance != null) return _dbInstance;
            _dbInstance = new DataBase(connectionString, dbName, login, password);
            return _dbInstance;
        }
    }
}
