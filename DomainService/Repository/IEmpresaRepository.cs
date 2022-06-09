using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IEmpresaRepository
    {
        public Task<Empresa> GetById(int? id);
        public Task<bool> Adicionar(Empresa empresa);
        public Task<bool> Editar(Empresa empresa);
        public Task<bool> Deletar(int id);
    }
}
