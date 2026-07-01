
namespace partycli.Interfaces
{
    public interface INordDataStoreService
    {
        void StoreValue(string name, string value, bool writeToConsole = true);
        void Log(string action);
    }
}
