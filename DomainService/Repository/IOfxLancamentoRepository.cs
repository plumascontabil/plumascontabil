using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IOfxLancamentoRepository
    {
        public Task<OfxLancamento> GetById(int? id);
        public List<OfxLancamento> GetAll();
        public IQueryable<OfxLancamento> GetByDataInicialFinal(DateTime inicial, DateTime final);
        public IQueryable<OfxLancamento> GetByContaCorrenteId(int? contaCorrenteId);
        public Task<bool> Adicionar(OfxLancamento Ofxlancamento);
        public Task<bool> Editar(OfxLancamento Ofxlancamento);
        public Task<bool> Deletar(int id);
    }
}
