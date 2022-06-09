using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IOfxLoteLancamentoRepository
    {
        public Task<OfxLoteLancamento> GetById(int? id);
        public Task<bool> Adicionar(OfxLoteLancamento ofxLoteLancamento);
        public Task<bool> Editar(OfxLoteLancamento ofxLoteLancamento);
        public Task<bool> Deletar(int id);
    }
}
