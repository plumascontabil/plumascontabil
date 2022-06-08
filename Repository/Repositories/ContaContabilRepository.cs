using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;


namespace Repository.Repositories
{
    public class ContaContabilRepository : Repository, IContaContabilRepository
    {
        private readonly DbSet<ContaContabil> _contaContabil;

        public ContaContabilRepository(PContext context) : base(context)
        {

            _contaContabil = Context.Set<ContaContabil>();
        }
    }
}
