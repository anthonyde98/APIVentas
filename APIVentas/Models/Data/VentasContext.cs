using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class VentasContext : DbContext
    {
        public VentasContext()
        {
        }

        public VentasContext(DbContextOptions<VentasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<DetalleOrdenVenta> DetalleOrdenVenta { get; set; }
        public virtual DbSet<OrdenVenta> OrdenVenta { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<ProductoCategoria> ProductoCategoria { get; set; }
        public virtual DbSet<Vendedor> Vendedors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost\\SQLEXPRESS;database=Ventas;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.HasIndex(e => e.Cedula, "UQ__Cliente__415B7BE5881509E7")
                    .IsUnique();

                entity.HasIndex(e => e.ClienteCodigo, "UQ__Cliente__A062A53ADA44A2B2")
                    .IsUnique();

                entity.Property(e => e.ClienteId).HasColumnName("clienteId");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("cedula");

                entity.Property(e => e.ClienteCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("clienteCodigo");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("correo");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnName("direccion");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaModificacion");

                entity.Property(e => e.MembresiaCodigo)
                    .HasMaxLength(10)
                    .HasColumnName("membresiaCodigo");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nombres");

                entity.Property(e => e.NumeroTelefono)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("numeroTelefono");
            });

            modelBuilder.Entity<DetalleOrdenVenta>(entity =>
            {
                entity.HasKey(e => e.DetalleOrdenVentaId)
                    .HasName("PK__DetalleO__D3B0E0D3EFBF7DC0");

                entity.HasIndex(e => e.DetalleOrdenVentaCodigo, "UQ__DetalleO__B25A58E0D334CE37")
                    .IsUnique();

                entity.Property(e => e.DetalleOrdenVentaId).HasColumnName("detalleOrdenVentaId");

                entity.Property(e => e.DescuentoTotal)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("descuentoTotal");

                entity.Property(e => e.DetalleOrdenVentaCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("detalleOrdenVentaCodigo");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaModificacion");

                entity.Property(e => e.ProductoCantidad).HasColumnName("productoCantidad");

                entity.Property(e => e.ProductoId).HasColumnName("productoId");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetalleOrdenVenta)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto");
            });

            modelBuilder.Entity<OrdenVenta>(entity =>
            {
                entity.HasKey(e => e.OrdenVentaId)
                    .HasName("PK__OrdenVen__C965AC7BEDC4D1A7");

                entity.HasIndex(e => e.OrdenVentaCodigo, "UQ__OrdenVen__5D4D5DFEFAE9A0E5")
                    .IsUnique();

                entity.Property(e => e.OrdenVentaId).HasColumnName("ordenVentaId");

                entity.Property(e => e.ClienteId).HasColumnName("clienteId");

                entity.Property(e => e.DescuentoTotalOrden)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("descuentoTotalOrden");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaModificacion");

                entity.Property(e => e.OrdenFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("ordenFecha");

                entity.Property(e => e.OrdenVentaCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("ordenVentaCodigo");

                entity.Property(e => e.ValorCantidadBruto)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("valorCantidadBruto");

                entity.Property(e => e.ValorCantidadNeto)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("valorCantidadNeto");

                entity.Property(e => e.ValorDevuelto)
                    .HasColumnType("decimal(7, 2)")
                    .HasColumnName("valorDevuelto");

                entity.Property(e => e.ValorPagado)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("valorPagado");

                entity.Property(e => e.VendedorId).HasColumnName("vendedorId");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.OrdenVenta)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente");

                entity.HasOne(d => d.Vendedor)
                    .WithMany(p => p.OrdenVenta)
                    .HasForeignKey(d => d.VendedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vendedor");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto");

                entity.HasIndex(e => e.ProductoCodigo, "UQ__Producto__D778113AEC638C6E")
                    .IsUnique();

                entity.Property(e => e.ProductoId).HasColumnName("productoId");

                entity.Property(e => e.CategoriaId).HasColumnName("categoriaId");

                entity.Property(e => e.Descuento)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("descuento");

                entity.Property(e => e.Empresa)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("empresa");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.FechaFinVenta)
                    .HasColumnType("date")
                    .HasColumnName("fechaFinVenta");

                entity.Property(e => e.FechaInicioVenta)
                    .HasColumnType("date")
                    .HasColumnName("fechaInicioVenta");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaModificacion");

                entity.Property(e => e.Ganancia)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("ganancia");

                entity.Property(e => e.Peso)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("peso");

                entity.Property(e => e.PrecioEstandar)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("precioEstandar");

                entity.Property(e => e.PrecioLista)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("precioLista");

                entity.Property(e => e.ProductoCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("productoCodigo");

                entity.Property(e => e.Tamanio)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("tamanio");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categoria");
            });

            modelBuilder.Entity<ProductoCategoria>(entity =>
            {
                entity.HasKey(e => e.CategoriaId)
                    .HasName("PK__Producto__6378C0C0F701B03F");

                entity.HasIndex(e => e.Nombre, "UQ__Producto__72AFBCC6313C7C38")
                    .IsUnique();

                entity.HasIndex(e => e.CategoriaCodigo, "UQ__Producto__CA40B18BB8CA49F2")
                    .IsUnique();

                entity.Property(e => e.CategoriaId).HasColumnName("categoriaId");

                entity.Property(e => e.CategoriaCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("categoriaCodigo");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaModificacion");

                entity.Property(e => e.Impuesto)
                    .HasColumnType("decimal(2, 2)")
                    .HasColumnName("impuesto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Vendedor>(entity =>
            {
                entity.ToTable("Vendedor");

                entity.HasIndex(e => e.Cedula, "UQ__Vendedor__415B7BE53648B668")
                    .IsUnique();

                entity.HasIndex(e => e.VendedorCodigo, "UQ__Vendedor__55FBDA7FCCBE49CD")
                    .IsUnique();

                entity.Property(e => e.VendedorId).HasColumnName("vendedorId");

                entity.Property(e => e.AfpCodigo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("afpCodigo");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("cedula");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("correo");

                entity.Property(e => e.DeduccionTotal)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("deduccionTotal");

                entity.Property(e => e.Departamento)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("departamento");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnName("direccion");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaModificacion");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nombres");

                entity.Property(e => e.NumeroTelefono)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("numeroTelefono");

                entity.Property(e => e.SalarioBruto)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("salarioBruto");

                entity.Property(e => e.SalarioNeto)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("salarioNeto");

                entity.Property(e => e.SeguroCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("seguroCodigo");

                entity.Property(e => e.VendedorCodigo)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasColumnName("vendedorCodigo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
