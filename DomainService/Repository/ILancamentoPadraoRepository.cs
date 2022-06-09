using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface ILancamentoPadraoRepository
    {
        public Task<LancamentoPadrao> GetById(int? id);
        public Task<bool> Adicionar(LancamentoPadrao lancamentoPadrao);
        public Task<bool> Editar(LancamentoPadrao lancamentoPadrao);
        public Task<bool> Deletar(int id);
    }
}
