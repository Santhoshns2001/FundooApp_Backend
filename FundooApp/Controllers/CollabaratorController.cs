using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.DTOs;
using ModalLayer.DTOs.Collaborator;
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
        public IActionResult AddCollaborator([FromBody] AddCollaboratorRequest request)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            Collaborator collaborator= collabBuss.AddCollaborator(request.Email, request.NotesId, userId);

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
        [HttpGet]
        [Route("GetCollaborator")]
        public IActionResult GetCollaborator(int notesId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            List<Collaborator> collaborators = collabBuss.GetCollaborator( notesId, userId);

            if (collaborators != null)
            {
                return Ok(new ResponseMdl<List<Collaborator>> { Data = collaborators, IsSuccuss = true, Message = "Collaborator Fetched" });
            }
            else
            {
                return BadRequest(new ResponseMdl<List<Collaborator>> { Data = collaborators, IsSuccuss = false, Message = "Unable to Add Collaborator" });
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("RemoveCollaboratorByEmail")]
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

        [Authorize]
        [HttpDelete]
        [Route("RemoveCollaborator")]
        public IActionResult RemoveCollaborator(int CollabId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);

            bool collaborator = collabBuss.RemoveCollaboratorByID(CollabId, userId);

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
