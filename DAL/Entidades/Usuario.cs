using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Usuario
{
    public bool CuentaConfirmada { get; set; }

    public DateTime? FchExpiracionToken { get; set; }

    public DateTime? FchRegistro { get; set; }

    public long IdUsuario { get; set; }

    public string TlfMovil { get; set; } = null!;

    public string? Rol { get; set; }

    public string NombreApellidos { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? Foto { get; set; }

    public string? TokenRecuperacion { get; set; }



}
