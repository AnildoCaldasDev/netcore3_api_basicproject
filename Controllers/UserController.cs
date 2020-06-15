using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netcore3_api_basicproject.Data;
using netcore3_api_basicproject.Models;
using netcore3_api_basicproject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace netcore3_api_basicproject.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {

            if (!ModelState.IsValid)
                return BadRequest(model);

            model.Role = "Employee";

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();

                model.Password = "";

                return Ok(model);

            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar o usuario" });
            }
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.
                               Users.
                      AsNoTracking().
                      Where(x => x.Username.ToUpper() == model.Username.ToUpper() && x.Password.ToUpper() == model.Password)
                      .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos!" });

            var token = TokenService.GenerateToken(model);

            model.Password = "";

            return Ok(new { user = user, token = token });

        }



        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context) {

            var users = await context.Users.AsNoTracking().ToListAsync();

            return Ok(users);
        }


        // METHODS ONLY FOR TESTS:
        //[HttpGet]
        //[Route("Anonimo")]
        //[AllowAnonymous]
        //public string Anonimo() => "anônimo";

        //[HttpGet]
        //[Route("Authenticado")]
        //[Authorize]
        //public string Authenticado() => "authenticado";

        //[HttpGet]
        //[Route("Funcionario")]
        //[Authorize(Roles ="Employee")]
        //public string Funcionario() => "funcionario";

        //[HttpGet]
        //[Route("Gerente")]
        //[Authorize(Roles = "Manager")]
        //public string Gerente() => "gerente";
    }
}
