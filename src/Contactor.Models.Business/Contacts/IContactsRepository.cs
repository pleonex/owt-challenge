namespace Contactor.Models.Business.Contacts;

using Contactor.Models.Business.Skills;

public interface IContactsRepository
{
    Task<int> Create(ContactDtoIn dto);

    Task<IEnumerable<ContactDtoOut>> GetAll();

    Task<ContactDtoOut?> GetById(int id);

    Task<bool> UpdateById(int id, ContactDtoIn dto);

    Task<bool> RemoveById(int id);

    Task<int> CreateSkill(int userId, SkillDtoIn skill);

    Task<bool> DeleteSkill(int userId, int skillId);
}
