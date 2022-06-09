using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain;
using DomainService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace DomainService
{
    public class AutoDescricoesDomainService
    {
        private readonly IAutoDescricaoRepository _autoDescricoesRepository;
        private readonly ILancamentoPadraoRepository _lancamentoPadraoRepository;


        public AutoDescricoesDomainService(
            IAutoDescricaoRepository autoDescricoesRepository,
            ILancamentoPadraoRepository lancamentoPadraoRepository
            )
        {
            _autoDescricoesRepository = autoDescricoesRepository;
            _lancamentoPadraoRepository = lancamentoPadraoRepository;
        }

        public IIncludableQueryable<AutoDescricao, LancamentoPadrao> GetAutoDescricoes()
        {
            try
            {
                var context = _autoDescricoesRepository.GetAutoDescricoes();
                return context;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<AutoDescricao> GetAutoDescricaoById(int? id)
        {
            try
            {
                var autoDescricao = await _autoDescricoesRepository.GetById(id);
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

        public async Task<bool> CreateValidar(AutoDescricao autoDescricao)
        {
            try
            {
                var adicionado = await _autoDescricoesRepository.Adicionar(autoDescricao);
                return adicionado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<bool> EditValidar(AutoDescricao autoDescricao)
        {
            try
            {
                var adicionado = await _autoDescricoesRepository.Editar(autoDescricao);
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
                var deletado = await _autoDescricoesRepository.Deletar(id);
                return deletado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
