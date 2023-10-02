using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Aplicacion.Services;
using Test.Dominio.Entities;
using Test.Infraestructura;

namespace Test.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : Controller
    {
        private readonly OrganizationService _organizationService;

        public OrganizationController(ApplicationDbContext context)
        {
            _organizationService = new OrganizationService(context);
        }

        [HttpGet("Organizations")]
        public IResult Organizations()
        {
            var users = _organizationService.GetAll();
            return Results.Ok(new { Users = users });

        }

        [HttpGet("OrganizationById")]
        public IResult GetUserById(int Id)
        {
            Organization org = _organizationService.GetById(Id);
            return Results.Ok(new { User = org });

        }

        [HttpPost("Add")]
        public IResult AddOrganization([FromBody] Organization organization)
        {

            bool result = _organizationService.Add(organization);
            return Results.Ok(new { Result = result });

        }

        [HttpDelete("Delete")]
        public IResult DeleteOrganization([FromBody] int Id)
        {

            bool result = _organizationService.Delete(Id);
            return Results.Ok(new { Result = result });

        }

        [HttpPut("Update")]
        public IResult UpdateOrganization([FromBody] Organization organization)
        {

            bool result = _organizationService.Update(organization);
            return Results.Ok(new { Result = result });

        }

    }
}
