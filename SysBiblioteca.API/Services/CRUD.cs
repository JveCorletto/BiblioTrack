namespace SysBiblioteca.API.Services
{
    public interface CRUD<T>
    {
        void Create(T entity);
        List<T> Read();
        T getById(long? id);
        void Update(T entity);
        void Delete(T entity);
    }
}