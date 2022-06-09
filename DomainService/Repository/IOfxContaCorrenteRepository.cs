using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IOfxContaCorrenteRepository
    {
        public Task<OfxContaCorrente> GetById(int? id);
        public bool GetByIdExists(int? id);
        public bool GetByNumeroContaExists(string numeroConta);
        public OfxContaCorrente GetByNumeroConta(string numeroConta);
        public IQueryable<OfxContaCorrente> GetByEmpresaId(int? EmpresaId);
        public Task<bool> Adicionar(OfxContaCorrente ofxContaCorrente);
        public Task<bool> Editar(OfxContaCorrente ofxContaCorrente);
        public Task<bool> Deletar(int id);
    }
}
