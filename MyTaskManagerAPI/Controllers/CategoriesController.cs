using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTaskManagerAPI.Data;
using MyTaskManagerAPI.Models;

namespace MyTaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly MyTaskManagerContext _context;

        public CategoriesController(MyTaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
        .Select(c => new { c.CategoryId, c.Name, c.Description })  // Only select required fields
        .ToListAsync(); ;
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryId }, new { category.CategoryId, category.Name, category.Description });
        }
        
        // DELETE: api/categories/{categoryId}
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            // Find category
            var category = await _context.Categories
                                         .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null) return NotFound();

            // Delete all tasks related to this category
            var tasks = await _context.Tasks
                                      .Where(t => t.CategoryId == categoryId)
                                      .ToListAsync();

            if (tasks.Any())
                _context.Tasks.RemoveRange(tasks);

            // Delete category
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
