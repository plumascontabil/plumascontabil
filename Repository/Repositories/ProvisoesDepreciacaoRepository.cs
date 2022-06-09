using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProvisoesDepreciacaoRepository : Repository, IProvisoesDepreciacaoRepository
    {
        private readonly DbSet<ProvisoesDepreciacao> _provisoesDepreciacao;

        public ProvisoesDepreciacaoRepository(PContext context) : base(context)
        {

            _provisoesDepreciacao = Context.Set<ProvisoesDepreciacao>();
        }

        public async Task<ProvisoesDepreciacao> GetById(int? id)
        {
            var provisoesDepreciacao = await _provisoesDepreciacao
                .FirstOrDefaultAsync(m => m.Id == id);
            return provisoesDepreciacao;
        }

        public async Task<bool> Adicionar(ProvisoesDepreciacao provisoesDepreciacao)
        {
            _provisoesDepreciacao.Add(provisoesDepreciacao);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(ProvisoesDepreciacao provisoesDepreciacao)
        {
            _provisoesDepreciacao.Update(provisoesDepreciacao);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var produto = await _provisoesDepreciacao.FindAsync(id);
            _provisoesDepreciacao.Remove(produto);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
