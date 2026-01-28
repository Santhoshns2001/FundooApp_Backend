using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.DTOs;
using ModalLayer.DTOs.Label;
using ModalLayer.Entities;

namespace FundooApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBuss labelBuss;

        public LabelController(ILabelBuss labelBuss)
        {
            this.labelBuss = labelBuss;
        }

        [HttpPost]
        [Authorize]
        [Route("addLabel")]
        public IActionResult CreateLabel( [FromBody] CreateLabelRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Label label=labelBuss.CreateLabel(userId, request.LabelName);

            if (label != null)
            {
                return Ok(new ResponseMdl<Label> { IsSuccuss = true, Data = label, Message = "Label added to the notes " });
            }
            else
            {
                return BadRequest(new ResponseMdl<Label> { IsSuccuss = false, Data = label, Message = "Adding Label to Notes is Failed" });
            }
        }

        [Authorize]
        [HttpGet("GetAllLabels")]
        public IActionResult FetchLabel()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            List<Label> label = labelBuss.FetchLabel( userId);

            if (label != null)
            {
                return Ok(new ResponseMdl<List<Label>> { IsSuccuss = true, Data = label, Message = "Label fetched successfully" });
            }
            else
            {
                return BadRequest(new ResponseMdl<List<Label>> { IsSuccuss = false, Data = label, Message = "Failed to fetch label" });
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("DeleteLabel")]
        public IActionResult DeleteLabel(int LabelId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            bool label = labelBuss.DeleteLabel( userId, LabelId);

            if (label)
            {
                return Ok(new ResponseMdl<bool> { IsSuccuss = true, Data = label, Message = "label removed successfully " });
            }
            else
            {
                return BadRequest(new ResponseMdl<bool> { IsSuccuss = false, Data = label, Message = "Label removal has been failed " });
            }
        }


        [HttpPut]
        [Authorize]
        [Route("UpdateLabel")]
        public IActionResult UpdateLabel(int NotesId, string NewLabelname,int LabelId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Label label = labelBuss.UpdateLabel(userId, LabelId, NewLabelname);

            if (label!=null)
            {
                return Ok(new ResponseMdl<Label> { IsSuccuss = true, Data = label, Message = "label renamed successfully " });
            }
            else
            {
                return BadRequest(new ResponseMdl<Label> { IsSuccuss = false, Data = label, Message = "Label removal has been failed " });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("AddLabelToNote")]
        public IActionResult AddLabelToNote([FromQuery]int LabelId,[FromQuery] int NotesId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            bool label = labelBuss.AddLabelToNote(userId, LabelId, NotesId);

            if (label)
            {
                return Ok(new ResponseMdl<bool> { IsSuccuss = true, Data = label, Message = "label added to note successfully " });
            }
            else
            {
                return BadRequest(new ResponseMdl<bool> { IsSuccuss = false, Data = label, Message = "Label added has been failed " });
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("RemoveLabel")]
        public IActionResult RemoveLabelFromNote(int LabelId, int NotesId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            bool label = labelBuss.RemoveLabelFromNote(userId, LabelId, NotesId);

            if (label)
            {
                return Ok(new ResponseMdl<bool> { IsSuccuss = true, Data = label, Message = "label added to note successfully " });
            }
            else
            {
                return BadRequest(new ResponseMdl<bool> { IsSuccuss = false, Data = label, Message = "Label added has been failed " });
            }
        }


        [Authorize]
        [HttpGet("GetNotesByLabel")]
        public IActionResult GetNotesByLabel(int labelId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            List<Notes> label = labelBuss.GetNotesByLabel(userId,labelId);

            if (label != null)
            {
                return Ok(new ResponseMdl<List<Notes>> { IsSuccuss = true, Data = label, Message = "Notes fetched successfully" });
            }
            else
            {
                return BadRequest(new ResponseMdl<List<Notes>> { IsSuccuss = false, Data = label, Message = "Failed to fetch Notes" });
            }
        }

    }
}
