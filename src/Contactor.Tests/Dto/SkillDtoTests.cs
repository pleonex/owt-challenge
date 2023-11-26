namespace Contactor.Tests.Dto;

using System.Linq;
using Contactor.Backend.Models.Dto.Skills;
using NUnit.Framework;

[TestFixture]
public class SkillDtoTests
{
    private static readonly SkillDtoIn validSkill = new() {
        Name = "csharp",
        Level = 5,
    };

    [Test]
    public void ValidateRequiredProperties()
    {
        var invalidSkill = validSkill with { Name = string.Empty };

        var errors = SimpleModelValidator.ValidateModel(invalidSkill);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(SkillDtoIn.Name)));
    }

    [Test]
    public void ValidateLevelValidRange()
    {
        var invalidSkill = validSkill with { Level = 6 };

        var errors = SimpleModelValidator.ValidateModel(invalidSkill);
        var errorPropertyNames = errors.SelectMany(x => x.MemberNames);

        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errorPropertyNames, Has.One.EqualTo(nameof(SkillDtoIn.Level)));
    }
}
