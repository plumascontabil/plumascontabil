using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CategoriaRepository : Repository, ICategoriaRepository
    {
        private readonly DbSet<Categoria> _categoria;

        public CategoriaRepository(PContext context) : base(context)
        {

            _categoria = Context.Set<Categoria>();
        }

        public async Task<Categoria> GetById(int? id)
        {
            var categoria = await _categoria
                           .FirstOrDefaultAsync(m => m.Id == id);
            return categoria;
        }

        public List<Categoria> GetAll()
        {
            var categoria = _categoria
                           .ToList();
            return categoria;
        }

        public async Task<bool> Adicionar(Categoria categoria)
        {
            _categoria.Add(categoria);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(Categoria categoria)
        {
            _categoria.Update(categoria);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var categoria = await _categoria.FindAsync(id);
            _categoria.Remove(categoria);
            await Context.SaveChangesAsync();
            return true;
        }

    }
}
