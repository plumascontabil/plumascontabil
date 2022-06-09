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
        public Task<bool> Adicionar(SaldoMensal saldoMensal);
        public Task<bool> Editar(SaldoMensal saldoMensal);
        public Task<bool> Deletar(int id);
    }
}
