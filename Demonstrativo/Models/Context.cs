using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Context : DbContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<ProvisoesDepreciacao> ProvisoesDepreciacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=PLM366\SQLEXPRESS;Initial Catalog=teste;Integrated Security=True");
        }
    }
}
