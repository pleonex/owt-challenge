namespace Contactor.Backend;

public interface IRepository<T>
{
    Task<int> Create(T model);

    IEnumerable<T> ReadAll();

    Task<T?> Read(int id);

    Task<bool> Update(int id, T model);

    Task<bool> Remove(int id);
}
