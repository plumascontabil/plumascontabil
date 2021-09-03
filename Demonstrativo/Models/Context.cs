using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Context : DbContext
    {
        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Contas> Contas { get; set; }
        public DbSet<Competencias> Competencias { get; set; }
        public DbSet<Lancamentos> Demonstrativos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=PLM366\SQLEXPRESS;Initial Catalog=teste;Integrated Security=True");
        }

        //Eu havia relacionado as tabelas, porém tirei por causa que sempre que tentava inputar um dado dava erro

    }
}
