using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Repository.Repositories
{
    public class ItemVendaRepository : Repository, IItemVendaRepository
    {
        private readonly DbSet<ItemVenda> _itemVenda;

        public ItemVendaRepository(PContext context) : base(context)
        {

            _itemVenda = Context.Set<ItemVenda>();
        }
        public async Task<ItemVenda> GetById(int? id)
        {
            var itemVenda = await _itemVenda
                .FirstOrDefaultAsync(m => m.Id == id);
            return itemVenda;
        }

        public  List<ItemVenda> GetAll()
        {
            var itemVenda = _itemVenda
                .ToList();
            return itemVenda;
        }

        public async Task<bool> Adicionar(ItemVenda itemVenda)
        {
            _itemVenda.Add(itemVenda);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(ItemVenda itemVenda)
        {
            _itemVenda.Update(itemVenda);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var itemVenda = await _itemVenda.FindAsync(id);
            _itemVenda.Remove(itemVenda);
            await Context.SaveChangesAsync();
            return true;
        }

    }
}
