using DomainService.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class RoleDomainService
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
        private readonly RoleManager<IdentityRole> _roleManager;



        public RoleDomainService(
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

    }
}
