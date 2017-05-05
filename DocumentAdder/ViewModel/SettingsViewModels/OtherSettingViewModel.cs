using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DocumentAdder.Helpers;
using DocumentAdder.Model.SettingsModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DocumentAdder.ViewModel.SettingsViewModels
{
    public class OtherSettingViewModel
    {
        public OtherSettingModel OtherModel { get; }

        public ICommand AddMoveDirectoryCommand { get; set; }

        public OtherSettingViewModel()
        {
            OtherModel = new OtherSettingModel();
            AddMoveDirectoryCommand = new DelegateCommand(action => AddMoveDirectory());
        }


        private void AddMoveDirectory()
        {
            var cofd = new CommonOpenFileDialog
            {
                Multiselect = false,
                IsFolderPicker = true
            };

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OtherModel.ReplacePath = cofd.FileName;
            }
        }

    }
}
