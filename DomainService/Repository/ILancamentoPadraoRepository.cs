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
        public bool GetByIdExists(int? id);
        public LancamentoPadrao GetByCodigoId(int? codigoId);
        public Task<LancamentoPadrao> GetByIdRelations(int? id);
        public List<LancamentoPadrao> GetAll();
        public Task<bool> Adicionar(LancamentoPadrao lancamentoPadrao);
        public Task<bool> Editar(LancamentoPadrao lancamentoPadrao);
        public Task<bool> Deletar(int id);
    }
}
