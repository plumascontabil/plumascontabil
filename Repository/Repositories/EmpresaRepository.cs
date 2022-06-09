using Domain;
using DomainService.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class EmpresaRepository : Repository , IEmpresaRepository
    {
        private readonly DbSet<Empresa> _empresa;

        public EmpresaRepository(PContext context) : base(context)
        {

            _empresa = Context.Set<Empresa>();
        }

        public async Task<Empresa> GetById(int? id)
        {
            var empresa = await _empresa
                .FirstOrDefaultAsync(m => m.Codigo == id);
            return empresa;
        }

        public async Task<bool> Adicionar(Empresa empresa)
        {
            _empresa.Add(empresa);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Editar(Empresa empresa)
        {
            _empresa.Update(empresa);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var autoDescricao = await _empresa.FindAsync(id);
            _empresa.Remove(autoDescricao);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
