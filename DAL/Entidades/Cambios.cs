using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Cambios
{
    public long Id { get; set; }
    public required string Titulo { get; set; }
    public required string Description { get; set; }
    public bool Publicado { get; set; }
    public required ICollection<Cuenta> Cuentas { get; set; }




}
