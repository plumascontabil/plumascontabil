using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OfxLoteLancamentoRepository : Repository, IOfxLoteLancamentoRepository
    {
        private readonly DbSet<OfxLoteLancamento> _ofxLoteLancamento;

        public OfxLoteLancamentoRepository(PContext context) : base(context)
        {

            _ofxLoteLancamento = Context.Set<OfxLoteLancamento>();
        }

        public async Task<OfxLoteLancamento> GetById(int? id)
        {
            var ofxLoteLancamento = await _ofxLoteLancamento
                .FirstOrDefaultAsync(m => m.Id == id);
            return ofxLoteLancamento;
        }

        public async Task<bool> Adicionar(OfxLoteLancamento ofxLoteLancamento)
        {
            _ofxLoteLancamento.Add(ofxLoteLancamento);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(OfxLoteLancamento ofxLoteLancamento)
        {
            _ofxLoteLancamento.Update(ofxLoteLancamento);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var ofxLoteLancamento = await _ofxLoteLancamento.FindAsync(id);
            _ofxLoteLancamento.Remove(ofxLoteLancamento);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
