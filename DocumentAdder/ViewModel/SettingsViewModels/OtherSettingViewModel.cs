using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Model.SettingsModels;

namespace DocumentAdder.ViewModel.SettingsViewModels
{
    public class OtherSettingViewModel
    {
        public OtherSettingModel OtherModel { get; }

        public OtherSettingViewModel()
        {
            OtherModel = new OtherSettingModel();
        }
    }
}
