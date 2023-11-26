namespace Contactor.Models.Business.Contacts;

using Contactor.Models.Business.Skills;

public interface IContactsRepository
{
    Task<int> CreateAsync(ContactDtoIn dto);

    Task<IEnumerable<ContactDtoOut>> GetAllAsync();

    Task<ContactDtoOut?> GetByIdAsync(int id);

    Task<bool> UpdateByIdAsync(int id, ContactDtoIn dto);

    Task<bool> RemoveByIdAsync(int id);

    Task<int> CreateSkillAsync(int userId, SkillDtoIn skill);

    Task<bool> DeleteSkillAsync(int userId, int skillId);
}
