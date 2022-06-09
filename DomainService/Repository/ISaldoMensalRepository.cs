using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface ISaldoMensalRepository
    {
        public Task<SaldoMensal> GetById(int? id);
        public bool GetByIdExists(int? id);
        public Task<SaldoMensal> GetByRelationsId(int? id);
        public SaldoMensal GetByDataContaCorrenteId(DateTime data, int contaCorrenteId);
        public SaldoMensal GetByCompetenciaIdContaCorrenteId(DateTime? competenciasId, int contaCorrenteId);
        public Task<bool> Adicionar(SaldoMensal saldoMensal);
        public Task<bool> Editar(SaldoMensal saldoMensal);
        public Task<bool> Deletar(int id);
    }
}
