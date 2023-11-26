namespace Contactor.Backend.Controllers;

using Contactor.Models.Business.Skills;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// REST API controller to manage contact's skills.
/// </summary>
/// <param name="repository"></param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class SkillsController(ISkillRepository repository) : ControllerBase
{
    /// <summary>
    /// Gets all the existing skills.
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<SkillDtoOut>> Get()
    {
        return await repository.GetAll().ConfigureAwait(false);
    }

    /// <summary>
    /// Get the specific skill by its ID.
    /// </summary>
    /// <param name="id">Skill's ID.</param>
    /// <returns>The specific skill.</returns>
    /// <response code="200">Returns the specific skill.</response>
    /// <response code="404">Skill not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SkillDtoOut>> Get(int id)
    {
        SkillDtoOut? skill = await repository.GetById(id).ConfigureAwait(false);
        return skill is null ? NotFound() : skill;
    }

    /// <summary>
    /// Delete the specific skill.
    /// </summary>
    /// <param name="id">The skill ID.</param>
    /// <returns>The operation result.</returns>
    /// <response code="204">Operation succeed.</response>
    /// <response code="404">Skill ID not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        bool result = await repository.RemoveById(id).ConfigureAwait(false);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Updates the skill information.
    /// </summary>
    /// <remarks>
    /// Use the contacts endpoint to add or remove contacts to an skill.
    /// Be careful updating as it will affect to **all** contacts.
    /// </remarks>
    /// <param name="id">Skill ID to update.</param>
    /// <param name="value">The new skill information.</param>
    /// <returns>Operation result.</returns>
    /// <response code="204">Operation succeed.</response>
    /// <response code="400">Invalid skill information.</response>
    /// <response code="404">Skill ID not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] SkillDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        bool result = await repository.UpdateById(id, value).ConfigureAwait(false);
        return result ? NoContent() : NotFound();
    }
}
