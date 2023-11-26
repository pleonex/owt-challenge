namespace Contactor.Tests.Controllers;

using System.Threading.Tasks;
using Contactor.Backend.Controllers;
using Contactor.Backend.Models.Dto.Contacts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ContactControllerTests
{
    [Test]
    public async Task GetReturnsFullList()
    {
        ContactDtoOut[] expected = ContactsData.AllContacts;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.GetAll())
            .ReturnsAsync(expected);

        var actualContacts = await controller.Get().ConfigureAwait(false);

        Assert.That(actualContacts, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByIdReturnsExpectedContact()
    {
        ContactDtoOut expected = ContactsData.Contact2;
        int id = 2;

        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.GetById(id))
            .ReturnsAsync(expected);

        var result = await controller.Get(id).ConfigureAwait(false);

        Assert.That(result.Value, Is.SameAs(expected));
    }

    [Test]
    public async Task GetByWrongIdReturnsNotFound()
    {
        int id = 5;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.GetById(id))
            .ReturnsAsync((ContactDtoOut)null);

        var result = await controller.Get(id).ConfigureAwait(false);

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

        repository.Setup(x => x.Create(contact))
            .ReturnsAsync(id)
            .Verifiable(Times.Once);

        _ = await controller.Post(contact).ConfigureAwait(false);

        repository.Verify();
    }

    [Test]
    public async Task PostReturnsRedirection()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.Create(contact))
            .ReturnsAsync(id);

        var result = await controller.Post(contact).ConfigureAwait(false);
        var createdResult = result.Result as CreatedAtActionResult;

        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(ContactsController.Get)));
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
        var result = await controller.Post(contact).ConfigureAwait(false);

        Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PutUpdatesModel()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.UpdateById(id, contact))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.Put(id, contact).ConfigureAwait(false);

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

        controller.ModelState.AddModelError(nameof(ContactDtoIn.FirstName), "Invalid name");
        var result = await controller.Put(id, contact).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PutFailedToUpdateReturnsBadRequest()
    {
        int id = 1;
        ContactDtoIn contact = ContactsData.Contact1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.UpdateById(id, contact))
            .ReturnsAsync(false);

        var result = await controller.Put(id, contact).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task DeleteRemovesContact()
    {
        int id = 1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.RemoveById(id))
            .ReturnsAsync(true);

        var result = await controller.Delete(id).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteFailedReturnsNotFound()
    {
        int id = 1;
        var repository = new Mock<IContactsRepository>();
        var controller = new ContactsController(repository.Object);

        repository.Setup(x => x.RemoveById(id))
            .ReturnsAsync(false);

        var result = await controller.Delete(id).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
