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

    public class ListsController : ControllerBase {

        private readonly TrelloContext _context;

        public ListsController(TrelloContext context) {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListDTO>> GetOne(int id) {
            var list = await _context.Lists.Where(l => l.Id == id).FirstOrDefaultAsync();
            if (list == null)
                return NotFound();
            return list.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ListDTO>> PostList(int id, ListDTO data){
            var list = await _context.Lists.FindAsync(data.Id);
            if (list != null) {
                var err = new ValidationErrors().Add("List already in use", nameof(list.Name));
                return BadRequest(err);
            }
            var newList = new List(){
                Name = data.Name,
                BoardId = id
            };

            _context.Lists.Add(newList);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);


            return CreatedAtAction(nameof(GetOne), new { name = newList.Name }, newList.ToDTO());
        }
    }
}