using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MetAndsInsUn.Database
{
    public partial class MetAndInsContext : DbContext
    {
        public MetAndInsContext()
        {
        }

        public MetAndInsContext(DbContextOptions<MetAndInsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Calibraciones> Calibraciones { get; set; }
        public virtual DbSet<Equipos> Equipos { get; set; }
        public virtual DbSet<Programaciones> Programaciones { get; set; }
        public virtual DbSet<TipoInstrumento> TipoInstrumento { get; set; }
        public virtual DbSet<Tipos> Tipos { get; set; }
        public virtual DbSet<TiposData> TiposData { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Calibraciones>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CalibrationFilePath)
                    .HasColumnName("calibration_file_path")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEquipoNavigation)
                    .WithMany(p => p.Calibraciones)
                    .HasForeignKey(d => d.IdEquipo)
                    .HasConstraintName("FK_Calibraciones_Equipos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Calibraciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Calibraciones_Usuarios");
            });

            modelBuilder.Entity<Equipos>(entity =>
            {
                entity.HasIndex(e => e.Area)
                    .HasName("NonClusteredIndex-20191125-123621");

                entity.HasIndex(e => e.Marca)
                    .HasName("NonClusteredIndex-20191125-123808");

                entity.HasIndex(e => e.TipoInstrumento)
                    .HasName("NonClusteredIndex-20191125-123842");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BuscarPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CheckEliminados)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Código)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DivisiónEscala)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.Frecuencia).HasColumnName("frecuencia");

                entity.Property(e => e.ImagePath)
                    .HasColumnName("image_path")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaxRango)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Rango)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Serie)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.EquiposAreaNavigation)
                    .HasForeignKey(d => d.Area)
                    .HasConstraintName("FK_Equipos_Tipos_Data_area");

                entity.HasOne(d => d.EstatusNavigation)
                    .WithMany(p => p.EquiposEstatusNavigation)
                    .HasForeignKey(d => d.Estatus)
                    .HasConstraintName("FK_Equipos_Tipos_Data_estatus");

                entity.HasOne(d => d.MarcaNavigation)
                    .WithMany(p => p.EquiposMarcaNavigation)
                    .HasForeignKey(d => d.Marca)
                    .HasConstraintName("FK_Equipos_Tipos_Data_marca");

                entity.HasOne(d => d.TipoInstrumentoNavigation)
                    .WithMany(p => p.Equipos)
                    .HasForeignKey(d => d.TipoInstrumento)
                    .HasConstraintName("FK_Equipos_TipoInstrumento");
            });

            modelBuilder.Entity<Programaciones>(entity =>
            {
                entity.HasIndex(e => new { e.IdEquipo, e.Ano })
                    .HasName("NonClusteredIndex-20191125-123916");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ano).HasColumnName("ano");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Semana).HasColumnName("semana");

                entity.HasOne(d => d.IdEquipoNavigation)
                    .WithMany(p => p.Programaciones)
                    .HasForeignKey(d => d.IdEquipo)
                    .HasConstraintName("FK_Programaciones_Equipos");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Programaciones)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("FK_Programaciones_Tipos_Data");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Programaciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Programaciones_Usuarios");
            });

            modelBuilder.Entity<TipoInstrumento>(entity =>
            {
                entity.HasIndex(e => e.Codigo)
                    .HasName("UQ__TipoInst__40F9A2063D32EC9C")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tipos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposData>(entity =>
            {
                entity.ToTable("Tipos_Data");

                entity.HasIndex(e => e.IdTipo)
                    .HasName("NonClusteredIndex-20191125-134103");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipo).HasColumnName("idTipo");

                entity.HasOne(d => d.IdTipoNavigation)
                    .WithMany(p => p.TiposData)
                    .HasForeignKey(d => d.IdTipo)
                    .HasConstraintName("FK__Tipos_Dat__idTip__412EB0B6");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Apellidos)
                    .HasColumnName("apellidos")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .HasColumnName("nombres")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
