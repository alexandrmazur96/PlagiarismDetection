using DocumentAdder.Model.SettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.ViewModel.SettingsViewModels
{
    public class DatabaseSettingViewModel
    {
        public static DatabaseSettingModel DatabaseModel { get; private set; }

        static DatabaseSettingViewModel()
        {
            if(DatabaseModel == null)
            {
                DatabaseModel = new DatabaseSettingModel();
            }
        }

        public DatabaseSettingViewModel()
        {
        }
    }
}
