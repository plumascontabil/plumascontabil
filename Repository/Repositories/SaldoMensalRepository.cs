using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class SaldoMensalRepository : Repository, ISaldoMensalRepository
    {
        private readonly DbSet<SaldoMensal> _saldoMensal;

        public SaldoMensalRepository(PContext context) : base(context)
        {

            _saldoMensal = Context.Set<SaldoMensal>();
        }

        public async Task<SaldoMensal> GetById(int? id)
        {
            var saldoMensal = await _saldoMensal
                .FirstOrDefaultAsync(m => m.Id == id);
            return saldoMensal;
        }

        public async Task<bool> Adicionar(SaldoMensal saldoMensal)
        {
            _saldoMensal.Add(saldoMensal);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(SaldoMensal saldoMensal)
        {
            _saldoMensal.Update(saldoMensal);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var saldoMensal = await _saldoMensal.FindAsync(id);
            _saldoMensal.Remove(saldoMensal);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
