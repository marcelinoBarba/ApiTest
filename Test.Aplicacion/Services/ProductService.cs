using Microsoft.EntityFrameworkCore;
using Test.Aplicacion.Interfaces;
using Test.Dominio.Entities;
using Test.Infraestructura;

namespace Test.Aplicacion.Services
{
    public class ProductService : IGenericService<Product>
    {
        private readonly TenantDbContext _context;

        public ProductService(TenantDbContext context)
        {
            _context = context;
        }

        public Product? GetById(int id) => _context.Products.Where(w => w.Id == id).FirstOrDefault();

        public List<Product> GetAll() => _context.Products.ToList();

        public bool Add(Product product)
        {
            _context.Products.Add(product);
            return Convert.ToBoolean(_context.SaveChanges());
        }
        public bool Update(Product product)
        {
            _context.Products.Update(product);
            return Convert.ToBoolean(_context.SaveChanges());
        }
        public bool Delete(int Id)
        {
            var prd = _context.Products.Where(w => w.Id == Id).FirstOrDefault();
            if (prd == null)
            {
                return false;
            }

            _context.Products.Remove(prd);
            return Convert.ToBoolean(_context.SaveChanges());
        }
    }
}
