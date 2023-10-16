using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private static string FilePath = Environment.CurrentDirectory + "\\Data\\database.json";

        [HttpGet]
        public IActionResult SelectAllContact()
        {
            List<Contact> contactData = new List<Contact>();
            contactData = GetContacts();

            return Ok(contactData);
        }
      
        [HttpPost]
        public IActionResult CreateContact(Contact contact)
        {
            List<Contact> contactData = new List<Contact>();
            contactData = GetContacts();

            var tempVal = contactData.OrderByDescending(u => u.Id).FirstOrDefault();

            if (tempVal == null)
            {
                contact.Id = 1;
            }
            else
            {
                contact.Id = tempVal.Id + 1;
            }
            
            contactData.Add(contact);

            var result = JsonConvert.SerializeObject(contactData.ToArray());
            System.IO.File.WriteAllText(FilePath, result);

            return Ok("Inserted Successfully");
        }

        [HttpPut]
        public IActionResult UpdateContact(Contact contact)
        {
            List<Contact> contactData = new List<Contact>();
            contactData = GetContacts();

            var selectedContact = contactData.Find(x => x.Id == contact.Id);
            
            if (selectedContact == null)
            return NotFound("Data not found");

            selectedContact.FirstName = contact.FirstName;
            selectedContact.LastName = contact.LastName;
            selectedContact.Email = contact.Email;

            var result = JsonConvert.SerializeObject(contactData.ToArray());
            System.IO.File.WriteAllText(FilePath, result);

            return Ok(selectedContact);
        }
        
        [HttpDelete]
        public IActionResult DeleteContact(int id)
        {
            List<Contact> contactData = new List<Contact>();
            contactData = GetContacts();

            var getContactInList = contactData.Find(x => x.Id == id);
            
            if (getContactInList == null)
            {
                return NotFound("Data not found");
            }

            contactData.Remove(getContactInList);

            var result = JsonConvert.SerializeObject(contactData.ToArray());
            System.IO.File.WriteAllText(FilePath, result);

            return Ok();
        }

        public static List<Contact> GetContacts()
        {
            var result = System.IO.File.ReadAllText(FilePath);
            var contacts = JsonConvert.DeserializeObject<List<Contact>>(result);
            return contacts;
        }

    }
}
