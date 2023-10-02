using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Aplicacion.Interfaces;
using Test.Dominio.Entities;
using Test.Infraestructura;

namespace Test.Aplicacion.Services
{
    public class OrganizationService : IGenericService<Organization>
    {
        private readonly ApplicationDbContext _context;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Organization? GetById(int id) =>  _context.Organizations.Where(w => w.Id == id).FirstOrDefault();

        public  List<Organization> GetAll() =>  _context.Organizations.ToList();

        public bool Add(Organization organization)
        {
            _context.Organizations.Add(organization);
            return Convert.ToBoolean(_context.SaveChanges());  
        }
        public bool Update(Organization organization)
        {
            _context.Organizations.Update(organization);
            return Convert.ToBoolean(_context.SaveChanges());
        }
        public bool Delete(int Id)
        {
            var users = _context.Users.Where(w => w.IdOrganization == Id).ToList();
            if (users.Count > 0)
            {
                return false;
            }

            var org = _context.Organizations.Where(w => w.Id == Id).FirstOrDefault();
            if (org == null)
            {
                return false;
            }

            _context.Organizations.Remove(org);
            return Convert.ToBoolean(_context.SaveChanges());
        }
    }
}
