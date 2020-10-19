using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace T5AN_Campeonato.Models
{
    public class Consulta
    {
        [DisplayName("CÓDIGO")]
        public int codigo { get; set; }
        [DisplayName("EQUIPO")]
        public String nombre { get; set; }
        [DisplayName("DISTRITO")]
        public String distrito { get; set; }
        [DisplayName("LOCALIDAD")]
        public String localidad { get; set; }
        [DisplayName("FECHA DE CREACIÓN")]
        public String fecha { get; set; }
        [DisplayName("AÑOS DE CREACIÓN")]
        public String años { get; set; }

    }
}
