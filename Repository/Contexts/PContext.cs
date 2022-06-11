using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
    public class PContext : DbContext
    {
        public DbSet<AutoDescricao> AutoDescricoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<ContaContabil> ContasContabeis { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<ItemVenda> ItensVendas { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<LancamentoPadrao> LancamentosPadroes { get; set; }
        public DbSet<OfxBanco> OfxBancos { get; set; }
        public DbSet<OfxContaCorrente> ContasCorrentes { get; set; }
        public DbSet<OfxLancamento> OfxLancamentos { get; set; }
        public DbSet<OfxLoteLancamento> OfxLoteLancamentos { get; set; }
        public DbSet<OfxSaldoConta> OfxSaldosContas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProvisoesDepreciacao> ProvisoesDepreciacoes { get; set; }
        public DbSet<RoleTela> RoleTela { get; set; }
        public DbSet<SaldoMensal> SaldoMensal { get; set; }
        public DbSet<Tela> Tela { get; set; }
        public DbSet<TipoConta> TiposContas { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        public PContext(DbContextOptions<PContext> options) : base(options)
        {

        }
    }
}
