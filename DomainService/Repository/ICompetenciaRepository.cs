using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface ICompetenciaRepository
    {
        public Task<Competencia> GetByData(DateTime data);
        public Task<bool> Adicionar(Competencia competencia);
        public Task<bool> Editar(Competencia competencia);
        public Task<bool> Deletar(int id);
    }
}
