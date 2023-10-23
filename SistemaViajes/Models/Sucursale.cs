﻿using System;
using System.Collections.Generic;

namespace SistemaViajes.Models;

public partial class Sucursale
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string BarrioColonia { get; set; } = null!;

    public virtual ICollection<ColaboradorSucursal> ColaboradorSucursals { get; set; } = new List<ColaboradorSucursal>();

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
