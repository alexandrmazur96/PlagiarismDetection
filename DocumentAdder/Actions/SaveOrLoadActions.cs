using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace DocumentAdder.Actions
{
    internal static class SaveOrLoadActions
    {

        private static readonly JsonSerializer Serializer;
        private static readonly string SettingsPath;

        /// <summary>
        /// Сохраняет настройки приложения, сериализуя их в JSON-файл
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                using (var sw = new StreamWriter(SettingsPath))
                {
                    using (JsonWriter jw = new JsonTextWriter(sw))
                    {
                        Serializer.Serialize(jw, ProgramSettings.GetInstance());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Скорее всего, у программы нету прав для записи. " + ex.Message);
            }
        }

        /// <summary>
        /// Загружает настройки приложения, десериализуя их из JSON-файла. Вызывается в App.xaml.cs
        /// </summary>
        public static void LoadSettings()
        {
            try
            {
                using (var sr = new StreamReader(SettingsPath))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        ProgramSettings.LoadSettings(Serializer.Deserialize<ProgramSettings>(jr));
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine("Скорее всего, это первая инициализация приложения, или файл настроек был удален. Невозможно найти/открыть файл. " 
                    + ex.Message);
            }
        }

        /// <summary>
        /// Инициализируем Newtonsoft.Json сериализатор и путь (строку, string) к настройке приложения.
        /// </summary>
        static SaveOrLoadActions()
        {
            Serializer = new JsonSerializer();
            SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + 
                @"\PlagiarismDetection\" +  "da_settings.json";
        }
    }
}
