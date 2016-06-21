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

            using (var stream = AssetHelper.Instance.Open(fileName))
            using (var reader = new StreamReader(stream))
            {
                try
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var json = JObject.Parse(normalizeContent(jsonContent));
                    return JsonConvert.DeserializeObject<Level>(json.ToString(), _JsonSettings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("JSON reader Exception on reading level {0}", levelNumber);
                    Console.WriteLine("Message: {0}", ex.Message);
                }

                return null;
            }
        }

        public static async Task<Common> LoadCommonAsync()
        {
            var fileName = "common.json";

            using (var stream = AssetHelper.Instance.Open(fileName))
            using (var reader = new StreamReader(stream))
            {
                try
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var json = JObject.Parse(normalizeContent(jsonContent));
                    return JsonConvert.DeserializeObject<Common>(json.ToString(), _JsonSettings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("JSON reader Exception on reading {0}!", fileName);
                    Console.WriteLine("Message: {0}", ex.Message);
                }

                return null;
            }
        }

        private static string normalizeContent(string jsonContent)
        {
            string PKZipHeader = Encoding.UTF8.GetString(new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14 });
            string PKZipHeader2 = Encoding.UTF8.GetString(new byte[] { 0x50, 0x4B, 0x01, 0x02, 0x2D });

            var idx = jsonContent.IndexOf(PKZipHeader);
            idx = (idx == -1) ? jsonContent.IndexOf(PKZipHeader2) : idx;
            return (idx > -1) ? jsonContent.Substring(0, idx) : jsonContent;
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