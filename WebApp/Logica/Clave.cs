using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contratos;

namespace Logica
{
    public class Clave
    {
        public string Email { get; set; }
        public string Contrasena  { get; set; }
        public Roles[] Roles { get; set; }
    }
}
