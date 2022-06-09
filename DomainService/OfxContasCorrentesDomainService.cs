using Domain;
using DomainService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class OfxContasCorrentesDomainService
    {
        private readonly IContaContabilRepository _contaContabilRepository;
        private readonly ILancamentoPadraoRepository _lancamentoPadraoRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IOfxLancamentoRepository _ofxLancamentoRepository;
        private readonly IOfxContaCorrenteRepository _ofxContaCorrenteRepository;
        private readonly ICompetenciaRepository _competenciaRepositoryRepository;
        private readonly IOfxBancoRepository _ofxBancoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IAutoDescricaoRepository _autoDescricaoRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ISaldoMensalRepository _saldoMensalRepository;
        private readonly IProvisoesDepreciacaoRepository _provisoesDepreciacaoRepository;
        private readonly IVendaRepository _vendaRepository;
        private readonly IItemVendaRepository _ItemVendaRepository;
        private readonly IProdutoRepository _produtoRepository;



        public OfxContasCorrentesDomainService(
           IContaContabilRepository contaContabilRepository,
           ILancamentoPadraoRepository lancamentoPadraoRepository,
           IEmpresaRepository empresaRepository,
           IOfxLancamentoRepository ofxLancamentoRepository,
           IOfxContaCorrenteRepository ofxContaCorrenteRepository,
           IOfxBancoRepository ofxBancoRepository,
           ICompetenciaRepository competenciaRepositoryRepository,
           ICategoriaRepository categoriaRepository,
           IAutoDescricaoRepository autoDescricaoRepository,
           ILancamentoRepository lancamentoRepository,
           ISaldoMensalRepository saldoMensalRepository,
           IProvisoesDepreciacaoRepository provisoesDepreciacaoRepository,
           IVendaRepository vendaRepository,
           IItemVendaRepository ItemVendaRepository,
           IProdutoRepository produtoRepository

           )
        {
            _contaContabilRepository = contaContabilRepository;
            _lancamentoPadraoRepository = lancamentoPadraoRepository;
            _empresaRepository = empresaRepository;
            _ofxLancamentoRepository = ofxLancamentoRepository;
            _ofxContaCorrenteRepository = ofxContaCorrenteRepository;
            _competenciaRepositoryRepository = competenciaRepositoryRepository;
            _categoriaRepository = categoriaRepository;
            _autoDescricaoRepository = autoDescricaoRepository;
            _lancamentoRepository = lancamentoRepository;
            _saldoMensalRepository = saldoMensalRepository;
            _ofxBancoRepository = ofxBancoRepository;
            _provisoesDepreciacaoRepository = provisoesDepreciacaoRepository;
            _vendaRepository = vendaRepository;
            _ItemVendaRepository = ItemVendaRepository;
            _produtoRepository = produtoRepository;

        }


        public async Task<OfxContaCorrente> Details(int? id)
        {
            try
            {
                var ofxContasCorrentes = await _ofxContaCorrenteRepository.GetById(id);
                return ofxContasCorrentes;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<bool> CreateValidar(OfxContaCorrente ofxContasCorrentes)
        {
            try
            {
                var adicionado = await _ofxContaCorrenteRepository.Adicionar(ofxContasCorrentes);
                return adicionado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<bool> EditValidar(OfxContaCorrente ofxContasCorrentes)
        {
            try
            {
                var adicionado = await _ofxContaCorrenteRepository.Editar(ofxContasCorrentes);
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
                var deletado = await _ofxContaCorrenteRepository.Deletar(id);
                return deletado;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        private bool OfxContasCorrentesExists(int id)
        {
            try
            {
                var ofxContasCorrentesExist = _ofxContaCorrenteRepository.GetByIdExists(id);
                return ofxContasCorrentesExist;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
    }
}
