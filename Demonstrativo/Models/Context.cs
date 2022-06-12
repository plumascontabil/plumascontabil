using Microsoft.EntityFrameworkCore;

namespace Demonstrativo.Models
{
    public class Context : DbContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<LancamentoPadrao> LancamentosPadroes { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<ProvisoesDepreciacao> ProvisoesDepreciacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoConta> TiposContas { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVendas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<OfxLancamento> OfxLancamentos { get; set; }
        public DbSet<OfxLoteLancamento> OfxLoteLancamento { get; set; }
        public DbSet<ContaContabil> ContasContabeis { get; set; }
        public DbSet<OfxBanco> OfxBancos { get; set; }
        public DbSet<OfxContaCorrente> ContasCorrentes { get; set; }
        public DbSet<AutoDescricao> AutoDescricoes { get; set; }
        public DbSet<SaldoMensal> SaldoMensal { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LancamentoPadrao>()
                           .HasOne(h => h.ContaCredito)
                           .WithMany(c => c.LancamentoPadraoCreditar)
                           .HasForeignKey(x => x.ContaCreditoId)
                           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LancamentoPadrao>()
                           .HasOne(h => h.ContaDebito)
                           .WithMany(c => c.LancamentoPadraoDebitar)
                           .HasForeignKey(x => x.ContaDebitoId)
                           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfxLoteLancamento>()
                .HasMany(f => f.Lancamentos)
                .WithOne(x => x.Lote)
                .HasForeignKey(x => x.LoteLancamentoId);

            modelBuilder.Entity<OfxLoteLancamento>()
               .HasOne(f => f.Empresa)
               .WithMany(x => x.Lotes)
               .HasForeignKey(x => x.EmpresaId);

        }

    }
}
