using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entidades;

/// <summary>
/// Contexto de la base de datos en la aplicación
/// </summary>
public partial class BancoContext : DbContext
{
    public BancoContext()
    {
    }

    public BancoContext(DbContextOptions<BancoContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConexionBBDD");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios", "esqUsuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .HasColumnName("contraseña");
            entity.Property(e => e.CuentaConfirmada)
                .HasDefaultValue(false)
                .HasColumnName("cuenta_confirmada");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FchExpiracionToken)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_expiracion_token");
            entity.Property(e => e.FchRegistro)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_registro");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.NombreApellidos)
                .HasMaxLength(50)
                .HasColumnName("nombre_apellidos");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasColumnName("rol");
            entity.Property(e => e.TlfMovil)
                .HasMaxLength(9)
                .HasColumnName("tlf_movil");
            entity.Property(e => e.TokenRecuperacion)
                .HasMaxLength(100)
                .HasColumnName("token_recuperacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
