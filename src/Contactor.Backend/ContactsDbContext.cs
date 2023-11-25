namespace Contactor.Backend;

using Contactor.Backend.Contacts;
using Microsoft.EntityFrameworkCore;

public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
}
