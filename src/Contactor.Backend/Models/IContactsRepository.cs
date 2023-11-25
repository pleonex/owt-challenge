namespace Contactor.Backend.Models;

public interface IContactsRepository
{
    Task<int> Create(ContactDto dto);

    Task<IEnumerable<ContactDto>> GetAll();

    Task<ContactDto?> GetById(int id);

    Task<bool> UpdateById(ContactDto dto);

    Task<bool> RemoveById(int id);
}
