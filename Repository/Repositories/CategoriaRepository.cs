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
    public class CategoriaRepository : Repository , ICategoriaRepository
    {
        private readonly DbSet<Categoria> _categoria;

        public CategoriaRepository(PContext context) : base(context)
        {

            _categoria = Context.Set<Categoria>();
        }

    }
}
