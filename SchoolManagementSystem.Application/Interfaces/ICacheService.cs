
namespace SchoolManagementSystem.Application.Interfaces
{
    public interface ICacheService
    {
        public void RemoveSecrets(string key);
        public T GetData<T>(string key);
        public bool SetData<T>(string key, T value);
    }
}
