namespace Contactor.Backend.Controllers;

using Contactor.Models.Business.Contacts;
using Contactor.Models.Business.Skills;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// REST API controller to manage the contact list.
/// </summary>
/// <param name="contactsRepo">Repository to manage the contact storage.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ContactsController(IContactsRepository contactsRepo) : ControllerBase
{
    /// <summary>
    /// Gets the full list of contacts.
    /// </summary>
    /// <returns>Collection of current contacts.</returns>
    /// <response code="200">Returns the current contact list.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ContactDtoOut>> GetAllContacts()
    {
        return await contactsRepo.GetAllAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a contact by its ID.
    /// </summary>
    /// <param name="id">Contact ID.</param>
    /// <returns>The required contact.</returns>
    /// <response code="200">Returns the contact matching ID.</response>
    /// <response code="404">Contact with the given ID not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDtoOut>> GetContactById(int id)
    {
        ContactDtoOut? contact = await contactsRepo.GetByIdAsync(id);
        return contact is null ? NotFound() : contact;
    }

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="value">Contact information.</param>
    /// <returns>A newly contact.</returns>
    /// <response code="201">The newly created contact.</response>
    /// <response code="400">The contact information is not valid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreateNewContact([FromBody] ContactDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        int itemId = await contactsRepo.CreateAsync(value);
        return CreatedAtAction(nameof(GetContactById), new { id = itemId }, value);
    }

    /// <summary>
    /// Updates the contact information.
    /// </summary>
    /// <param name="id">Contact's ID.</param>
    /// <param name="value">The new contact information.</param>
    /// <returns>Operation status result.</returns>
    /// <response code="204">Operation succeed.</response>
    /// <response code="400">Invalid contact information.</response>
    /// <response code="404">Contact not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateContactById(int id, [FromBody] ContactDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        bool result = await contactsRepo.UpdateByIdAsync(id, value);
        if (!result) {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes the given contact information.
    /// </summary>
    /// <param name="id">The contact's ID to delete.</param>
    /// <returns>Operation result.</returns>
    /// <response code="204">Operation succeed.</response>
    /// <response code="404">Contact does not exist.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteContactById(int id)
    {
        bool result = await contactsRepo.RemoveByIdAsync(id);
        if (!result) {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Adds a skill to the given contact.
    /// </summary>
    /// <param name="userId">The contact's ID to add skills.</param>
    /// <param name="value">Skill information.</param>
    /// <returns>The newly skill.</returns>
    /// <response code="201">Operation succeed.</response>
    /// <response code="404">Contact does not exist.</response>
    [HttpPost("{userId}/skills")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreateContactSkillById(int userId, [FromBody] SkillDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        int result = await contactsRepo.CreateSkillAsync(userId, value);
        if (result < 0) {
            return NotFound();
        }

        return CreatedAtAction(nameof(GetContactById), new { id = userId }, value);
    }

    /// <summary>
    /// Deletes a skill from a contact.
    /// </summary>
    /// <param name="userId">The contact's ID to remove skills.</param>
    /// <param name="skillId">The skill's ID to remove.</param>
    /// <returns>Operation result.</returns>
    /// <response code="204">Skill removed successfully.</response>
    /// <response code="404">The contact does not exist or it doesn't have that skill.</response>
    [HttpDelete("{userId}/skills/{skillId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContactSkillById(int userId, int skillId)
    {
        bool result = await contactsRepo.DeleteSkillAsync(userId, skillId);

        return result ? NoContent() : NotFound();
    }
}
