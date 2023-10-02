using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Dominio.Entities
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }
        public  string? Name { get; set; }

    }
}
