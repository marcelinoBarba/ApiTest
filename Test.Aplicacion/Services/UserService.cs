using Microsoft.EntityFrameworkCore;
using Test.Aplicacion.Interfaces;
using Test.Dominio.Entities;
using Test.Infraestructura;

namespace Test.Aplicacion.Services
{
    public class UserService : IGenericService<User>
    {

        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CheckCredentials (string email, string password)
        {
            var user =  _context.Users.Where(w => w.Password == password && w.Password == password).FirstOrDefault();
            return (user != null) ? true : false;

        }

        public User? GetById(int id) => _context.Users.Include("Organization").Where(w => w.Id == id).FirstOrDefault();

        public User? GetByEmailAndPassword(string email, string password)
            => _context.Users
            .Include("Organization")
            .Where(w => w.Email == email && w.Password == password).FirstOrDefault();

        public List<User> GetAll() => _context.Users.ToList();

        public bool Add(User user)
        {
            _context.Users.Add(user);
            return Convert.ToBoolean(_context.SaveChanges());
        }
        public bool Update(User user)
        {
            _context.Users.Update(user);
            return Convert.ToBoolean(_context.SaveChanges());
        }
        public bool Delete(int Id)
        {
            var usr = _context.Users.Where(w => w.Id == Id).FirstOrDefault();
            if (usr == null)
            {
                return false;
            }

            _context.Users.Remove(usr);
            return Convert.ToBoolean(_context.SaveChanges());
        }

    }
}

