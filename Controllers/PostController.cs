using BlogBackend.DataContext;
using BlogBackend.DTO;
using BlogBackend.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogDbContext _context;
        public PostController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllPosts")]
        public async Task<ActionResult<Post>> GetPosts()
        {
            var list = await _context.Posts.ToListAsync();
            return Ok(list);
        }

        [HttpPost("addPost")]
        public async Task<ActionResult<Post>> AddPost(addPostDTO addPostObj)
        {
            var check = await _context.Posts.FirstOrDefaultAsync(x => x.Title == addPostObj.Title);
            if (check != null)
                return BadRequest("Title is already taken");

            var postToAdd = new Post
            {
                Title= addPostObj.Title,
                AuthorName= addPostObj.AuthorName, 
                AuthorRole= addPostObj.AuthorRole,
                CategoryId = addPostObj.CategoryId,
                CategoryName = addPostObj.CategoryName,
                content = addPostObj.Content,
                CreatedAt = addPostObj.CreatedAt,
                ImgPath = addPostObj.ImgPath,
                IsFeatured = false,
                Permalink = addPostObj.Permalink,
                Subtitle = addPostObj.Subtitle,
                Views=0,

            };

            _context.Posts.Add(postToAdd);
            _context.SaveChanges();
            return Ok(postToAdd);

        }

        [HttpDelete("deletePost/{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var check = await _context.Posts.FindAsync(id);
            if (check == null)
                return BadRequest("Post not present");

            _context.Posts.Remove(check);
            _context.SaveChanges();
            return Ok(new { message = "Deleted successfully" });


        }

        [HttpPut("editPost/{id}")]
        public async Task<ActionResult<Post>> EditPost(int id, addPostDTO addpostObj)
        {
            var check = await _context.Posts.FindAsync(id);
            if (check == null)
                return NotFound("Post not found");

            check.Title = addpostObj.Title;
            check.Subtitle=addpostObj.Subtitle;
            check.Permalink = addpostObj.Permalink;
            check.ImgPath = addpostObj.ImgPath;
            check.CategoryId = addpostObj.CategoryId;
            check.CategoryName = addpostObj.CategoryName;
            check.content = addpostObj.Content;
            check.AuthorName = addpostObj.AuthorName;
            check.AuthorRole = addpostObj.AuthorRole;
            check.CreatedAt = addpostObj.CreatedAt;

            _context.SaveChanges();
            return Ok(addpostObj);
        }

        [HttpPut("addView/{id}")]
        public async Task<ActionResult<Post>> AddView(int id) 
        { 
            var check = await _context.Posts.FindAsync(id);
            if (check == null)
                return NotFound("Post not found");

            check.Views++;

            _context.SaveChanges();
            return Ok(check);
        }

        [HttpGet("getAllViews")]
        public async Task<ActionResult> GetAllViews()
        {
            var views =await _context.Posts.SumAsync(post => post.Views);
            return Ok(views);
        }

        [HttpGet("getCategoryViews")]
        public async Task<ActionResult> GetCategoryViews()
        {
            

            var views =await _context.Posts.
                GroupBy(x => new {x.CategoryId, x.CategoryName}).
                Select(y => new CategoryViewDTO
            {
                CategoryId = y.Key.CategoryId,
                CategoryName= y.Key.CategoryName,
                TotalViews = y.Sum(x => x.Views),
                
            }).ToListAsync();
                
            return Ok(views);
        }
        
        [HttpGet("getSinglePost/{id}")]

        public async Task<ActionResult<Post>> GetSinglePost(int id)
        {
            var check = await _context.Posts.FindAsync(id);
            if (check == null)
                return NotFound("Post not found");

            return Ok(check);
        }

        [HttpGet("getLatestPosts")]

        public async Task<ActionResult<List<Post>>> GetLatestPosts()
        {
            var list = await _context.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return Ok(list);
        }

        [HttpGet("getByCategory/{id}")]

        public async Task<ActionResult<List<Post>>> GetByCategory(int id)
        {
            var check= await _context.Posts.Where(x=> x.CategoryId == id).ToListAsync();
            return Ok(check);
        }

        [HttpGet("getCount")]

        public async Task<ActionResult> GetCount()
        {
            var count = await _context.Posts.CountAsync();
            return Ok(count);
        }

        [HttpGet("getPostsByViews")]

        public async Task<ActionResult<List<Post>>> GetPostsByViews()
        {
            var posts = await _context.Posts.OrderByDescending(x => x.Views).ToListAsync();
            return Ok(posts);
        }
    }
}
