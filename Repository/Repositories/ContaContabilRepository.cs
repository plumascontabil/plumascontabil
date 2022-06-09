using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ContaContabilRepository : Repository, IContaContabilRepository
    {
        private readonly DbSet<ContaContabil> _contaContabil;

        public ContaContabilRepository(PContext context) : base(context)
        {

            _contaContabil = Context.Set<ContaContabil>();
        }

        public async Task<List<ContaContabil>> GetAll()
        {
            var contaContabil = await _contaContabil
                           .ToListAsync();
            return contaContabil;
        }

        public async Task<ContaContabil> GetById(int id)
        {
            var contaContabil = await _contaContabil
                .FirstOrDefaultAsync(m => m.Codigo == id);
            return contaContabil;
        }

        public async Task<bool> Adicionar(ContaContabil contaContabil)
        {
            _contaContabil.Add(contaContabil);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(ContaContabil contaContabil)
        {
            _contaContabil.Update(contaContabil);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var autoDescricao = await _contaContabil.FindAsync(id);
            _contaContabil.Remove(autoDescricao);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
