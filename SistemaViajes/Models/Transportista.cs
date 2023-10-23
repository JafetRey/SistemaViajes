using System;
using System.Collections.Generic;

namespace SistemaViajes.Models;

public partial class Transportista
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal TarifaPorKilometro { get; set; }

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
