namespace Contactor.Backend.Models.Dto.Contacts;

using Contactor.Backend.Models.Dto.Skills;

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
