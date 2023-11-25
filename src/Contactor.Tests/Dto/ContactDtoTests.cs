namespace Contactor.Tests.Dto;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Contactor.Backend.Models;
using NUnit.Framework;

[TestFixture]
public class ContactDtoTests
{
    private static readonly ContactDto contact = new() {
        Id = 1,
        FirstName = "Benito",
        LastName = "Palacios Sanchez",
        Address = "1095 Lutry",
        Email = "benito.palsan@protonmail.com",
        MobilePhone = "077888102030",
    };

    [Test]
    public void FullNameConcatenatesBothNames()
    {
        string expected = $"{contact.LastName}, {contact.FirstName}";
        Assert.That(contact.FullName, Is.EqualTo(expected));
    }

    [Test]
    public void ValidateRequiredProperties()
    {
        var invalidContact = contact with {
            FirstName = string.Empty,
            LastName = string.Empty,
            Address = string.Empty,
            Email = string.Empty,
            MobilePhone = string.Empty,
        };

        var errors = ValidateModel(invalidContact);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(5));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.FirstName)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.LastName)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.Address)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.Email)));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.MobilePhone)));
    }

    [Test]
    public void ValidateInvalidEmail()
    {
        var invalidContact = contact with {
            Email = "benito-pm.me",
        };

        var errors = ValidateModel(invalidContact);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.Email)));
    }

    [Test]
    public void ValidateInvalidPhone()
    {
        var invalidContact = contact with {
            MobilePhone = "a4177888102030b",
        };

        var errors = ValidateModel(invalidContact);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(ContactDto.MobilePhone)));
    }

    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
