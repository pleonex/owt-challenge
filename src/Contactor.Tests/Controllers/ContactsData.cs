namespace Contactor.Tests.Controllers;
using Contactor.Backend.Models;

internal static class ContactsData
{
    private static ContactDto contact1;
    private static ContactDto contact2;
    private static ContactDto invalidContact;
    private static ContactDto[] allContacts;

    public static ContactDto Contact1 => contact1 ??= new ContactDto {
        Id = 1,
        FirstName = "First1",
        LastName = "Last1",
        Address = "Lutry",
        Email = "first1@last1.com",
        MobilePhone = "+4177810102030",
    };

    public static ContactDto Contact2 => contact2 ??= new ContactDto {
        Id = 2,
        FirstName = "First2",
        LastName = "Last2",
        Address = "Lausanne",
        Email = "first2@last2.com",
        MobilePhone = "+4177810112131",
    };

    public static ContactDto InvalidContact => invalidContact ??= new ContactDto {
        Id = 2,
        FirstName = "",
        LastName = "",
        Address = "",
        Email = "first2-last2.com",
        MobilePhone = "-4177810112131",
    };

    public static ContactDto[] AllContacts => allContacts ??= [Contact1, Contact2];
}
