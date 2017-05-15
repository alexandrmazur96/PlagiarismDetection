using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DocumentAdder.Types
{
    [DataContract]
    public class Document
    {
        public struct FTPFiles
        {
            uint filesCount;
            List<string> filesNames;

            public FTPFiles(uint filesCount, List<string> filesNames)
            {
                this.filesCount = filesCount;
                this.filesNames = filesNames;
            }
        }

        #region Fields
        //data for serialize in json format
        [DataMember]
        private string _documentName;
        [DataMember]
        private string _documentPath;
        [DataMember]
        private string _documentType;
        [DataMember]
        private string _documentHash;
        [DataMember]
        private string _documentAuthor;
        [DataMember]
        private string _documentAuthorGroup;
        [DataMember]
        private string[] _documentTokens;
        [DataMember]
        private Dictionary<string, double> _documentTfVector;
        [DataMember]
        private DateTime _addTime;
        #endregion

        #region Properties

        /// <summary>
        /// Возвращает имя документа
        /// <value>DocumentName свойство возвращает значение типа string поля, _documentName</value>
        /// </summary>
        public string DocumentName
        {
            get
            {
                return _documentName;
            }
            private set
            {
                _documentName = value;
            }
        }

        /// <summary>
        /// Возвращает автора документа
        /// </summary>
        public string DocumentAuthor
        {
            get { return _documentAuthor; }
            set { _documentAuthor = value; }
        }

        /// <summary>
        /// Возвращает группу автора документа
        /// </summary>
        public string DocumentAuthorGroup
        {
            get
            {
                return _documentAuthorGroup;
            }
            set { _documentAuthorGroup = value; }
        }

        /// <summary>
        /// Возвращает путь на диске/ftp к документу
        /// <value>DocumentPath свойство возвращает значения типа string поля, _documentPath</value>
        /// </summary>
        public string DocumentPath
        {
            get
            {
                return _documentPath;
            }
            private set
            {
                _documentPath = value;
            }
        }

        /// <summary>
        /// Возвращает тип документа (*.doc, *.docx, *.rtf, *.txt, etc)
        /// <value>DocumentType свойство возвращает значения типа string поля, _documentType</value>
        /// </summary>
        public string DocumentType
        {
            get
            {
                return _documentType;
            }
            private set
            {
                _documentType = value;
            }
        }

        /// <summary>
        /// Возвращает канонизированный текст документа.
        /// </summary>
        public string[] DocumentTokens
        {
            get { return _documentTokens; }
            set { _documentTokens = value; }
        }

        /// <summary>
        /// Вектор документа 
        /// </summary>
        public Dictionary<string, double> DocumentTfVector
        {
            get
            {
                return _documentTfVector;
            }
            set
            {
                _documentTfVector = value;
            }
        }

        /// <summary>
        /// Возвращает или задает отпечаток файла документа (его Hash-сумму, используется SHA384 хэш-функция)
        /// </summary>
        public string DocumentHash
        {
            get
            {
                return _documentHash;
            }
            set
            {
                _documentHash = value;
            }
        }

        /// <summary>
        /// Возвращает дату добавления во внутреннюю файловую структуру
        /// <value>AddTime свойство возвращает типа DateTime поля, _addTime</value>
        /// </summary>
        public DateTime AddTime
        {
            get
            {
                return _addTime;
            }
            set
            {
                _addTime = value;
            }
        }
        #endregion

        /// <summary>
        /// Создает новый объект, который преобретает необходимый вид для адекватной работы с документом
        /// </summary>
        /// <param name="documentName">Имя документа.</param>
        /// <param name="documentAuthor">Автор документа.</param>
        /// <param name="documentAuthorGroup">Группа автора документа.</param>
        /// <param name="documentPath">Путь к документу.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documentHash">Хэш сумма файла документа.</param>
        /// <param name="documentTokens">Все слова документа.</param>
        /// <param name="documentTFVector">Вектор TF-значений для слов документа.</param>
        /// <param name="addTime">Дата добавления файла во внутреннее хранилище.</param>
        public Document(string documentName, string documentAuthor, string documentAuthorGroup, string documentPath, string documentType, string documentHash, string[] documentTokens, Dictionary<string, double> documentTFVector, DateTime addTime)
        {
            this.DocumentName = documentName;
            this.DocumentAuthor = documentAuthor;
            this.DocumentAuthorGroup = documentAuthorGroup;
            this.DocumentPath = documentPath;
            this.DocumentType = documentType;
            this.DocumentHash = documentHash;
            this.DocumentTokens = documentTokens;
            this.DocumentTfVector = documentTFVector;
            this.AddTime = addTime;
        }
    }
}
