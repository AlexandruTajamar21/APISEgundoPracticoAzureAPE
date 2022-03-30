using APISEgundoPracticoAzureAPE.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE.Data
{
    public class AzureContext : DbContext
    {
        public AzureContext(DbContextOptions<AzureContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
