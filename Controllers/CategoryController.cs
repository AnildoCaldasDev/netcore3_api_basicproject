using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using netcore3_api_basicproject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace netcore3_api_basicproject.Controllers
{

    //endpoints:
    //http://localhost:5000
    //htpps://localhost:50001



    //[Route("api/[controller]")]
    //[ApiController]

    [Route("categories")]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return new List<Category>();
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Get(int id) {
            return new Category() { Id = id, Title = "teste" };
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Post([FromBody] Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(model);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Put(int id, [FromBody]Category model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Id == id)
                 return Ok(model);

            return NotFound();
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Delete(int id)
        {
            return Ok();
        }
    }
}
