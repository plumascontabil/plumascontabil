using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OfxSaldoContaRepository : Repository, IOfxSaldoContaRepository
    {
        private readonly DbSet<OfxSaldoConta> _ofxSaldoConta;

        public OfxSaldoContaRepository(PContext context) : base(context)
        {

            _ofxSaldoConta = Context.Set<OfxSaldoConta>();
        }

        public async Task<OfxSaldoConta> GetById(int? id)
        {
            var ofxSaldoConta = await _ofxSaldoConta
                .FirstOrDefaultAsync(m => m.Id == id);
            return ofxSaldoConta;
        }

        public async Task<bool> Adicionar(OfxSaldoConta ofxSaldoConta)
        {
            _ofxSaldoConta.Add(ofxSaldoConta);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(OfxSaldoConta ofxSaldoConta)
        {
            _ofxSaldoConta.Update(ofxSaldoConta);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var ofxSaldoConta = await _ofxSaldoConta.FindAsync(id);
            _ofxSaldoConta.Remove(ofxSaldoConta);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
