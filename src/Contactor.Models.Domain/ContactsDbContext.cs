﻿namespace Contactor.Models.Domain;

using Microsoft.EntityFrameworkCore;

public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Skill> Skills { get; set; }
}
