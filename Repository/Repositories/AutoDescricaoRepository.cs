using Domain;
using DomainService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AutoDescricaoRepository : Repository, IAutoDescricaoRepository
    {
        private readonly DbSet<AutoDescricao> _autoDescricao;


        public AutoDescricaoRepository(PContext context) : base(context)
        {

            _autoDescricao = Context.Set<AutoDescricao>();
        }
        public IIncludableQueryable<AutoDescricao, LancamentoPadrao> GetAutoDescricoes()
        {
            var context = _autoDescricao.Include(a => a.LancamentoPadrao);
            return context;
        }

        public async Task<AutoDescricao> GetById(int? id)
        {
            var autoDescricao = await _autoDescricao
                           .Include(a => a.LancamentoPadrao)
                           .FirstOrDefaultAsync(m => m.Id == id);
            return autoDescricao;
        }

        public async Task<bool> Adicionar(AutoDescricao autoDescricao)
        {
            _autoDescricao.Add(autoDescricao);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(AutoDescricao autoDescricao)
        {
            _autoDescricao.Update(autoDescricao);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var autoDescricao = await _autoDescricao.FindAsync(id);
            _autoDescricao.Remove(autoDescricao);
            await Context.SaveChangesAsync();
            return true;
        }

    }
}
