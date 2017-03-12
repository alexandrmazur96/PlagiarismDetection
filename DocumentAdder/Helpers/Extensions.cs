using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Helpers
{
    public static class Extensions
    {
        public static bool LocalStorageContains(this System.Collections.ObjectModel.ObservableCollection<Types.RepositoryPath> input, string path)
        {
            foreach (var item in input)
            {
                if (item.StorageType == Types.InternalStorageType.Directory && item.StoragePath == path)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool RemoteStorageContains(this System.Collections.ObjectModel.ObservableCollection<Types.RepositoryPath> input, string path)
        {
            foreach (var item in input)
            {
                if (item.StorageType == Types.InternalStorageType.FTP && item.StoragePath == path)
                {
                    return true;
                }
            }
            return false;
        }
    }
}