namespace Contactor.Tests.Controllers;

using System.Threading.Tasks;
using Contactor.Backend.Controllers;
using Contactor.Models.Business.Skills;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

[TestFixture]
public class SkillsControllerTests
{
    [Test]
    public async Task GetReturnsFullList()
    {
        SkillDtoOut[] expected = SkillsData.AllSkills;

        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(expected)
            .Verifiable(Times.Once);

        var result = await controller.GetAllSkills().ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByIdReturnsExpectedSkill()
    {
        SkillDtoOut skill = SkillsData.Skill1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.GetByIdAsync(skill.Id))
            .ReturnsAsync(skill)
            .Verifiable(Times.Once);

        var result = await controller.GetSkillsById(skill.Id).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result.Value, Is.SameAs(skill));
    }

    [Test]
    public async Task GetByWrongIdReturnsNotFound()
    {
        int skillId = 1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.GetByIdAsync(skillId))
            .ReturnsAsync((SkillDtoOut)null)
            .Verifiable(Times.Once);

        var result = await controller.GetSkillsById(skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        Assert.That(result.Value, Is.Null);
    }

    [Test]
    public async Task PutUpdatesModel()
    {
        SkillDtoOut skill = SkillsData.Skill2;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.UpdateByIdAsync(skill.Id, skill))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.UpdateSkillById(skill.Id, skill).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PutValidatesModel()
    {
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        controller.ModelState.AddModelError("error", "Some error");
        var result = await controller.UpdateSkillById(3, SkillsData.InvalidSkill).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PutFailedToUpdateReturnsNotFound()
    {
        SkillDtoOut skill = SkillsData.Skill2;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.UpdateByIdAsync(skill.Id, skill))
            .ReturnsAsync(false)
            .Verifiable(Times.Once);

        var result = await controller.UpdateSkillById(skill.Id, skill).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteRemovesContact()
    {
        int skillId = 1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.RemoveByIdAsync(skillId))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.DeleteSkillById(skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteFailedReturnsNotFound()
    {
        int skillId = 1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.RemoveByIdAsync(skillId))
            .ReturnsAsync(false)
            .Verifiable(Times.Once);

        var result = await controller.DeleteSkillById(skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
