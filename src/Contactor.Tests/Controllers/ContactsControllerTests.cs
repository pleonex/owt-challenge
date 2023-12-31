﻿namespace Contactor.Tests.Controllers;

using System.Threading.Tasks;
using Contactor.Backend.Controllers;
using Contactor.Models.Business.Contacts;
using Contactor.Models.Business.Skills;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ContactsControllerTests
{
    [Test]
    public async Task GetReturnsFullList()
    {
        ContactDtoOut[] expected = ContactsData.AllContacts;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(expected)
            .Verifiable(Times.Once);

        var actualContacts = await controller.GetAllContacts().ConfigureAwait(false);

        repository.Verify();
        Assert.That(actualContacts, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByIdReturnsExpectedContact()
    {
        ContactDtoOut expected = ContactsData.Contact2;
        int id = 2;

        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(expected);

        var result = await controller.GetContactById(id).ConfigureAwait(false);

        Assert.That(result.Value, Is.SameAs(expected));
    }

    [Test]
    public async Task GetByWrongIdReturnsNotFound()
    {
        int id = 5;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((ContactDtoOut)null);

        var result = await controller.GetContactById(id).ConfigureAwait(false);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        Assert.That(result.Value, Is.Null);
    }

    [Test]
    public async Task PostCreatesNewContact()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.CreateAsync(contact))
            .ReturnsAsync(id)
            .Verifiable(Times.Once);

        _ = await controller.CreateNewContact(contact).ConfigureAwait(false);

        repository.Verify();
    }

    [Test]
    public async Task PostReturnsRedirection()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.CreateAsync(contact))
            .ReturnsAsync(id);

        var result = await controller.CreateNewContact(contact).ConfigureAwait(false);
        var createdResult = result as CreatedAtActionResult;

        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(ContactsController.GetContactById)));
        Assert.That(createdResult.RouteValues["id"], Is.EqualTo(id));
        Assert.That(createdResult.Value, Is.SameAs(contact));
    }

    [Test]
    public async Task PostChecksModelValidationAndReturnsBadRequest()
    {
        ContactDtoIn contact = ContactsData.InvalidContact;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        // Actual model validation is done in other test suite - Here we just give it one error
        controller.ModelState.AddModelError(nameof(ContactDtoIn.FirstName), "Invalid name");
        var result = await controller.CreateNewContact(contact).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PutUpdatesModel()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.UpdateByIdAsync(id, contact))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.UpdateContactById(id, contact).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PutValidatesModel()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.InvalidContact;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.UpdateByIdAsync(id, contact))
            .Verifiable(Times.Never);

        controller.ModelState.AddModelError(nameof(ContactDtoIn.FirstName), "Invalid name");
        var result = await controller.UpdateContactById(id, contact).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PutFailedToUpdateReturnsNotFound()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.UpdateByIdAsync(id, contact))
            .ReturnsAsync(false);

        var result = await controller.UpdateContactById(id, contact).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteRemovesContact()
    {
        int id = 1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.RemoveByIdAsync(id))
            .ReturnsAsync(true);

        var result = await controller.DeleteContactById(id).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteFailedReturnsNotFound()
    {
        int id = 1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.RemoveByIdAsync(id))
            .ReturnsAsync(false);

        var result = await controller.DeleteContactById(id).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task AddSkillsUpdatesContactAndReturnsRelocation()
    {
        int userId = 1;
        int skillId = 5;
        SkillDtoIn skill = SkillsData.Skill1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.CreateSkillAsync(userId, skill))
            .ReturnsAsync(skillId)
            .Verifiable(Times.Once);

        var result = await controller.CreateContactSkillById(userId, skill).ConfigureAwait(false);
        var createdResult = result as CreatedAtActionResult;

        repository.Verify();
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(ContactsController.GetContactById)));
        Assert.That(createdResult.RouteValues["id"], Is.EqualTo(userId));
        Assert.That(createdResult.Value, Is.SameAs(skill));
    }

    [Test]
    public async Task AddSkillChecksModelValidation()
    {
        int userId = 1;
        SkillDtoIn skill = SkillsData.InvalidSkill;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        controller.ModelState.AddModelError("error", "Something is wrong with the input");
        var result = await controller.CreateContactSkillById(userId, skill).ConfigureAwait(false);

        Assert.That(result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task AddSkillsForInvalidUserReturnsNotFound()
    {
        int userId = 1;
        SkillDtoIn skill = SkillsData.Skill1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.CreateSkillAsync(userId, skill))
            .ReturnsAsync(-1)
            .Verifiable(Times.Once);

        var result = await controller.CreateContactSkillById(userId, skill).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteSkillUpdatesContact()
    {
        int userId = 1;
        int skillId = 5;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.DeleteSkillAsync(userId, skillId))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.DeleteContactSkillById(userId, skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteSkillForInvalidUserOrSkillReturnsNotFound()
    {
        int userId = 1;
        int skillId = 5;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.DeleteSkillAsync(userId, skillId))
            .ReturnsAsync(false)
            .Verifiable(Times.Once);

        var result = await controller.DeleteContactSkillById(userId, skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
