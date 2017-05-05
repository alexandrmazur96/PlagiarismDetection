using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DocumentAdder.Helpers;
using DocumentAdder.ViewModel.SettingsViewModels;

namespace DocumentAdder.ViewModel
{
    public class AddFTPRepositoryViewModel
    {
        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        #endregion

        public DocumentAdder.Model.AddFTPRepositoryModel FTPRepositoryModel { get; private set; }

        public AddFTPRepositoryViewModel()
        {
            FTPRepositoryModel = new Model.AddFTPRepositoryModel();

            CancelCommand = new Helpers.DelegateCommand(arg => RepositorySettingViewModel.AddFtpView.Close());
            AddCommand = new Helpers.DelegateCommand(arg => AddRepository());
        }

        //Вроде работает! (на самом деле нет, а впрочем...хер с ним)
        private void AddRepository()
        {
            if(FTPRepositoryModel.FTPPath == null)
            {
                FTPRepositoryModel.FTPPath = "";
            }

            try
            {
                //если вначале строки по каким-то причинам не стоит "ftp://", то добавляем его.
                if (!FTPRepositoryModel.FTPPath.Substring(0, 6).Equals("ftp://"))
                {
                    FTPRepositoryModel.FTPPath = FTPRepositoryModel.FTPPath.Insert(0, "ftp://");
                    System.Windows.MessageBox.Show(FTPRepositoryModel.FTPPath);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                FTPRepositoryModel.FTPPath = FTPRepositoryModel.FTPPath.Insert(0, "ftp://");
                Console.WriteLine(ex.Message);
            }

            try
            {
                //Проверяем на валидность ftp-путь, если путь валидный
                if (Verifications.IsFtpValid(FTPRepositoryModel.FTPPath))
                {
                    //и такого нету в коллекции путей
                    if (!RepositorySettingViewModel.RepositoryModel.CollectionPaths.RemoteStorageContains(FTPRepositoryModel.FTPPath))
                    {
                        //тогда добавляем его
                        RepositorySettingViewModel.RepositoryModel.CollectionPaths.Add(
                            new Types.RepositoryPath(Types.InternalStorageType.FTP, FTPRepositoryModel.FTPPath,
                            FTPRepositoryModel.FTPLogin, FTPRepositoryModel.FTPPassword));
                        //после добавления - очищаем введенные поля, дабы потом использовать вновь, если надо
                        FTPRepositoryModel.FTPPath = "";
                        FTPRepositoryModel.FTPLogin = "";
                        FTPRepositoryModel.FTPPassword = "";
                        //и "закрываем" окно
                        RepositorySettingViewModel.AddFtpView.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                //иначе пишем, что путь неправильный
                else
                {
                    System.Windows.MessageBox.Show("FTP-путь к хранилищу неправильный! \nПопробуйте еще раз!", "Ошибка");
                }
            }
            catch (UriFormatException ex)
            {
                System.Windows.MessageBox.Show("FTP-путь к хранилищу неправильный! \nПопробуйте еще раз!", "Ошибка");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
