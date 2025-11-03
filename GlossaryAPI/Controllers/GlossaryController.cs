using GlossaryAPI.DTOs;
using GlossaryAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlossaryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {
                
        private readonly IGlossaryService _glossaryService;
        public GlossaryController(IGlossaryService glossaryService)
        {
            _glossaryService = glossaryService;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<GlossaryTermDTO>> GetAllTerms()
        {
            try
            {
               var terms = _glossaryService.GetAllTermsWithUser();

                return Ok(terms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]       
        public ActionResult<GlossaryTermDTO> GetTermById(int id)
        {
            try
            {
                var term = _glossaryService.GetTermById(id);
                if (term == null)
                {
                    return NotFound();
                }
                return Ok(term);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<GlossaryTermDTO> CreateTerm(GlossaryTermDTO newTerm)
        {
            try
            {
                if (newTerm == null)
                    return BadRequest("Term data is required");
                int userId = GetUserId();

                var createdTerm = _glossaryService.CreateTerm(newTerm, userId);
                
                return CreatedAtAction(nameof(GetTermById), new { id = createdTerm.id }, createdTerm);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateTerm(GlossaryTermDTO updatedTerm)
        {
            try
            {
                int userId = GetUserId();
                var result = _glossaryService.UpdateTerm(updatedTerm, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public IActionResult ArchiveTerm(int id)
        {
            try
            {
                int userId = GetUserId();
                var result = _glossaryService.ArchiveTerm(id, userId);
                
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("publish")]
        [Authorize(Roles = "Publisher")]
        public IActionResult PublishTerm(GlossaryTermDTO updatedTerm)
        {
            try
            {
                int userId = GetUserId();
                var result = _glossaryService.PublishTerm(updatedTerm, userId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTerm(int id)
        {
            try
            {
                int userId = GetUserId();
                _glossaryService.DeleteTerm(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) throw new UnauthorizedAccessException("User ID not found in token");
            return int.Parse(userIdClaim);
        }
    }
}
