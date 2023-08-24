namespace SysBiblioteca.API.Services
{
    public interface CRUD<T>
    {
        void Create(T entity);
        List<T> Read();
        void Update(T entity);
        void Delete(T entity);
    }
}