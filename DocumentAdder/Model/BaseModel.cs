using DocumentAdder.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Types;

namespace DocumentAdder.Model
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
