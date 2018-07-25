using System.Collections.Generic;
using IOTAmHackCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOTAmHackCore.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        public RoomController(IRoomRepository rooms)
        {
            Rooms = rooms;
        }
        public IRoomRepository Rooms { get; set; }

        public IEnumerable<Room> GetAll()
        {
            return Rooms.GetAll();
        }

        [HttpGet("{id}", Name = "GetRoom")]
        public IActionResult GetById(string id)
        {
            var room = Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return new ObjectResult(room);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest();
            }

            Rooms.Add(room);

            return CreatedAtRoute("GetRoom", new { id = room.Key }, room);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Room room)
        {
            if (room == null || room.Key != id)
            {
                return BadRequest();
            }

            var existingRoom = Rooms.Find(id);
            if (existingRoom == null)
            {
                return NotFound();
            }

            Rooms.Update(room);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Rooms.Remove(id);
        }
    }

   
}