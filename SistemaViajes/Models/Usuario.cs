using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaViajes.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    [Range(1, 4)]
    public int? RolId { get; set; }

    public virtual Rol? Rol { get; set; }

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
