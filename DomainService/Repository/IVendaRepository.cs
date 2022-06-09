using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IVendaRepository
    {
        public Task<Venda> GetById(int? id);
        public List<Venda> GetAll();
        public Task<bool> Adicionar(Venda venda);
        public Task<bool> Editar(Venda venda);
        public Task<bool> Deletar(int id);
    }
}
