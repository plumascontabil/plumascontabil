using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;


namespace Repository.Repositories
{
    public class EmpresaRepository : Repository , IEmpresaRepository
    {
        private readonly DbSet<Empresa> _empresa;

        public EmpresaRepository(PContext context) : base(context)
        {

            _empresa = Context.Set<Empresa>();
        }
    }
}
