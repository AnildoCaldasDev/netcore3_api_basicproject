﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using netcore3_api_basicproject.Data;
using netcore3_api_basicproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore3_api_basicproject.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {

            var products = await context.
                                   Products.
                                   Include(x => x.Category).
                                   AsNoTracking().
                                   ToListAsync();
            return Ok(products);

        }


        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.
                                   Products.
                                   Include(x => x.Category).
                                   AsNoTracking().
                                   FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado!" });


            return Ok(product);
        }


        [HttpGet]//products/categories/1 -----> trará todos os produtos da categoria 1
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.
                                   Products.
                                   Include(x => x.Category).
                                   AsNoTracking().
                                   Where(x => x.CategoryId == id).
                                   ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel adicionar o produto" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Product>> Put(int id, [FromBody] Product model, [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { message = "Produto não encontrado" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este produto já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar o produto" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Product>> Delete(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este produto já foi removido" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover o produto" });
            }
        }
    }
}
