using DocumentAdder.Helpers;
using DocumentAdder.Model.SettingsModels;
using DocumentAdder.Types;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DocumentAdder.ViewModel.SettingsViewModels
{
    public class RepositorySettingViewModel
    {
        public static RepositorySettingModel RepositoryModel { get; private set; }
        public static View.AddFTPRepositoryView AddFtpView { get; private set; }

        #region Commands
        public ICommand AddLocalStorageCommand { get; private set; }
        public ICommand ShowFTPStorageCommand { get; private set; }
        #endregion

        #region Methods
        private void addLocalStorage()
        {
            List<string> selectedPaths = null;
            var cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;
            cofd.Multiselect = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                selectedPaths = cofd.FileNames.ToList();
            }

            if (selectedPaths != null)
            {
                foreach (var item in selectedPaths)
                {
                    if (!RepositoryModel.CollectionPaths.LocalStorageContains(item))
                    {
                        RepositoryModel.CollectionPaths.Add(new RepositoryPath(InternalStorageType.Directory, item));
                    }
                }
            }
        }

        public static View.AddFTPRepositoryView getInstance()
        {
            return AddFtpView = new View.AddFTPRepositoryView();
        }

        #endregion

        static RepositorySettingViewModel()
        {
            if (RepositoryModel == null)
            {
                RepositoryModel = new RepositorySettingModel();
            }
        }

        public RepositorySettingViewModel()
        {
            AddLocalStorageCommand = new DelegateCommand(arg => addLocalStorage());
            ShowFTPStorageCommand = new DelegateCommand(arg => getInstance().ShowDialog());
        }
    }
}
