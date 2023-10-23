using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaViajes.Models;

public partial class SistemaViajesContext : DbContext
{
    public SistemaViajesContext()
    {
    }

    public SistemaViajesContext(DbContextOptions<SistemaViajesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ColaboradorSucursal> ColaboradorSucursals { get; set; }

    public virtual DbSet<Colaboradore> Colaboradores { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<Transportista> Transportistas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Viaje> Viajes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ColaboradorSucursal>(entity =>
        {
            entity.HasKey(e => new { e.ColaboradorId, e.SucursalId }).HasName("PK__Colabora__2E613A0FB97923CA");

            entity.ToTable("ColaboradorSucursal");

            entity.HasOne(d => d.Colaborador).WithMany(p => p.ColaboradorSucursals)
                .HasForeignKey(d => d.ColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Colaborad__Colab__52593CB8");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.ColaboradorSucursals)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Colaborad__Sucur__534D60F1");
        });

        modelBuilder.Entity<Colaboradore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Colabora__3214EC07561E444E");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3213E83F75278DA9");

            entity.ToTable("Rol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sucursal__3214EC07F1D624A5");

            entity.Property(e => e.BarrioColonia)
                .HasMaxLength(100)
                .HasColumnName("Barrio_Colonia");
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Transportista>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3214EC0703DFED79");

            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.TarifaPorKilometro).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3213E83F590BC404");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasColumnName("rolId");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK__Usuarios__rolId__4BAC3F29");
        });

        modelBuilder.Entity<Viaje>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Viajes__3214EC07CD1146A9");

            entity.Property(e => e.Fecha).HasColumnType("datetime");

            entity.HasOne(d => d.Colaborador).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.ColaboradorId)
                .HasConstraintName("FK__Viajes__Colabora__5812160E");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.SucursalId)
                .HasConstraintName("FK__Viajes__Sucursal__59063A47");

            entity.HasOne(d => d.Transportista).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.TransportistaId)
                .HasConstraintName("FK__Viajes__Transpor__59FA5E80");

            entity.HasOne(d => d.UsuarioRegistrador).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.UsuarioRegistradorId)
                .HasConstraintName("FK__Viajes__UsuarioR__5AEE82B9");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
