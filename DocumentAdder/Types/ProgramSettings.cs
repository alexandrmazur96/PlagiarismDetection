using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Types
{
    [DataContract]
    internal static class ProgramSettings
    {
        #region Properties
        //database settings
        [DataMember]
        internal static string ConnectionString { get; set; }
        
        [DataMember]
        internal static string Login { get; set; }

        [DataMember]
        //надо будет зашифровать!!!
        internal static string Password { get; set; }

        [DataMember]
        internal static string DataBaseName { get; set; }

        //repository settings
        [DataMember]
        internal static string FileTypes { get; set; }

        [DataMember]
        internal static ObservableCollection<RepositoryPath> CollectionsPaths { get; set; }

        #endregion

        static ProgramSettings()
        {
            
        }
    }
}
