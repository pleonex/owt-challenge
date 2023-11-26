namespace Contactor.Models.Business.Skills;

using System.Collections.Generic;
using System.Threading.Tasks;
using Contactor.Models.Domain;
using Microsoft.EntityFrameworkCore;

public class SkillRepository(ContactsDbContext dbContext) : ISkillRepository
{
    public async Task<IEnumerable<SkillDtoOut>> GetAll()
    {
        List<Skill> list = await dbContext.Skills
            .Include(s => s.Contacts)
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);

        return list.Select(SkillDtoOut.FromModel);
    }

    public async Task<SkillDtoOut?> GetById(int id)
    {
        Skill? model = await dbContext.Skills
            .Include(s => s.Contacts)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);

        return model is null ? null : SkillDtoOut.FromModel(model);
    }

    public async Task<bool> RemoveById(int id)
    {
        Skill? model = await dbContext.Skills.FindAsync(id).ConfigureAwait(false);
        if (model is null) {
            return false;
        }

        dbContext.Skills.Remove(model);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<bool> UpdateById(int id, SkillDtoIn dto)
    {
        Skill? model = await dbContext.Skills.FindAsync(id).ConfigureAwait(false);
        if (model is null) {
            return false;
        }

        dto.UpdateModel(model);

        dbContext.Skills.Update(model);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
