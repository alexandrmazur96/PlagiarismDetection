using PlagiarismDetector.Model;

namespace PlagiarismDetector.ViewModel
{
    public class SettingsViewModel
    {
        public SettingsModel SettingModel { get; set; }

        public SettingsViewModel()
        {
            SettingModel = new SettingsModel();
        }
    }
}