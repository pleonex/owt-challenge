namespace Contactor.Backend.Contacts;

public class ContactDto
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string FullName => $"{LastName}, {FirstName}";

    public required string Address { get; set; }

    public required string Email { get; set; }

    public required string MobilePhone { get; set; }
}
