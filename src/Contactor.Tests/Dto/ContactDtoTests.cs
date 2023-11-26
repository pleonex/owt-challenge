namespace Contactor.Tests.Dto;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Contactor.Backend.Models.Dto.Contacts;
using NUnit.Framework;

[TestFixture]
public class ContactDtoTests
{
    private static readonly ContactDtoOut outputContact = new() {
        Id = 1,
        FirstName = "Benito",
        LastName = "Palacios Sanchez",
        Address = "1095 Lutry",
        Email = "benito.palsan@protonmail.com",
        MobilePhone = "077888102030",
    };

    private static readonly ContactDtoIn inputContact = new() {
        FirstName = "Benito",
        LastName = "Palacios Sanchez",
        Address = "1095 Lutry",
        Email = "benito.palsan@protonmail.com",
        MobilePhone = "077888102030",
    };

    [Test]
    public void FullNameConcatenatesBothNames()
    {
        string expected = $"{outputContact.LastName}, {outputContact.FirstName}";
        Assert.That(outputContact.FullName, Is.EqualTo(expected));
    }

    [Test]
    public void ValidateRequiredProperties()
    {
        var invalidContact = inputContact with {
            FirstName = string.Empty,
            LastName = string.Empty,
            Address = string.Empty,
            Email = string.Empty,
            MobilePhone = string.Empty,
        };

        var errors = SimpleModelValidator.ValidateModel(invalidContact);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(5));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.FirstName)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.LastName)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.Address)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.Email)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.MobilePhone)));
    }

    [Test]
    public void ValidateInvalidEmail()
    {
        var invalidContact = inputContact with {
            Email = "benito-pm.me",
        };

        var errors = SimpleModelValidator.ValidateModel(invalidContact);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.Email)));
    }

    [Test]
    public void ValidateInvalidPhone()
    {
        var invalidContact = inputContact with {
            MobilePhone = "a4177888102030b",
        };

        var errors = SimpleModelValidator.ValidateModel(invalidContact);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDtoIn.MobilePhone)));
    }
}
