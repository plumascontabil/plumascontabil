using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OfxBancoRepository : Repository, IOfxBancoRepository
    {
        private readonly DbSet<OfxBanco> _ofxBanco;

        public OfxBancoRepository(PContext context) : base(context)
        {

            _ofxBanco = Context.Set<OfxBanco>();
        }

        public async Task<OfxBanco> GetById(int? id)
        {
            var ofxBanco = await _ofxBanco
                .FirstOrDefaultAsync(m => m.Id == id);
            return ofxBanco;
        }
        public async Task<OfxBanco> GetByCodigoId(int? codigoId)
        {
            var ofxBanco = await _ofxBanco
                .FirstOrDefaultAsync(m => m.Codigo == codigoId);
            return ofxBanco;
        }

        public bool GetByCodigoIdExists(int? codigoId)
        {
            var ofxBanco = _ofxBanco
                .Any(m => m.Codigo == codigoId);
            return ofxBanco;
        }
        public bool GetByIdExists(int? id)
        {
            var ofxBanco = _ofxBanco
                .Any(e => e.Id == id);
            return ofxBanco;
        }

        public async Task<bool> Adicionar(OfxBanco ofxBanco)
        {
            _ofxBanco.Add(ofxBanco);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(OfxBanco ofxBanco)
        {
            _ofxBanco.Update(ofxBanco);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var ofxBanco = await _ofxBanco.FindAsync(id);
            _ofxBanco.Remove(ofxBanco);
            await Context.SaveChangesAsync();
            return true;
        }

    }
}
