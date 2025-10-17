using Microsoft.AspNetCore.Mvc;
using GlossaryAPI.Models;

namespace GlossaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {

        static private List<GlossaryTerm> glossaryTerms = new List<GlossaryTerm>
        {
            new GlossaryTerm { Id = 1, Term = "Term 1", Definition = "Published - Definition 1 of term need to be more than 30 chars", CreatedBy = "Admin", Status = ItemStatus.Published },
            new GlossaryTerm { Id = 2, Term = "Term 2", Definition = "Published - Definition 2 of term need to be more than 30 chars", CreatedBy = "Admin", Status = ItemStatus.Published },
            new GlossaryTerm { Id = 3, Term = "Term 3", Definition = "Draft - Definition 3 of term need to be more than 30 chars", CreatedBy = "User", Status =ItemStatus.Draft },
            new GlossaryTerm { Id = 4, Term = "Term 4", Definition = "Archived - Definition 4 of term need to be more than 30 chars", CreatedBy = "User", Status =ItemStatus.Archived }

        };


        [HttpGet]
        public ActionResult<List<GlossaryTerm>> GetAllTerms()
        {
            return Ok(glossaryTerms);
        }
        [HttpGet("{id}")]
        public ActionResult<GlossaryTerm> GetTermById(int id)
        {
            var term = glossaryTerms.FirstOrDefault(t => t.Id == id);
            if (term == null)
            {
                return NotFound();
            }
            return Ok(term);
        }


        [HttpPost]
        public ActionResult<GlossaryTerm> CreateTerm(GlossaryTerm newTerm)
        {
            if (newTerm == null)
                return BadRequest();

            glossaryTerms.Add(newTerm); // call action for Add term changes to database if applicable
            return CreatedAtAction(nameof(GetTermById), new { id = newTerm.Id }, newTerm);
        }

        [HttpPut]
        public IActionResult UpdateTerm(GlossaryTerm updatedTerm)
        {
            var existingTerm = glossaryTerms.FirstOrDefault(t => t.Id == updatedTerm.Id);
            if (existingTerm == null)
            {
                return NotFound();
            }
            existingTerm.Term = updatedTerm.Term;
            existingTerm.Definition = updatedTerm.Definition;
            existingTerm.CreatedBy = updatedTerm.CreatedBy;
            existingTerm.Status = updatedTerm.Status;

            // call action for Save term changes to database if applicable
            return NoContent();
        }


        [HttpPut]
        public IActionResult ArchiveTerm(int id)
        {
            var existingTerm = glossaryTerms.FirstOrDefault(t => t.Id == id);
            if (existingTerm == null)
            {
                return NotFound();
            }
            var user = HttpContext.Items["User"] as User;  // Assume user is set in middleware

            if (existingTerm.CreatedBy != user.Username)
                return Forbid();

            existingTerm.Status = ItemStatus.Archived;
            // call action for Save term changes to database if applicable
            return NoContent();
        }

            [HttpDelete("{id}")]
        public IActionResult DeleteTerm(int id)
        {
            var term = glossaryTerms.FirstOrDefault(t => t.Id == id);
            if (term == null)            
                return NotFound();
            
            if (term.Status != 0)
                    return Forbid();

            glossaryTerms.Remove(term); // call action for Delete term changes form database if applicable
            return NoContent();
        }
    }
}
