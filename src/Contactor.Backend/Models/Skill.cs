namespace Contactor.Backend.Models;

public record Skill
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int Level { get; init; }

    public required int ContactId { get; init; }

    public required Contact Contact { get; init; }
}
