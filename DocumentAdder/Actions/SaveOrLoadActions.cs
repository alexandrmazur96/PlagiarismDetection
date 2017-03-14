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
        public static void SaveSettings()
        {
            using (StreamWriter sw = new StreamWriter(SettingsPath))
            {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    Serializer.Serialize(jw, ProgramSettings.GetInstance());
                }
            }
        }

        public static void LoadSettings()
        {
            using (StreamReader sr = new StreamReader(SettingsPath))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    ProgramSettings.LoadSettings(Serializer.Deserialize<ProgramSettings>(jr));
                }
            }
        }

        static SaveOrLoadActions()
        {
            Serializer = new JsonSerializer();
            SettingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            Console.WriteLine(SettingsPath);
        }
    }
}
