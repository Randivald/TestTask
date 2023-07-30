namespace NASAData_API.Repositories
{
    public interface IRepository<T>
    {
        T[] GetAll();
        void InsertRange(T[] entities);       
        void DeleteRange(T[] entities);
        void DeleteAll();
    }
}