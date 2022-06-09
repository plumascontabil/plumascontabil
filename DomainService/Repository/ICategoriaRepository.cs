using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface ICategoriaRepository
    {
        public Task<Categoria> GetById(int? id);
        public Task<bool> Adicionar(Categoria autoDescricao);
        public Task<bool> Editar(Categoria autoDescricao);
        public Task<bool> Deletar(int id);
    }
}
