using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProdutoRepository : Repository, IProdutoRepository
    {
        private readonly DbSet<Produto> _produto;

        public ProdutoRepository(PContext context) : base(context)
        {

            _produto = Context.Set<Produto>();
        }

        public async Task<Produto> GetById(int? id)
        {
            var produto = await _produto
                .FirstOrDefaultAsync(m => m.Id == id);
            return produto;
        }

        public async Task<bool> Adicionar(Produto produto)
        {
            _produto.Add(produto);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(Produto produto)
        {
            _produto.Update(produto);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var produto = await _produto.FindAsync(id);
            _produto.Remove(produto);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
