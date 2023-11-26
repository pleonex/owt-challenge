namespace Contactor.Backend.Models.Domain;

using System.Collections.ObjectModel;

/// <summary>
/// Database model to represents a contact.
/// </summary>
public record Contact
{
    public int Id { get; init; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MobilePhone { get; set; } = string.Empty;

    public ICollection<Skill> Skills { get; set; } = new Collection<Skill>();
}
