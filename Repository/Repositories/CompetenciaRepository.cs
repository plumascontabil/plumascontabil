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

    public class CompetenciaRepository : Repository, ICompetenciaRepository
    {
        private readonly DbSet<Competencia> _competencia;

        public CompetenciaRepository(PContext context) : base(context)
        {

            _competencia = Context.Set<Competencia>();
        }

        public async Task<Competencia> GetByData(DateTime data)
        {
            var categoria = await _competencia
                           .FirstOrDefaultAsync(m => m.Data == data);
            return categoria;
        }

        public List<Competencia> GetAll()
        {
            var categoria =  _competencia.ToList();
            return categoria;
        }

        public bool validateCompetencia(DateTime competenciaAtual)
        {
            var categoria = _competencia.Any(c => c.Data == competenciaAtual);
            return categoria;
        }

        public async Task<bool> Adicionar(Competencia competencia)
        {
            _competencia.Add(competencia);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(Competencia competencia)
        {
            _competencia.Update(competencia);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var competencia = await _competencia.FindAsync(id);
            _competencia.Remove(competencia);
            await Context.SaveChangesAsync();
            return true;
        }

    }

}
