namespace Contactor.Backend.Models.Domain;

using System.Collections.ObjectModel;

public record Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Level { get; set; } = -1;

    public ICollection<Contact> Contacts { get; set; } = new Collection<Contact>();
}
