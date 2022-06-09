using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Repository
{
    public interface IProdutoRepository
    {
        public Task<Produto> GetById(int? id);
        public Task<bool> Adicionar(Produto produto);
        public Task<bool> Editar(Produto produto);
        public Task<bool> Deletar(int id);
    }
}
