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
        public  async Task<IActionResult> CreateNotes([FromForm] NotesDTO modal)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            var response = await notesBuss.CreateNotes(userId, modal);

            if (response != null)
            {
                return Ok(new ResponseMdl <Notes> { Message = "Notes Created Successfully", IsSuccuss = true, Data = response });
            }
            else
            {
                return BadRequest(new ResponseMdl <Notes> { Message = "Notes Creation failed ", Data = response });
            }
        }

        [Authorize]
        [HttpGet("getNotesByTitleAndDesc")]
        public IActionResult FetchNotesByTitleAndDescription(string? Title,string?  description)
        {
           List<Notes> notes=notesBuss.FetchNotesByTitleAndDescrptiion(Title, description);


            if (notes != null)
            {
                return Ok(new ResponseMdl<List<Notes>> { Message = "Notes fetch is success", IsSuccuss = true, Data = notes });
            }
            else
            {
                return BadRequest(new ResponseMdl<List<Notes>> { Message = "Notes Not Found", Data = notes });
            }

        }

        [Authorize]
        [HttpGet("getNotesByID")]
        public IActionResult GetNotesById(int NoteId)
        {
            Notes notes = notesBuss.GetNotesById(NoteId);


            if (notes != null)
            {
                return Ok(new ResponseMdl <Notes> { Message = "Notes fetch is success", IsSuccuss = true, Data = notes });
            }
            else
            {
                return BadRequest(new ResponseMdl<Notes> { Message = "Note not found", Data = notes });
            }

        }


        [Authorize]
        [HttpGet("getNotes")]
        public async Task<IActionResult> GetAllNotes()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            List<Notes> notes = await notesBuss.GetAllNotes(userId);


            if (notes != null)
            {
                return Ok(new ResponseMdl<List<Notes>> { Message = "Notes fetch is success", IsSuccuss = true, Data = notes });
            }
            else
            {
                return BadRequest(new ResponseMdl<List<Notes>> { Message = "Notes not found", Data = notes });
            }

        }


        [Authorize]
        [HttpPut("updateNotes")]
        public async Task<IActionResult> UpdateNotes(int NotesId,NotesDTO modal)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            var response =await notesBuss.UpdateNotes(userId,NotesId, modal);

            if (response != null)
            {
                return Ok(new ResponseMdl<Notes> { Message = "Notes Updated Successsfully", IsSuccuss = true, Data = response });
            }
            else
            {
                return BadRequest(new ResponseMdl<Notes> { Message = "Updation of Notes Failed", Data = response });
            }
        }

        [Authorize]
        [HttpPut("ToggleArchiveNotes")]
        public IActionResult ToggleArchiveNotes(int NotesId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            var response = notesBuss.ToggleArchiveNotes(userId, NotesId);

            if (response != null)
            {
                return Ok(new ResponseMdl<bool> { Message = "Notes Archived Successsfully", IsSuccuss = true, Data = response });
            }
            else
            {
                return BadRequest(new ResponseMdl<bool> { Message = "Archive of Notes Failed", Data = response });
            }
        }


        [Authorize]
        [HttpPut("ToggleTrashNotes")]
        public IActionResult ToggleTrashNotes(int NotesId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            var response = notesBuss.ToggleTrashNotes(userId, NotesId);

            if (response != null)
            {
                return Ok(new ResponseMdl<bool> { Message = "Notes Archived Successsfully", IsSuccuss = true, Data = response });
            }
            else
            {
                return BadRequest(new ResponseMdl<bool> { Message = "Archive of Notes Failed", Data = response });
            }
        }
    }

    

    
}
