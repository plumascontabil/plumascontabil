using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class LancamentoPadraoRepository : Repository, ILancamentoPadraoRepository
    {
        private readonly DbSet<LancamentoPadrao> _lancamentoPadrao;

        public LancamentoPadraoRepository(PContext context) : base(context)
        {

            _lancamentoPadrao = Context.Set<LancamentoPadrao>();
        }

        public async Task<LancamentoPadrao> GetById(int? id)
        {
            var LancamentoPadrao = await _lancamentoPadrao
                .FirstOrDefaultAsync(m => m.Id == id);
            return LancamentoPadrao;
        }

        public LancamentoPadrao GetByCodigoId(int? codigoId)
        {
            var LancamentoPadrao = _lancamentoPadrao
                .FirstOrDefault(l => l.Codigo == codigoId);
            return LancamentoPadrao;
        }

        public bool GetByIdExists(int? id)
        {
            var LancamentoPadrao = _lancamentoPadrao
                .Any(e => e.Id == id);
            return LancamentoPadrao;
        }

        public async Task<LancamentoPadrao> GetByIdRelations(int? id)
        {
            var LancamentoPadrao = await _lancamentoPadrao
                .Include(l => l.Categoria)
                .Include(l => l.ContaCredito)
                .Include(l => l.ContaDebito)
                .Include(l => l.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            return LancamentoPadrao;
        }

        public List<LancamentoPadrao> GetAll()
        {
            var categoria = _lancamentoPadrao.ToList();
            return categoria;
        }

        public async Task<bool> Adicionar(LancamentoPadrao lancamentoPadrao)
        {
            _lancamentoPadrao.Add(lancamentoPadrao);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(LancamentoPadrao lancamentoPadrao)
        {
            _lancamentoPadrao.Update(lancamentoPadrao);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var lancamentoPadrao = await _lancamentoPadrao.FindAsync(id);
            _lancamentoPadrao.Remove(lancamentoPadrao);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
