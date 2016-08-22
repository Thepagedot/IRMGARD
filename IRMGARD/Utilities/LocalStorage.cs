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
        private static JsonSerializerSettings _JsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public static object Levels { get; private set; }

        public static async Task<Level> LoadLevelAsync(int levelNumber)
        {
            var fileName = "level" + levelNumber + ".json";
            if (levelNumber == -1)
                fileName = "sandbox.json";

            return await LoadFromJsonAsync<Level>(fileName);
        }

        public static async Task<Common> LoadCommonAsync()
        {
            return await LoadFromJsonAsync<Common>("common.json");
        }

        static async Task<T> LoadFromJsonAsync<T>(string fileName)
        {
            using (var stream = AssetHelper.Instance.Open(fileName))
            {
                try
                {
                    // Since last Xamarin update the use of StreamReader for zipped files leads to performance issues
                    byte[] bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    var json = JObject.Parse(System.Text.Encoding.UTF8.GetString(bytes));
                    return JsonConvert.DeserializeObject<T>(json.ToString(), _JsonSettings);
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