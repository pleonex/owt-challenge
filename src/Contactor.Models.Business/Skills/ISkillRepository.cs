namespace Contactor.Models.Business.Skills;
public interface ISkillRepository
{
    Task<IEnumerable<SkillDtoOut>> GetAllAsync();

    Task<SkillDtoOut?> GetByIdAsync(int id);

    Task<bool> UpdateByIdAsync(int id, SkillDtoIn dto);

    Task<bool> RemoveByIdAsync(int id);
}
