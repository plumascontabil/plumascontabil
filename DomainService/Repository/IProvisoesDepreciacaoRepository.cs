using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IProvisoesDepreciacaoRepository
    {
        public Task<ProvisoesDepreciacao> GetById(int? id);
        public List<ProvisoesDepreciacao> GetAll();
        public Task<bool> Adicionar(ProvisoesDepreciacao provisoesDepreciacao);
        public Task<bool> Editar(ProvisoesDepreciacao provisoesDepreciacao);
        public Task<bool> Deletar(int id);
    }
}
