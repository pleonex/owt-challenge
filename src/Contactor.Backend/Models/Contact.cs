namespace Contactor.Backend.Models;

public class Contact
{
    public required int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Address { get; set; }

    public required string Email { get; set; }

    public required string MobilePhone { get; set; }
}
