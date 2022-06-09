using Domain;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IAutoDescricaoRepository
    {
        public IIncludableQueryable<AutoDescricao, LancamentoPadrao> GetAutoDescricoes();
        public List<AutoDescricao> GetAll();
        public Task<AutoDescricao> GetById(int? id);
        public AutoDescricao GetByDescricao(string descricao);
        public Task<bool> Adicionar(AutoDescricao autoDescricao);
        public Task<bool> Editar(AutoDescricao autoDescricao);
        public Task<bool> Deletar(int id);

    }
}
