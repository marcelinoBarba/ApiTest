using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Aplicacion.Services;
using Test.Dominio.Entities;
using Test.Infraestructura;

namespace Test.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class ProductController : Controller
    {
        private ProductService _productService;

        [HttpGet("Products")]
        public IResult GetProducts()
        {
            var _context = HttpContext.Items["DbContext"] as TenantDbContext;
            if (_context == null)
            {
                return Results.BadRequest();
            }
            _productService = new ProductService(_context);

            var products = _productService.GetAll();
            return Results.Ok(new { Products = products });

        }

        [HttpGet("ProductById")]
        public IResult GetProductById( int productId)
        {
            var _context = HttpContext.Items["DbContext"] as TenantDbContext;
            if (_context == null)
            {
                return Results.BadRequest();
            }
            _productService = new ProductService(_context);
            Product product = _productService.GetById(productId);

            return Results.Ok(new { Product = product });

        }


        [HttpPost("Add")]
        public IResult AddProduct([FromBody] Product product)
        {
            var _context = HttpContext.Items["DbContext"] as TenantDbContext;
            if (_context == null)
            {
                return Results.BadRequest();
            }
            _productService = new ProductService(_context);
            bool result = _productService.Add(product);
            
            return Results.Ok(new { Result = result });

        }

        [HttpDelete("Delete")]
        public IResult DeleteProduct([FromBody] int Id)
        {
            var _context = HttpContext.Items["DbContext"] as TenantDbContext;
            if (_context == null)
            {
                return Results.BadRequest();
            }
            _productService = new ProductService(_context);
            bool result = _productService.Delete(Id);

            return Results.Ok(new { Result = result });

        }

        [HttpPut("Update")]
        public IResult UpdateProduct([FromBody] Product product)
        {
            var _context = HttpContext.Items["DbContext"] as TenantDbContext;
            if (_context == null)
            {
                return Results.BadRequest();
            }
            _productService = new ProductService(_context);
            bool result = _productService.Update(product);

            return Results.Ok(new { Result = result });

        }

    }
}
