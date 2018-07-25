using System.Collections.Generic;
using IOTAmHackCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using IOTAmHackCore.Helpers;

namespace IOTAmHackCore.Controllers
{
    [Route("api/[controller]")]
    public class VisitorController : Controller
    {
        public VisitorController(IVisitorRepository visitors)
        {
            Visitors = visitors;
        }

        public IVisitorRepository Visitors { get; set; }

        public IEnumerable<Visitor> GetAll()
        {
            return Visitors.GetAll();
        }

        [HttpGet("{id}", Name = "GetVisitor")]
        public IActionResult GetById(string id)
        {
            var visitor = Visitors.Find(id);
            if (visitor == null)
            {
                return NotFound();
            }

            return new ObjectResult(visitor);
        }

        [HttpPost]
        public IActionResult CreateAsync([FromBody] Visitor visitor)
        {
            if (visitor == null)
            {
                return BadRequest();
            }

            Visitors.Add(visitor);

            //// Send the Adaptive card
            //string[] toRecipients = new string[]
            //{
            //    "admin@inlineconfig.onmicrosoft.com"
            //};

            //await AMHelper.SendActionableMessageAsync(toRecipients, visitor.Image, visitor.Name);

            return CreatedAtRoute("GetVisitor", new { id = visitor.Key }, visitor);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Visitor visitor)
        {
            if (visitor == null || visitor.Key != id)
            {
                return BadRequest();
            }

            var existingVisitor = Visitors.Find(id);
            if (existingVisitor == null)
            {
                return NotFound();
            }

            Visitors.Update(visitor);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Visitors.Remove(id);
        }
    }


}