using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Test.Aplicacion.Services;
using Test.Dominio.DTO;
using Test.Dominio.Entities;
using Test.Infraestructura;

namespace Test.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext context, IConfiguration configuration )
        {
            _userService = new UserService(context);
            _configuration = configuration;
        }


        [HttpPost("Login")]
        public async Task<IResult> Login([FromBody] LoginDTO loginDTO )
        {

            if (!_userService.CheckCredentials(loginDTO.email, loginDTO.password))
            {
                return Results.Ok(new { message = "incorrect password or email" }); 
            }

            var user = _userService.GetByEmailAndPassword(loginDTO.email, loginDTO.password);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            var slugTenant = user.Organization.SlugTenant;

            return Results.Ok(new { AccessToken = jwt,  SlugTenant = slugTenant} );

        }

        [Authorize]
        [HttpGet("Users")]
        public IResult Users()
        {
            var users = _userService.GetAll();
            return Results.Ok(new { Users = users });

        }

        [Authorize]
        [HttpGet("UserById")]
        public IResult GetUserById(int UserId)
        {
            User user = _userService.GetById(UserId);
            return Results.Ok(new { User = user });

        }

        [Authorize]
        [HttpPost("Add")]
        public IResult AddUser([FromBody] UserDTO userDto)
        {

            User user = new User();
            user.Email = userDto.email;
            user.Password = userDto.password;
            user.IdOrganization = userDto.IdOrganization;

            bool result = _userService.Add(user);

            return Results.Ok(new { Result = result });

        }

        [Authorize]
        [HttpDelete("Delete")]
        public IResult DeleteUser([FromBody] int Id)
        {
            bool result = _userService.Delete(Id);
            return Results.Ok(new { Result = result });

        }

        [Authorize]
        [HttpPut("Update")]
        public IResult UpdateUser([FromBody] UserDTO userDto)
        {
            User user = _userService.GetById(userDto.Id);
            if (user == null)
            {
                return Results.Ok(new { Result = "incorrect id" });
            }

            user.Email = userDto.email;
            user.Password = userDto.password;
            user.IdOrganization = userDto.IdOrganization;

            bool result = _userService.Update(user);
            return Results.Ok(new { Result = result });

        }



    }
}
