namespace Contactor.Backend.Contacts;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ContactsController(IRepository<ContactDto> contactsRepo) : ControllerBase
{
    // GET: api/<ContactsController>
    [HttpGet]
    public IEnumerable<ContactDto> Get()
    {
        return contactsRepo.ReadAll();
    }

    // GET api/<ContactsController>/5
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        ContactDto? contact = contactsRepo.Read(id);
        return contact is null ? NotFound() : Ok(contact);
    }

    // POST api/<ContactsController>
    [HttpPost]
    public void Post([FromBody] ContactDto value)
    {
        if (ModelState.IsValid) {
            contactsRepo.Create(value);
        }
    }

    // PUT api/<ContactsController>/5
    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] ContactDto value)
    {
        contactsRepo.Update(id, value);
    }

    // DELETE api/<ContactsController>/5
    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        contactsRepo.Remove(id);
    }
}
