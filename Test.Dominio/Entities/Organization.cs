using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Dominio.Entities
{
    [Table("Organization")]
    public class Organization : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }
        public  string Name { get; set; }
        public  string SlugTenant { get; set; }
    }
}
