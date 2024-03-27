using BlogBackend.DataContext;
using BlogBackend.DTO;
using BlogBackend.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public CategoryController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllCategories")]
        public async Task<ActionResult<Category>> GetCategories() 
        {
            var list = await _context.Categories.ToListAsync();
            return Ok(list);
        }


        [HttpPost("addCategory")]

        public async Task<ActionResult> AddCategory2(categoryDTO categoryObj)
        {
            var check = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == categoryObj.CategoryName);
            if (check != null)
                return StatusCode((int)HttpStatusCode.Ambiguous, "Category already exists!");


            var cattoadd = new Category

            {
                CategoryName = categoryObj.CategoryName,
                Status = categoryObj.Status,
            };

            _context.Categories.Add(cattoadd);
            _context.SaveChanges();

            return StatusCode((int)HttpStatusCode.OK, new { message = "Added" });
        }

        [HttpGet("getCategoryById/{id}")]
        public async Task<ActionResult<Category>> GetById(int id) 
        {
            var category = await _context.Categories.FindAsync(id);
            if(category != null)
                return Ok(category);
            return NotFound();
        }


        [HttpDelete("deleteCategory/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var check= await _context.Categories.FindAsync(id);
            if(check == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "Category not found");
            }
            _context.Categories.Remove(check);
            _context.SaveChanges();
            return Ok( new { message = "Deleted Sucessfully" });
        }

        [HttpPut("editCategory/{id}")]
        public async Task<ActionResult<categoryDTO>> EditCategory(int id, [FromBody] categoryDTO categoryobj)
        {
            var check = await _context.Categories.FindAsync(id);
            if(check == null) {
                return StatusCode((int)HttpStatusCode.NotFound, "Category not found");
            }

            check.Status = categoryobj.Status;  
            check.CategoryName = categoryobj.CategoryName;
            _context.SaveChanges();
            return Ok(new { message = "Changes made successfully" });
        }
    }
}
