using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;

namespace Repository.Repositories
{

    public class CompetenciaRepository : Repository, ICompetenciaRepository
    {
        private readonly DbSet<Competencia> _competencia;

        public CompetenciaRepository(PContext context) : base(context)
        {

            _competencia = Context.Set<Competencia>();
        }

    }

}
