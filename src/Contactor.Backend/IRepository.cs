namespace Contactor.Backend;

public interface IRepository<T>
{
    void Create(T model);

    IEnumerable<T> ReadAll();

    T Read(string id);

    void Update(string id, T model);

    void Remove(string id);
}
