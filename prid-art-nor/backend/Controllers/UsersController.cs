using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid_2021_A08.Models;
using System.ComponentModel.DataAnnotations;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Security.Claims;
using prid_2021_A08.Helpers;

namespace prid_2021_A08.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly TrelloContext _context;


        public UsersController(TrelloContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll() {
            return (await _context.Users.ToListAsync()).ToDTO();
        }
        
        [AllowAnonymous]
        [HttpGet("{pseudo}")]
        public async Task<ActionResult<UserDTO>> GetOne(string pseudo) {
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            return user.ToDTO();
        }

        [HttpGet("userId/{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id) {
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            return user.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO data) {
            var user = await _context.Users.FindAsync(data.Id);
            if (user != null) {
                var err = new ValidationErrors().Add("Pseudo already in use", nameof(user.Pseudo));
                return BadRequest(err);
            }
            var newUser = new User() {
                Pseudo = data.Pseudo,
                Password = data.Password,
                Email = data.Email,
                LastName = data.LastName,
                FirstName = data.FirstName,
                BirthDate = data.BirthDate,
                Role = data.Role
            };

            _context.Users.Add(newUser);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);

            return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());
        }

        [Authorized(Role.Admin)]
        [HttpPut("{pseudo}")]
        public async Task<IActionResult> PutUser(string pseudo, UserDTO userDTO) {
            if (pseudo != userDTO.Pseudo)
                return BadRequest();

            //var user = await _context.Users.FindAsync(userDTO.Id);
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).FirstOrDefaultAsync();

            if (user == null)
                return NotFound();
            if (userDTO.Password != null)
                user.Password = userDTO.Password;
            //user.Pseudo = userDTO.Pseudo;
            user.Email = userDTO.Email;
            user.LastName = userDTO.LastName;
            user.FirstName = userDTO.FirstName;
            user.BirthDate = userDTO.BirthDate;
            user.Role = userDTO.Role;
            
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);

            return NoContent();
        }

        [Authorized(Role.Admin)]
        [HttpDelete("{pseudo}")]
        public async Task<IActionResult> DeleteUser(string pseudo) {
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("available/{pseudo}")]
        public async Task<ActionResult<bool>> IsAvailable(string pseudo) {
            var user = await _context.Users.FindAsync(pseudo);
            return user == null;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult<UserDTO>> SignUp(UserDTO data) {
            return await PostUser(data);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> Authenticate(UserDTO data) {
            var user = await Authenticate(data.Pseudo, data.Password);

            if (user == null)
                return BadRequest(new ValidationErrors().Add("User not found", "Pseudo"));
            if (user.Token == null)
                return BadRequest(new ValidationErrors().Add("Incorrect password", "Password"));

            return Ok(user);
        }

        private async Task<User> Authenticate(string pseudo, string password) {
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).FirstOrDefaultAsync();

            // return null if member not found
            if (user == null)
                return null;

            if (user.Password == password) {
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("my-super-secret-key");
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity(new Claim[]
                                                 {
                                             new Claim(ClaimTypes.Name, user.Pseudo),
                                             new Claim(ClaimTypes.Role, user.Role.ToString())
                                                 }),
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(10000),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
            }

            // remove password before returning
            user.Password = null;

            return user;
        }

    }
}
