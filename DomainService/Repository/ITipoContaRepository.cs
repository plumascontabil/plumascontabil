using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface ITipoContaRepository
    {
        public Task<TipoConta> GetById(int? id);
        public Task<bool> Adicionar(TipoConta tipoConta);
        public Task<bool> Editar(TipoConta tipoConta);
        public Task<bool> Deletar(int id);
    }
}
