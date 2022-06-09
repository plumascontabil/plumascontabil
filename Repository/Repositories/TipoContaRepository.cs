using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TipoContaRepository : Repository, ITipoContaRepository
    {
        private readonly DbSet<TipoConta> _TipoConta;

        public TipoContaRepository(PContext context) : base(context)
        {

            _TipoConta = Context.Set<TipoConta>();
        }

        public async Task<TipoConta> GetById(int? id)
        {
            var tipoConta = await _TipoConta
                .FirstOrDefaultAsync(m => m.Id == id);
            return tipoConta;
        }

        public async Task<bool> Adicionar(TipoConta tipoConta)
        {
            _TipoConta.Add(tipoConta);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(TipoConta tipoConta)
        {
            _TipoConta.Update(tipoConta);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var tipoConta = await _TipoConta.FindAsync(id);
            _TipoConta.Remove(tipoConta);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
