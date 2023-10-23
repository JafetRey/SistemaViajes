using System;
using System.Collections.Generic;

namespace SistemaViajes.Models;

public partial class Viaje
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int? ColaboradorId { get; set; }

    public int? SucursalId { get; set; }

    public int? TransportistaId { get; set; }

    public int? UsuarioRegistradorId { get; set; }

    public double DistanciaKilometros { get; set; }

    public virtual Colaboradore? Colaborador { get; set; }

    public virtual Sucursale? Sucursal { get; set; }

    public virtual Transportista? Transportista { get; set; }

    public virtual Usuario? UsuarioRegistrador { get; set; }
}
