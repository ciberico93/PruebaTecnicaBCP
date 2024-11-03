using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Context;

public partial class PruebaTecnicaBcpContext : DbContext
{
    public PruebaTecnicaBcpContext()
    {
    }

    public PruebaTecnicaBcpContext(DbContextOptions<PruebaTecnicaBcpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoPedido> EstadoPedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoPedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido).HasName("PK__EstadoPe__86B98371F6C05A3E");

            entity.ToTable("EstadoPedido");

            entity.Property(e => e.Nombre)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC33DA626F1");

            entity.ToTable("Pedido");

            entity.HasIndex(e => e.NroPedido, "UQ__Pedido__33108A0B9CCE9456").IsUnique();

            entity.Property(e => e.FechaDespacho).HasColumnType("datetime");
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.FechaPedido).HasColumnType("datetime");
            entity.Property(e => e.NroPedido)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdDeliveryNavigation).WithMany(p => p.PedidoIdDeliveryNavigations)
                .HasForeignKey(d => d.IdDelivery)
                .HasConstraintName("FK__Pedido__IdDelive__3B75D760");

            entity.HasOne(d => d.IdEstadoPedidoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEstadoPedido)
                .HasConstraintName("FK__Pedido__IdEstado__3C69FB99");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.PedidoIdVendedorNavigations)
                .HasForeignKey(d => d.IdVendedor)
                .HasConstraintName("FK__Pedido__IdVended__3A81B327");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdPedidoDetalle).HasName("PK__PedidoDe__968E515BAF0B7183");

            entity.ToTable("PedidoDetalle");

            entity.Property(e => e.TotalPrecio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__PedidoDet__IdPed__3F466844");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__PedidoDet__IdPro__403A8C7D");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210B66F19F6");

            entity.ToTable("Producto");

            entity.HasIndex(e => e.Sku, "UQ__Producto__CA1FD3C5681F5B1C").IsUnique();

            entity.Property(e => e.Etiqueta)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasDefaultValueSql("((1))");
            entity.Property(e => e.Nombre)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(128)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584CE4FBACCC");

            entity.ToTable("Rol");

            entity.Property(e => e.Nombre)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97651345F7");

            entity.ToTable("Usuario");

            entity.Property(e => e.CodigoTrabajador)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Puesto)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoCelular)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__IdRol__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
