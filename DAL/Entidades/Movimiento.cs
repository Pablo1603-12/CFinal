using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Movimiento
{
    public long MovimientoId { get; set; }
    public string Tipo { get; set; }
    public decimal Saldo { get; set; }
    public decimal Valor { get; set; }
    public DateTime Fecha { get; set; }
    public string Descripcion { get; set; }
    public long CuentaId { get; set; }
    public Cuenta Cuenta { get; set; }
    public required ICollection<Cambios> Cambios { get; set; }


}
