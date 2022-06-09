using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface ILancamentoRepository
    {
        public Task<Lancamento> GetById(int? id);
        public bool GetByDataCompetencia(DateTime competencia);
        public List<Lancamento> GetByEmpresaIdDataCompetencia(int? empresasId, DateTime? competenciasId);
        public Task<bool> Adicionar(Lancamento lancamento);
        public Task<bool> Editar(Lancamento lancamento);
        public Task<bool> Deletar(int id);
    }
}
