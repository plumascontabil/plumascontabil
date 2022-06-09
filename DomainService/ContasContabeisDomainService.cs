using Domain;
using DomainService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class ContasContabeisDomainService
    {
        private readonly IContaContabilRepository _contaContabilRepository;
        private readonly ILancamentoPadraoRepository _lancamentoPadraoRepository;


        public ContasContabeisDomainService(
            IContaContabilRepository contaContabilRepository,
            ILancamentoPadraoRepository lancamentoPadraoRepository
            )
        {
            _contaContabilRepository = contaContabilRepository;
            _lancamentoPadraoRepository = lancamentoPadraoRepository;
        }

        public async Task<List<ContaContabil>> GetContasContabeis()
        {
            try
            {
                var context = await _contaContabilRepository.GetAll();
                return context;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<ContaContabil> GetContaContavelById(int id)
        {
            try
            {
                var autoDescricao = await _contaContabilRepository.GetById(id);
                return autoDescricao;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public string Create() // TODO
        {
            try
            {
                var autoDescricao = "";
                //var autoDescricao = new SelectList(_lancamentoPadraoRepository, "Id", "Descricao");
                return autoDescricao;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<bool> CreateValidar(ContaContabil contaContabil)
        {
            try
            {
                var adicionado = await _contaContabilRepository.Adicionar(contaContabil);
                return adicionado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<bool> EditValidar(ContaContabil contaContabil)
        {
            try
            {
                var adicionado = await _contaContabilRepository.Editar(contaContabil);
                return adicionado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<bool> DeleteConfirmado(int id)
        {
            try
            {
                var deletado = await _contaContabilRepository.Deletar(id);
                return deletado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }


    }
}
