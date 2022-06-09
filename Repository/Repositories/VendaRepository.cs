using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class VendaRepository : Repository, IVendaRepository
    {
        private readonly DbSet<Venda> _venda;

        public VendaRepository(PContext context) : base(context)
        {

            _venda = Context.Set<Venda>();
        }

        public async Task<Venda> GetById(int? id)
        {
            var venda = await _venda
                .FirstOrDefaultAsync(m => m.Id == id);
            return venda;
        }

        public List<Venda> GetAll()
        {
            var venda =  _venda.ToList();
            return venda;
        }

        public async Task<bool> Adicionar(Venda venda)
        {
            _venda.Add(venda);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(Venda venda)
        {
            _venda.Update(venda);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var venda = await _venda.FindAsync(id);
            _venda.Remove(venda);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
