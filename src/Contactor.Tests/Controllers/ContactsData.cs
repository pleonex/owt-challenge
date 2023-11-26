namespace Contactor.Tests.Controllers;

using Contactor.Backend.Models.Dto.Contacts;

internal static class ContactsData
{
    private static ContactDtoOut contact1;
    private static ContactDtoOut contact2;
    private static ContactDtoIn invalidContact;
    private static ContactDtoOut[] allContacts;
    private static ContactDtoOut.SkillDto contactSkill;
    public static ContactDtoOut Contact1 => contact1 ??= new ContactDtoOut {
        Id = 1,
        FirstName = "First1",
        LastName = "Last1",
        Address = "Lutry",
        Email = "first1@last1.com",
        MobilePhone = "+4177810102030",
    };

    public static ContactDtoOut Contact2 => contact2 ??= new ContactDtoOut {
        Id = 2,
        FirstName = "First2",
        LastName = "Last2",
        Address = "Lausanne",
        Email = "first2@last2.com",
        MobilePhone = "+4177810112131",
    };

    public static ContactDtoIn InvalidContact => invalidContact ??= new ContactDtoIn {
        FirstName = "",
        LastName = "",
        Address = "",
        Email = "first2-last2.com",
        MobilePhone = "-4177810112131",
    };

    public static ContactDtoOut[] AllContacts => allContacts ??= [Contact1, Contact2];

    public static ContactDtoOut.SkillDto ContactSkill => contactSkill ??= new() {
        Id = 1,
        Name = "csharp",
        Level = 5,
    };
}
