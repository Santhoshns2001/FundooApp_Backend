using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Service;
using DataAcessLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.DTOs;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;

namespace FundooApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase

    {
        private readonly FundooDBContext context;
        private readonly INotesBuss notesBuss;
        public NotesController(FundooDBContext context, INotesBuss notes)
        {
            this.context = context;
            this.notesBuss = notes;
        }

        [Authorize]
        [HttpPost("notes")]
        public IActionResult CreateNotes(NotesDTO modal)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            var response = notesBuss.CreateNotes(userId, modal);

            if (response != null)
            {
                return Ok(new ResponseMdl<Notes> { Message = "Registered Successfully", IsSuccuss = true, Data = response });
            }
            else
            {
                return BadRequest(new ResponseMdl<Notes> { Message = "User Failed to register", Data = response });
            }
        }
        



    }
}
