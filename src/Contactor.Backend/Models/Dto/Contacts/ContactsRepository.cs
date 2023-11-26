namespace Contactor.Backend.Models.Dto.Contacts;

using System.Collections.Generic;
using Contactor.Backend.Models.Domain;
using Contactor.Backend.Models.Dto.Skills;
using Microsoft.EntityFrameworkCore;

public class ContactsRepository(ContactsDbContext dbContext) : IContactsRepository
{
    public async Task<int> Create(ContactDtoIn dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var newModel = new Contact();
        dto.UpdateModel(newModel);

        await dbContext.AddAsync(newModel).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        // After the DB has assigned an ID, return it.
        return newModel.Id;
    }

    public async Task<ContactDtoOut?> GetById(int id)
    {
        Contact? contact = await dbContext.Contacts
            .Include(c => c.Skills)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);

        return contact is null ? null : ContactDtoOut.FromModel(contact);
    }

    public async Task<IEnumerable<ContactDtoOut>> GetAll()
    {
        // We don't include the skills to reduce the output as we don't have requirements
        // Needs to be queried by ID.
        List<Contact> list = await dbContext.Contacts
            .ToListAsync().ConfigureAwait(false);

        return list.Select(ContactDtoOut.FromModel);
    }

    public async Task<bool> RemoveById(int id)
    {
        Contact? contact = await dbContext.Contacts.FindAsync(id).ConfigureAwait(false);
        if (contact is null) {
            return false;
        }

        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<bool> UpdateById(int id, ContactDtoIn dto)
    {
        Contact? model = await dbContext.Contacts.FindAsync(id).ConfigureAwait(false);
        if (model is null) {
            return false;
        }

        dto.UpdateModel(model);

        dbContext.Contacts.Update(model);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<int> CreateSkill(int userId, SkillDtoIn skill)
    {
        Contact? contact = await dbContext.Contacts
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == userId).ConfigureAwait(false);
        if (contact is null) {
            return -1;
        }

        // Find matching skill or create
        Skill? skillModel = await dbContext.Skills
            .FirstOrDefaultAsync(s => s.Name == skill.Name && s.Level == skill.Level)
            .ConfigureAwait(false);
        if (skillModel is null) {
            skillModel = new Skill();
            skill.UpdateModel(skillModel);
        }

        contact.Skills.Add(skillModel);
        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return contact.Id;
    }

    public async Task<bool> DeleteSkill(int userId, int skillId)
    {
        Contact? contact = await dbContext.Contacts
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == userId).ConfigureAwait(false);
        if (contact is null) {
            return false;
        }

        Skill? skill = contact.Skills.FirstOrDefault(s => s.Id == skillId);
        if (skill is null) {
            return false;
        }

        contact.Skills.Remove(skill);
        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
