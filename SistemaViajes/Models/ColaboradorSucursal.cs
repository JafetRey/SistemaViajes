using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaViajes.Models
{
    public partial class ColaboradorSucursal
    {
        public int ColaboradorId { get; set; }

        public int SucursalId { get; set; }

        [Required(ErrorMessage = "La distancia es obligatoria.")]
        [Range(0, 50, ErrorMessage = "La distancia debe estar entre 0 y 50 kilómetros.")]
        public double DistanciaKilometros { get; set; }

        public virtual Colaboradore Colaborador { get; set; } = null!;

        public virtual Sucursale Sucursal { get; set; } = null!;
    }
}
