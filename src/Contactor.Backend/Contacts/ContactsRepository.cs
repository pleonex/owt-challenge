﻿namespace Contactor.Backend.Contacts;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class ContactsRepository(ContactsDbContext dbContext) : IRepository<ContactDto>
{
    public async Task<int> Create(ContactDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        Contact newContact = ToModel(dto);
        await dbContext.AddAsync(newContact).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return newContact.Id;
    }

    public async Task<ContactDto?> Read(int id)
    {
        Contact? contact = await dbContext.Contacts
            .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

        return contact is null ? null : ToDto(contact);
    }

    public IEnumerable<ContactDto> ReadAll()
    {
        // FUTURE: Add paging
        return dbContext.Contacts.Select(ToDto);
    }

    public async Task<bool> Remove(int id)
    {
        Contact? contact = await dbContext.Contacts.FindAsync(id).ConfigureAwait(false);
        if (contact is null) {
            return false;
        }

        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<bool> Update(int id, ContactDto dto)
    {
        if (id != dto.Id) {
            return false;
        }

        Contact? model = await dbContext.Contacts.FindAsync(id).ConfigureAwait(false);
        if (model is null) {
            return false;
        }

        model.FirstName = dto.FirstName;
        model.LastName = dto.LastName;
        model.Address = dto.Address;
        model.Email = dto.Email;
        model.MobilePhone = dto.MobilePhone;

        dbContext.Contacts.Update(model);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    private static Contact ToModel(ContactDto dto)
    {
        // FUTURE: Use AutoMapper
        return new Contact {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Email = dto.Email,
            MobilePhone = dto.MobilePhone,
        };
    }

    private static ContactDto ToDto(Contact model)
    {
        return new ContactDto {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            Email = model.Email,
            MobilePhone = model.MobilePhone,
        };
    }
}
