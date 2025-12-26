using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.DTOs;
using ModalLayer.Entities;

namespace FundooApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabaratorController : ControllerBase
    {
        private readonly ICollabBuss collabBuss;

        

        public CollabaratorController(ICollabBuss collabBuss)
        {
            this.collabBuss = collabBuss;
        }

        [Authorize]
        [HttpPost]
        [Route("AddCollaborator")]
        public IActionResult AddCollaborator(string Email,int notesId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Collaborator collaborator= collabBuss.AddCollaborator(Email, notesId,userId);

            if (collaborator != null)
            {
                return Ok(new ResponseMdl<Collaborator> { Data = collaborator, IsSuccuss = true, Message = "Collaborator Added" });
            }
            else
            {
                return BadRequest(new ResponseMdl<Collaborator> { Data = collaborator, IsSuccuss = false, Message = "Unable to Add Collaborator" });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("RemoveCollaborator")]
        public IActionResult RemoveCollaborator(string Email, int notesId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            bool collaborator = collabBuss.RemoveCollaborator(Email, notesId, userId);

            if (collaborator)
            {
                return Ok(new ResponseMdl<bool> { Data = collaborator, IsSuccuss = true, Message = "Collaborator has been Removed " });
            }
            else
            {
                return BadRequest(new ResponseMdl<bool> { Data = collaborator, IsSuccuss = false, Message = "Unable to remove Collaborator" });
            }
        }
    }
}
