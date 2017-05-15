using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlagiarismDetector.Types;

namespace PlagiarismDetector.Model
{
    public class BaseModel : INotifyPropertyChanged
    {
        protected static ProgramSettings ProgramSettings;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseModel()
        {
            ProgramSettings = ProgramSettings.GetInstance();
        }
    }
}
