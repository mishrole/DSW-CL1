using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T5AN_Campeonato.Models
{
    public class Equipo_
    {
        [DisplayName("CÓDIGO")]
        public int codigo { get; set; }

        [DisplayName("EQUIPO")]
        [Required(ErrorMessage = "Ingrese nombre de Equipo")]
        public String nombre { get; set; }

        [DisplayName("DISTRITO")]
        public int distrito { get; set; }

        [DisplayName("LOCALIDAD")]
        [Required(ErrorMessage = "Ingrese nombre de Localidad")]
        public String localidad { get; set; }

        [DisplayName("FECHA DE CREACIÓN")]
        [Required(ErrorMessage = "Ingrese Fecha de Creación")]
        public String fecha { get; set; }

        [DisplayName("FUNDADOR(A)")]
        [Required(ErrorMessage = "Ingrese nombre de Fundador(a)")]
        public String fundacion { get; set; }
    }
}