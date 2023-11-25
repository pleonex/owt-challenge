﻿namespace Contactor.Backend.Models;

using System.Collections.ObjectModel;

public record Contact
{
    public required int Id { get; init; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Address { get; set; }

    public required string Email { get; set; }

    public required string MobilePhone { get; set; }

    public ICollection<Skill> Skills { get; set; } = new Collection<Skill>();
}
