namespace Contactor.Backend;

using Contactor.Backend.Models;
using Microsoft.EntityFrameworkCore;

public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
}
