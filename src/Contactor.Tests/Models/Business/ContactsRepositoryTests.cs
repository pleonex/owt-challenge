namespace Contactor.Tests.Models.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Contactor.Models.Business.Contacts;
using Contactor.Models.Domain;
using Contactor.Tests.Controllers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

[TestFixture]
public class ContactsRepositoryTests
{
    [Test]
    public async Task CreateContactUpdatesTable()
    {
        ContactDtoIn expected = ContactsData.Contact1;

        using var context = DatabaseFixture.CreateContext();
        var repository = new ContactsRepository(context);

        await context.Database.BeginTransactionAsync();
        await repository.CreateAsync(expected);
        context.ChangeTracker.Clear();

        Contact actual = await context.Contacts.SingleAsync();
        actual.Should().BeEquivalentTo(expected);
        actual.Id.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task CreateReturnsNewId()
    {
        ContactDtoIn expected = ContactsData.Contact1;

        using var context = DatabaseFixture.CreateContext();
        var repository = new ContactsRepository(context);

        await context.Database.BeginTransactionAsync();
        int newId = await repository.CreateAsync(expected);
        context.ChangeTracker.Clear();

        Contact actual = await context.Contacts.SingleAsync();
        actual.Id.Should().Be(newId);
    }

    [Test]
    public async Task GetAllReturnsFullList()
    {
        using var context = DatabaseFixture.CreateContext();
        var repository = new ContactsRepository(context);
        await SeedContactsAsync(context);

        var actual = await repository.GetAllAsync();

        actual.Should().BeEquivalentTo(ContactsData.AllContacts);
    }

    [Test]
    public async Task GetAllDoesNotIncludeSkills()
    {
        using var context = DatabaseFixture.CreateContext();
        var repository = new ContactsRepository(context);
        await SeedWithSkillsAsync(context);

        var actual = await repository.GetAllAsync();

        actual.Should().BeEquivalentTo(ContactsData.AllContacts);
    }

    [Test]
    public async Task GetByIdReturnsContactWithSkills()
    {
        var expected = ContactsData.Contact1 with {
            Skills = [
                new ContactDtoOut.SkillDto {
                    Id = SkillsData.Skill1.Id,
                    Name = SkillsData.Skill1.Name,
                    Level = SkillsData.Skill1.Level,
                }
            ],
        };

        using var context = DatabaseFixture.CreateContext();
        var repository = new ContactsRepository(context);
        await SeedWithSkillsAsync(context);

        var actual = await repository.GetByIdAsync(expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdReturnsNullIfNotFound()
    {
        using var context = DatabaseFixture.CreateContext();
        var repository = new ContactsRepository(context);
        await SeedContactsAsync(context);

        var actual = await repository.GetByIdAsync(1000);

        actual.Should().BeNull();
    }

    private async Task SeedContactsAsync(ContactsDbContext context)
    {
        await context.Database.BeginTransactionAsync();
        foreach (var contact in ContactsData.AllContacts) {
            Contact model = new Contact();
            contact.UpdateModel(model);
            await context.Contacts.AddAsync(model);
        }

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    private async Task SeedWithSkillsAsync(ContactsDbContext context)
    {
        await SeedContactsAsync(context);

        var contact = await context.Contacts.FirstAsync();

        var skill = new Skill();
        SkillsData.Skill1.UpdateModel(skill);
        contact.Skills.Add(skill);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}
