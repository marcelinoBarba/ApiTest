using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Dominio.Interfaces;

namespace Test.Dominio.Entities
{
    public class BaseEntity: IEntity
    {
        public int Id { get; set; }
    }
}
