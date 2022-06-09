using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OfxContaCorrenteRepository : Repository, IOfxContaCorrenteRepository
    {
        private readonly DbSet<OfxContaCorrente> _ofxContaCorrente;

        public OfxContaCorrenteRepository(PContext context) : base(context)
        {

            _ofxContaCorrente = Context.Set<OfxContaCorrente>();
        }

        public async Task<OfxContaCorrente> GetById(int? id)
        {
            var ofxContaCorrente = await _ofxContaCorrente
                .FirstOrDefaultAsync(m => m.Id == id);
            return ofxContaCorrente;
        }

        public bool GetByNumeroContaExists(string numeroConta)
        {
            var ofxContaCorrente = _ofxContaCorrente
                .Any(c => c.NumeroConta == numeroConta);
            return ofxContaCorrente;
        }

        public OfxContaCorrente GetByNumeroConta(string numeroConta)
        {
            var ofxContaCorrente = _ofxContaCorrente
                .FirstOrDefault(c => c.NumeroConta == numeroConta);
            return ofxContaCorrente;
        }
        public bool GetByIdExists(int? id)
        {
            var ofxBanco = _ofxContaCorrente
                .Any(e => e.Id == id);
            return ofxBanco;
        }
        public IQueryable<OfxContaCorrente> GetByEmpresaId(int? EmpresaId)
        {
            var ofxContaCorrente = _ofxContaCorrente
                .Where(m => m.EmpresaId == EmpresaId);
            return ofxContaCorrente;
        }

        public async Task<bool> Adicionar(OfxContaCorrente ofxContaCorrente)
        {
            _ofxContaCorrente.Add(ofxContaCorrente);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(OfxContaCorrente ofxContaCorrente)
        {
            _ofxContaCorrente.Update(ofxContaCorrente);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var ofxContaCorrente = await _ofxContaCorrente.FindAsync(id);
            _ofxContaCorrente.Remove(ofxContaCorrente);
            await Context.SaveChangesAsync();
            return true;
        }

    }
}
