using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IOfxBancoRepository
    {
        public Task<OfxBanco> GetById(int? id);
        public Task<bool> Adicionar(OfxBanco ofxBanco);
        public Task<bool> Editar(OfxBanco ofxBanco);
        public Task<bool> Deletar(int id);
    }
}
