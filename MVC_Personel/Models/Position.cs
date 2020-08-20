using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Personel.Models
{
    public class Position
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Authority { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
