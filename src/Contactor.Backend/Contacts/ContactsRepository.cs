namespace Contactor.Backend.Contacts;

using System.Collections.Generic;

public class ContactsRepository : IRepository<Contact>
{
    private readonly List<Contact> contacts = new();

    public void Create(Contact model)
    {
        if (contacts.Contains(model)) {
            throw new Exception("Already here");
        }

        contacts.Add(model);
    }

    public Contact Read(string id)
    {
        Contact? contact = contacts.Find(c => c.Id == id);
        if (contact is null) {
            throw new Exception("not found");
        }

        return contact;
    }

    public IEnumerable<Contact> ReadAll()
    {
        return contacts;
    }

    public void Remove(string id)
    {
        int index = contacts.FindIndex(c => c.Id == id);
        if (index == -1) {
            throw new Exception("not found");
        }

        contacts.RemoveAt(index);
    }

    public void Update(string id, Contact model)
    {
        int index = contacts.FindIndex(c => c.Id == id);
        if (index == -1) {
            throw new Exception("not found");
        }

        // TODO: copy ID
        contacts[index] = model;
    }
}
