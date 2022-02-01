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
        public DbSet<LancamentoPadrao> Contas { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<ProvisoesDepreciacao> ProvisoesDepreciacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoConta> TiposContas { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVendas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ImportacaoOfx> Ofxs { get; set; }
        public DbSet<ContaContabil> ContasContabeis { get; set; }
        public DbSet<HistoricoOfx> HistoricosOfx { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<HistoricoOfx>()
                           .HasOne(h => h.ContaCredito)
                           .WithMany(c => c.HistoricosCreditosOfx)
                           .HasForeignKey(x => x.ContaCreditoId)
                           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoOfx>()
                           .HasOne(h => h.ContaDebito)
                           .WithMany(c => c.HistoricosDebitosOfx)
                           .HasForeignKey(x => x.ContaDebitoId)
                           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
