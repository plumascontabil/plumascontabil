using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IContaContabilRepository
    {
        public Task<List<ContaContabil>> GetAll();
        public Task<ContaContabil> GetById(int id);
        public Task<bool> Adicionar(ContaContabil contaContabil);
        public Task<bool> Editar(ContaContabil contaContabil);
        public Task<bool> Deletar(int id);
    }
}
