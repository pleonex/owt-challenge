﻿namespace Contactor.Tests.Controllers;

using System.Threading.Tasks;
using Contactor.Backend.Controllers;
using Contactor.Backend.Models.Domain;
using Contactor.Backend.Models.Dto.Skills;
using Microsoft.AspNetCore.Http.HttpResults;
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

        repository.Setup(x => x.GetAll())
            .ReturnsAsync(expected)
            .Verifiable(Times.Once);

        var result = await controller.Get().ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByIdReturnsExpectedSkill()
    {
        SkillDtoOut skill = SkillsData.Skill1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.GetById(skill.Id))
            .ReturnsAsync(skill)
            .Verifiable(Times.Once);

        var result = await controller.Get(skill.Id).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result.Value, Is.SameAs(skill));
    }

    [Test]
    public async Task GetByWrongIdReturnsNotFound()
    {
        int skillId = 1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.GetById(skillId))
            .ReturnsAsync((SkillDtoOut)null)
            .Verifiable(Times.Once);

        var result = await controller.Get(skillId).ConfigureAwait(false);

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

        repository.Setup(x => x.UpdateById(skill.Id, skill))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.Put(skill.Id, skill).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PutValidatesModel()
    {
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        controller.ModelState.AddModelError("error", "Some error");
        var result = await controller.Put(3, SkillsData.InvalidSkill).ConfigureAwait(false);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PutFailedToUpdateReturnsNotFound()
    {
        SkillDtoOut skill = SkillsData.Skill2;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.UpdateById(skill.Id, skill))
            .ReturnsAsync(false)
            .Verifiable(Times.Once);

        var result = await controller.Put(skill.Id, skill).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteRemovesContact()
    {
        int skillId = 1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.RemoveById(skillId))
            .ReturnsAsync(true)
            .Verifiable(Times.Once);

        var result = await controller.Delete(skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteFailedReturnsNotFound()
    {
        int skillId = 1;
        var repository = new Mock<ISkillRepository>();
        var controller = new SkillsController(repository.Object);

        repository.Setup(x => x.RemoveById(skillId))
            .ReturnsAsync(false)
            .Verifiable(Times.Once);

        var result = await controller.Delete(skillId).ConfigureAwait(false);

        repository.Verify();
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}