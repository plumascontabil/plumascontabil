using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IItemVendaRepository
    {
        public Task<ItemVenda> GetById(int? id);
        public List<ItemVenda> GetAll();
        public Task<bool> Adicionar(ItemVenda itemVenda);
        public Task<bool> Editar(ItemVenda itemVenda);
        public Task<bool> Deletar(int id);
    }
}
