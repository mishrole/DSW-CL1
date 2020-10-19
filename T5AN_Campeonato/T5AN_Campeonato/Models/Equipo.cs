using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T5AN_Campeonato.Models
{
    public class Equipo
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
        [DisplayName("FUNDADOR(A)")]
        public String fundacion { get; set; }

    }
}