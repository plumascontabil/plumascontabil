using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OfxLancamentoRepository : Repository, IOfxLancamentoRepository
    {
        private readonly DbSet<OfxLancamento> _ofxLancamento;

        public OfxLancamentoRepository(PContext context) : base(context)
        {

            _ofxLancamento = Context.Set<OfxLancamento>();
        }

        public async Task<OfxLancamento> GetById(int? id)
        {
            var ofxLancamento = await _ofxLancamento
                .FirstOrDefaultAsync(m => m.Id == id);
            return ofxLancamento;
        }

        public List<OfxLancamento> GetAll()
        {
            var ofxLancamento =  _ofxLancamento.Include(x => x.LancamentoPadrao).ToList();
            return ofxLancamento;
        }
        public IQueryable<OfxLancamento> GetByContaCorrenteId(int? contaCorrenteId)
        {
            var ofxLancamento =  _ofxLancamento
                .Where(o => o.ContaCorrenteId == contaCorrenteId);

            return ofxLancamento;
        }

        public IQueryable<OfxLancamento> GetByDataInicialFinal(DateTime inicial, DateTime final)
        {
            var dadosOfx = _ofxLancamento.
                Where(o => (o.Data.Date >= inicial && o.Data.Date <= final));
            return dadosOfx;
        }

        public async Task<bool> Adicionar(OfxLancamento ofxLancamento)
        {
            _ofxLancamento.Add(ofxLancamento);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(OfxLancamento ofxLancamento)
        {
            _ofxLancamento.Update(ofxLancamento);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var OfxLancamento = await _ofxLancamento.FindAsync(id);
            _ofxLancamento.Remove(OfxLancamento);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
