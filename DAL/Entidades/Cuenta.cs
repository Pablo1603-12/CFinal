using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Cuenta
{
    public long CuentaId { get; set; }
    public string NumeroCuenta { get; set; }
    public string TipoCuenta { get; set; }
    public decimal SaldoInicial { get; set; }
    public object Usuario { get; internal set; }
    public List<Movimiento> movimientos { get; set; }



}
