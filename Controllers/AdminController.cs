using BlogBackend.DataContext;
using BlogBackend.DTO;
using BlogBackend.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BlogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IConfiguration _configuation;
        private SymmetricSecurityKey _key;

        public AdminController(BlogDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuation = configuration;
        }

        [HttpGet("getAllAdmins")]
        public async Task<ActionResult<Admin>> GetAdmins()
        {
            var list = await _context.Admins.ToListAsync();
            return Ok(list);
        }

        [HttpPost("addAdmin")]
        public async Task<ActionResult<Admin>> AddAdmin(adminDTO adminObj)
        {
            var check = await _context.Admins.FirstOrDefaultAsync(x=>x.Email==adminObj.Email);
            if (check != null)
                return StatusCode((int)HttpStatusCode.Ambiguous, "Email already registered");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(adminObj.Password);
            adminObj.Password = passwordHash;

            var AddAdmin = new Admin
            {
                Email = adminObj.Email,
                Password = adminObj.Password,
                Name = adminObj.Name,
                IsMaster = adminObj.IsMaster,
            };

            _context.Admins.Add(AddAdmin);
            _context.SaveChanges();
            return Ok( new { message = "Admin added successfully" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> AdminLogin(loginDTO loginObj)
        {
            var check = await _context.Admins.FirstOrDefaultAsync(x => x.Email == loginObj.Email);
            if (check == null)
                return NotFound("User not found");

            if(!BCrypt.Net.BCrypt.Verify(loginObj.Password, check.Password))
                return StatusCode((int)HttpStatusCode.BadRequest, "Incorrect password");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, loginObj.Email),
                new Claim(JwtRegisteredClaimNames.Name, check.Name),
                new Claim(JwtRegisteredClaimNames.Typ, check.IsMaster.ToString(), ClaimValueTypes.Boolean)
            };

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuation["TokenKey"]));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires= DateTime.UtcNow.AddMinutes(30),
                SigningCredentials=creds,
                Issuer = _configuation["ValidIssuer"],
                Audience= _configuation["ValidAudience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            var AuthModel = new
            {
                token = tokenHandler.WriteToken(token),
                valid = token.ValidTo,
                name = check.Name,
                email = check.Email,
                role = check.IsMaster

            };
            return Ok(AuthModel);

        }

        [HttpDelete("deleteAdmin/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var user = await _context.Admins.FindAsync(id);

            if (user != null)
            {
                _context.Admins.Remove(user);
                _context.SaveChanges();
                return Ok(user);
            }

            return BadRequest("Admin does not exist");

        }
    }
}
