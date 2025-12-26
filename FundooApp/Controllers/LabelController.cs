using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.DTOs;
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
        public IActionResult AddLabel(int NotesId, string LabelName)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Label label=labelBuss.AddLabel(NotesId,userId, LabelName);

            if (label != null)
            {
                return Ok(new ResponseMdl<Label> { IsSuccuss = true, Data = label, Message = "Label added to the notes " });
            }
            else
            {
                return BadRequest(new ResponseMdl<Label> { IsSuccuss = false, Data = label, Message = "Adding Label to Notes is Failed" });
            }
        }


        [HttpPut]
        [Authorize]
        [Route("GetLabel")]
        public IActionResult FetchLabel(int NotesId, string LabelName)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Label label = labelBuss.FetchLabel(NotesId, userId, LabelName);

            if (label != null)
            {
                return Ok(new ResponseMdl<Label> { IsSuccuss = true, Data = label, Message = "Label fetched successfully" });
            }
            else
            {
                return BadRequest(new ResponseMdl<Label> { IsSuccuss = false, Data = label, Message = "Failed to fetch label" });
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("RemoveLabel")]
        public IActionResult RemoveLabel(int NotesId, string LabelName)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            bool label = labelBuss.RemoveLabel(NotesId, userId, LabelName);

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
        [Route("Renamelabel")]
        public IActionResult RenameLabel(int NotesId, string NewLabelname,int LabelId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Label label = labelBuss.RenameLabel(NotesId, userId, NewLabelname,LabelId);

            if (label!=null)
            {
                return Ok(new ResponseMdl<Label> { IsSuccuss = true, Data = label, Message = "label renamed successfully " });
            }
            else
            {
                return BadRequest(new ResponseMdl<Label> { IsSuccuss = false, Data = label, Message = "Label removal has been failed " });
            }
        }
    }
}
