using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Model
{
    public class AddFTPRepositoryModel : BaseModel
    {
        #region Fields
        private string _ftpPath;
        private string _ftpLogin;
        private string _ftpPassword;
        #endregion

        #region Properties
        /// <summary>
        /// Служит для привязки ftp-пути к хранилищу данных        
        /// </summary>
        public string FTPPath
        {
            get
            {
                return _ftpPath;
            }
            set
            {
                _ftpPath = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Служит для привязки ftp-логина от хранилища данных, которое указано по @FTPPath
        /// </summary>
        public string FTPLogin
        {
            get
            {
                return _ftpLogin;
            }
            set
            {
                _ftpLogin = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Служит для привязки ftp-пароля от хранилища данных, которое указано по @FTPPath
        /// </summary>
        public string FTPPassword
        {
            get
            {
                return _ftpPassword;
            }
            set
            {
                _ftpPassword = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        public AddFTPRepositoryModel()
        {

        }
    }
}
