namespace SizzlingHotProducts.Interfaces
{
    public interface IJsonFileAccess
    {
        List<T> ReadJsonFile<T>(string fileName);
        void WriteJsonFile<T>(string fileName, List<T> data);
    }
}
