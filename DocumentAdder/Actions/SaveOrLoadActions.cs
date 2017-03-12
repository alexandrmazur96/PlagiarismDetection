using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace DocumentAdder.Actions
{
    internal static class SaveOrLoadActions
    {
        public static void SaveSettings()
        {
            var serializer = new JsonSerializer();

            using (var fs = new FileStream("settings.json", FileMode.OpenOrCreate))
            {
                var jsonWriter = new BsonWriter(fs);
                serializer.Serialize(jsonWriter, settings);
            }
        }

        public static void LoadSettings()
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Model.MainSettingModel));

            using (FileStream fs = new FileStream("settings.json", FileMode.Open))
            {
                ViewModel.MainViewModel.SettingModel = (Model.MainSettingModel)jsonFormatter.ReadObject(fs);
            }
        }
    }
}
