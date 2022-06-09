using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IOfxSaldoContaRepository
    {
        public Task<OfxSaldoConta> GetById(int? id);
        public Task<bool> Adicionar(OfxSaldoConta ofxSaldoConta);
        public Task<bool> Editar(OfxSaldoConta ofxSaldoConta);
        public Task<bool> Deletar(int id);
    }
}
