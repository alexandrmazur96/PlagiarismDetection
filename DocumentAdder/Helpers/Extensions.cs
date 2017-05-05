using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Types;

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

        /// <summary>
        /// Удаляет все элементы, удовлетворяющие условию predicate
        /// </summary>
        /// <typeparam name="T">Generic тип в ObservableCollection</typeparam>
        /// <param name="coll">Коллекция, к которой вызывается данный extension метод</param>
        /// <param name="predicate">Условия удаления</param>
        /// <returns>Количесво удаленных элементов</returns>
        public static int RemoveAll<T>(this ObservableCollection<T> coll, Func<T, bool> predicate)
        {
            var itemsToRemove = coll.Where(predicate).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        /// <summary>
        /// Заменяет искомый элемент в коллекции на указанный.
        /// </summary>
        /// <typeparam name="T">Любой c# тип (Generics)</typeparam>
        /// <param name="list">Индексатор, позволяющий вызвать этот метод прямо на объекте List'а</param>
        /// <param name="neededItem">Искомый элемент, который нужно заменить.</param>
        /// <param name="replaceItem">Элемент, на который нужно заменить.</param>
        /// <returns></returns>
        public static List<T> ItemReplace<T>(this List<T> list, T neededItem, T replaceItem)
        {
            var tmpList = new List<T>(list);
            int i = 0;
            foreach (var listItem in list)
            {
                if (listItem.Equals(neededItem))
                {
                    tmpList[i] = replaceItem;
                }
                i++;
            }
            return tmpList;
        }

        /// <summary>
        /// Выводит коллекцию List с указанным заголовком @listName
        /// </summary>
        /// <typeparam name="T">Generic type (любой тип C#)</typeparam>
        /// <param name="list">Индексатор, позволяющий вызвать этот метод прямо на объекте List'а</param>
        /// <param name="listName">Название заголовка, необязательный параметр.</param>
        public static void ConsolePrintList<T>(this List<T> list, string listName = "LIST:")
        {
            Console.WriteLine("----------" + listName + "----------");
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }

        /// <summary>
        /// Показывает, находится ли слово в коллекции List_IdfItem.
        /// </summary>
        /// <param name="list">Индексатор, позволяющий вызвать этот метод прямо на объекте List'a.</param>
        /// <param name="token">Искомое слово.</param>
        /// <returns>Результат bool нахождения слова в коллекции.</returns>
        public static bool ContainsIdf(this List<IdfItem> list, string token)
        {
            return list.Any(idfItem => idfItem.Token.Equals(token));
        }
    }
}