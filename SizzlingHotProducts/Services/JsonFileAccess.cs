using Newtonsoft.Json;
using SizzlingHotProducts.Interfaces;

namespace SizzlingHotProducts.Services
{
    public class JsonFileAccess : IJsonFileAccess
    {
        private readonly string _folderPath;

        public JsonFileAccess(string folderPath)
        {
            _folderPath = folderPath;
            Directory.CreateDirectory(folderPath);
        }

        public List<T> ReadJsonFile<T>(string fileName)
        {
            var filePath = Path.Combine(_folderPath, fileName);
            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(jsonContent);
            }
            return new List<T>();
        }

        public void WriteJsonFile<T>(string fileName, List<T> data)
        {
            var filePath = Path.Combine(_folderPath, fileName);
            var jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
        }
    }
}
