using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Models;

namespace WebApplicationFacturas.Context
{
    public class ApplicationDBContext: DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> Options): base(Options)
        {
            
        }

        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Empleados> Empleados { get; set; }
        public DbSet<Cargos> Cargos { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Facturas> Facturas { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<TipoClientes> TipoClientes { get; set; }
        public  DbSet<FacturasProductos> FacturasProductos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargos>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.EmpleadoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cargos_Empleados");
            });

            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Empleados>(entity =>
            {
                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Empresas)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.EmpresaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Empleados_Empresas1");
            });

            modelBuilder.Entity<Empresas>(entity =>
            {
                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nit)
                    .HasColumnName("NIT")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Propietario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Facturas>(entity =>
            {
                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCliente)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Facturas_Clientes");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.EmpleadoId)
                    .HasConstraintName("FK_Facturas_Empleados");
            });

            modelBuilder.Entity<FacturasProductos>(entity =>
            {
                entity.Property(e => e.Id).UseIdentityColumn();

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Facturas)
                    .WithMany(p => p.FacturasProductos)
                    .HasForeignKey(d => d.FacturaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FacturasProductos_Facturas");

                entity.HasOne(d => d.Productos)
                    .WithMany(p => p.FacturasProductos)
                    .HasForeignKey(d => d.ProductosId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FacturasProductos_Productos");
            });

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categorias)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Productos_Categorias");
            });

            modelBuilder.Entity<TipoClientes>(entity =>
            {
                entity.Property(e => e.TipoCliente)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.TipoClientes)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__TipoClien__Clien__46E78A0C");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
