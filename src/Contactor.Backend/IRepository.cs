namespace Contactor.Backend;

public interface IRepository<T>
{
    bool Create(T model);

    IEnumerable<T> ReadAll();

    T? Read(Guid id);

    bool Update(Guid id, T model);

    bool Remove(Guid id);
}
