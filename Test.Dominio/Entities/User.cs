using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Dominio.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }


        [Required]
        public int IdOrganization { get; set; }

        [ForeignKey("IdOrganization")]
        public virtual Organization Organization { get; set; }

    }
}
