﻿// <auto-generated />
using System;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(BancoContext))]
    [Migration("20240305231259_bancoBBDD")]
    partial class bancoBBDD
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entidades.Usuario", b =>
                {
                    b.Property<long>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_usuario");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdUsuario"));

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("contraseña");

                    b.Property<bool>("CuentaConfirmada")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("cuenta_confirmada");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("FchExpiracionToken")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("fch_expiracion_token");

                    b.Property<DateTime?>("FchRegistro")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("fch_registro");

                    b.Property<byte[]>("Foto")
                        .HasColumnType("bytea")
                        .HasColumnName("foto");

                    b.Property<string>("NombreApellidos")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("nombre_apellidos");

                    b.Property<string>("Rol")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("rol");

                    b.Property<string>("TlfMovil")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)")
                        .HasColumnName("tlf_movil");

                    b.Property<string>("TokenRecuperacion")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("token_recuperacion");

                    b.HasKey("IdUsuario")
                        .HasName("usuarios_pkey");

                    b.HasIndex(new[] { "Email" }, "usuarios_email_key")
                        .IsUnique();

                    b.ToTable("usuarios", "esqUsuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
