namespace Contactor.Backend.Contacts;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ContactsController(IRepository<Contact> contactsRepo) : ControllerBase
{
    // GET: api/<ContactsController>
    [HttpGet]
    public IEnumerable<Contact> Get()
    {
        return contactsRepo.ReadAll();
    }

    // GET api/<ContactsController>/5
    [HttpGet("{id}")]
    public Contact Get(string id)
    {
        return contactsRepo.Read(id);
    }

    // POST api/<ContactsController>
    [HttpPost]
    public void Post([FromBody] Contact value)
    {
        contactsRepo.Create(value);
    }

    // PUT api/<ContactsController>/5
    [HttpPut("{id}")]
    public void Put(string id, [FromBody] Contact value)
    {
        contactsRepo.Update(id, value);
    }

    // DELETE api/<ContactsController>/5
    [HttpDelete("{id}")]
    public void Delete(string id)
    {
        contactsRepo.Remove(id);
    }
}
