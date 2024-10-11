using demoCliente.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace demoCliente.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string? path = Path.GetDirectoryName(path: Assembly.GetEntryAssembly().Location);
        //    var conn = new ConfigurationBuilder()
        //    .SetBasePath(basePath: path)
        //    .AddJsonFile("appSettings.json", optional: false)
        //    .Build();

        //    optionsBuilder.UseSqlServer(conn.GetConnectionString("localDB"));
        //}

        public DbSet<DemoCliente> Clientes { get; set; }
        public DbSet<DemoDireccion> Direcciones { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DemoCliente>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__demoClie__3214EC07BC995483");
                entity.ToTable("demoClientes");
                entity.Property(e => e.ApellidoMaterno).HasMaxLength(100);
                entity.Property(e => e.ApellidoPaterno).HasMaxLength(100);
                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            mb.Entity<DemoDireccion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__demoDire__3214EC07478147C3");
                entity.ToTable("demoDireccion");
                entity.Property(e => e.Calle).HasMaxLength(100);
                entity.Property(e => e.CodigoPostal).HasMaxLength(10);
                entity.Property(e => e.Colonia).HasMaxLength(100);
                entity.Property(e => e.Municipio).HasMaxLength(100);
                entity.Property(e => e.NumeroInterior).HasMaxLength(10);
                entity.HasOne(d => d.Cliente).WithMany(p => p.DemoDireccions)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__demoDirec__Clien__4AB8E647");
            });
        }

        public async Task<string> AltaClienteAsync(string nombre, string apellidoPaterno, string apellidoMaterno, string calle, string numeroInterior, string colonia, string codigoPostal, string municipio)
        {
            var mensaje = new SqlParameter
            {
                ParameterName = "@Mensaje",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 50,
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await this.Database.ExecuteSqlRawAsync(
                "EXEC AltaCliente @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Calle, @NumeroInterior, @Colonia, @CodigoPostal, @Municipio, @Mensaje OUT",
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@ApellidoPaterno", apellidoPaterno),
                new SqlParameter("@ApellidoMaterno", apellidoMaterno),
                new SqlParameter("@Calle", calle),
                new SqlParameter("@NumeroInterior", numeroInterior),
                new SqlParameter("@Colonia", colonia),
                new SqlParameter("@CodigoPostal", codigoPostal),
                new SqlParameter("@Municipio", municipio),
                mensaje
            );

            return mensaje.Value?.ToString();
        }

        public async Task<string> EliminarClienteAsync(int clienteId)
        {
            var mensaje = new SqlParameter
            {
                ParameterName = "@Mensaje",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 50,
                Direction = System.Data.ParameterDirection.Output
            };

            await this.Database.ExecuteSqlRawAsync(
                "EXEC EliminarCliente @ClienteId, @Mensaje OUT",
                new SqlParameter("@ClienteId", clienteId),
                mensaje
            );

            return mensaje.Value?.ToString();
        }
    }
}
