using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using IRMGARD.Models;
using System.IO;
using Newtonsoft.Json.Linq;

namespace IRMGARD.Utilities
{
    public static class LocalStorage
    {
        public const string JSONConfigFilesDir = "Config/";

        private static JsonSerializerSettings _JsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public static object Levels { get; private set; }

        public static Level LoadLevel(int levelNumber)
        {
            var fileName = JSONConfigFilesDir + "level" + levelNumber + ".json";
            if (levelNumber == -1)
                fileName = JSONConfigFilesDir + "sandbox.json";      // A single lesson to test separately
            else if (levelNumber == -2)
                fileName = JSONConfigFilesDir + "testlayout.json";   // Lessons to test for different display sizes

            return LoadFromJson<Level>(fileName);
        }

        public static Common LoadCommon()
        {
            return LoadFromJson<Common>(JSONConfigFilesDir + "common.json");
        }

        static T LoadFromJson<T>(string fileName)
        {
            using (var stream = AssetHelper.Instance.Open(fileName))
            {
                try
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var jsonContent = reader.ReadToEnd();
                        var json = JObject.Parse(jsonContent);
                        return JsonConvert.DeserializeObject<T>(json.ToString(), _JsonSettings);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("JSON reader Exception on reading {0}!", fileName);
                    Console.WriteLine("Message: {0}", ex.Message);
                }

                return default(T);
            }
        }

        public static async Task SaveToFileAsync(string fileName, object content)
        {
            var json = JsonConvert.SerializeObject(content, Formatting.Indented, _JsonSettings);

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filename = Path.Combine(path, fileName);

            using (var streamWriter = new StreamWriter(filename, false))
            {
                await streamWriter.WriteLineAsync(json);
            }
        }

        public static async Task<T> LoadFromFileAsync<T>(string fileName)
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filename = Path.Combine(path, fileName);

            try
            {
                using (var streamReader = new StreamReader(filename))
                {
                    var json = await streamReader.ReadToEndAsync();
                    T content = JsonConvert.DeserializeObject<T>(json, _JsonSettings);
                    return content;
                }
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
            catch (JsonException)
            {
                return default(T);
            }
        }
    }
}