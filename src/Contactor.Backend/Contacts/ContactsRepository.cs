namespace Contactor.Backend.Contacts;

using System.Collections.Generic;

public class ContactsRepository : IRepository<ContactDto>
{
    private readonly List<Contact> contacts = new();

    public bool Create(ContactDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var newContact = ToModel(dto);
        newContact.Id = Guid.NewGuid(); // will be assigned by DB later.

        contacts.Add(newContact);
        return true;
    }

    public ContactDto? Read(Guid id)
    {
        Contact? contact = contacts.Find(c => c.Id == id);

        return contact is null ? null : ToDto(contact);
    }

    public IEnumerable<ContactDto> ReadAll()
    {
        return contacts.Select(ToDto);
    }

    public bool Remove(Guid id)
    {
        int index = contacts.FindIndex(c => c.Id == id);
        if (index == -1) {
            return false;
        }

        contacts.RemoveAt(index);
        return true;
    }

    public bool Update(Guid id, ContactDto dto)
    {
        int index = contacts.FindIndex(c => c.Id == id);
        if (index == -1) {
            return false;
        }

        var model = contacts[index];
        model.FirstName = dto.FirstName;
        model.LastName = dto.LastName;
        model.Address = dto.Address;
        model.Email = dto.Email;
        model.MobilePhone = dto.MobilePhone;
        return true;
    }

    private static Contact ToModel(ContactDto dto)
    {
        // FUTURE: Use AutoMapper
        return new Contact {
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
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            Email = model.Email,
            MobilePhone = model.MobilePhone,
        };
    }
}
