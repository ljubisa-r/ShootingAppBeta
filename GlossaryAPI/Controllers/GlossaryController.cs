using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;
using GlossaryAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlossaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {
        //static private List<GlossaryTerm> glossaryTerms = new List<GlossaryTerm>
        //{
        //    new GlossaryTerm { Id = 1, Term = "Term 1", Definition = "Published - Definition 1 of term need to be more than 30 chars", CreatedBy = "Admin", Status = ItemStatus.Published },
        //    new GlossaryTerm { Id = 2, Term = "Term 2", Definition = "Published - Definition 2 of term need to be more than 30 chars", CreatedBy = "Admin", Status = ItemStatus.Published },
        //    new GlossaryTerm { Id = 3, Term = "Term 3", Definition = "Draft - Definition 3 of term need to be more than 30 chars", CreatedBy = "User", Status =ItemStatus.Draft },
        //    new GlossaryTerm { Id = 4, Term = "Term 4", Definition = "Archived - Definition 4 of term need to be more than 30 chars", CreatedBy = "User", Status =ItemStatus.Archived }

        //};
        
        private readonly IGlossaryService _glossaryService;
        public GlossaryController(IGlossaryService glossaryService)
        {
            _glossaryService = glossaryService;
        }

        [HttpGet]
        public ActionResult<List<GlossaryTermDTO>> GetAllTerms()
        {
            try
            {
                var terms = _glossaryService.GetAllTerms();
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

                var createdTerm = _glossaryService.CreateTerm(newTerm);
                return CreatedAtAction(nameof(GetTermById), new { id = createdTerm.Id }, createdTerm);
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
                var result = _glossaryService.UpdateTerm(updatedTerm);
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
        public IActionResult ArchiveTerm(int id, User user)
        {
            try
            {
                //var user = HttpContext.Items["User"] as User;
                if (user == null)
                    return Unauthorized();

                var result = _glossaryService.ArchiveTerm(id, user.Username);
                
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
        public IActionResult PublishTerm(GlossaryTermDTO updatedTerm)
        {
            try
            {
                var user = HttpContext.Items["User"] as User; // User from token
                if (user == null)
                    return Unauthorized();

                var result = _glossaryService.PublishTerm(updatedTerm);

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

        [HttpDelete("{id}")]
        public IActionResult DeleteTerm(int id)
        {
            try
            {
                _glossaryService.DeleteTerm(id);
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
    }
}
