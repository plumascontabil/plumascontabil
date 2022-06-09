using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using Domain;
using DomainService.ViewModel;
using DomainService;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DomainService
{
    public class ImportacaoDomainService
    {
        private readonly IContaContabilRepository _contaContabilRepository;
        private readonly ILancamentoPadraoRepository _lancamentoPadraoRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IOfxLancamentoRepository _ofxLancamentoRepository;
        private readonly IOfxContaCorrenteRepository _ofxContaCorrenteRepository;


        public ImportacaoDomainService(
            IContaContabilRepository contaContabilRepository,
            ILancamentoPadraoRepository lancamentoPadraoRepository,
            IEmpresaRepository empresaRepository,
            IOfxLancamentoRepository ofxLancamentoRepository,
            IOfxContaCorrenteRepository ofxContaCorrenteRepository
            )
        {
            _contaContabilRepository = contaContabilRepository;
            _lancamentoPadraoRepository = lancamentoPadraoRepository;
            _empresaRepository = empresaRepository;
            _ofxLancamentoRepository = ofxLancamentoRepository;
            _ofxContaCorrenteRepository = ofxContaCorrenteRepository;

        }

        public async Task<bool> Importar(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();

                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, new CultureInfo("pt-BR")))
                {
                    var records = csv.GetRecords<ImportacaoCsvViewModel>().ToList();

                    foreach (var record in records)
                    {
                        var insertEmpresa = new Empresa()
                        {
                            Codigo = record.Codigo,
                            RazaoSocial = record.Razao,
                            Apelido = record.Apelido,
                            Cnpj = record.Cnpj ?? "00.000.000/0001-00",
                            Situacao = record.Situacao,
                        };

                        await _empresaRepository.Adicionar(insertEmpresa);

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<bool> ImportarContasContabeis(IFormFile fileContasContabeis)
        {
            try
            {
                var stream = fileContasContabeis.OpenReadStream();

                using (var reader = new StreamReader(stream))
                using (var csvContas = new CsvReader(reader, new CultureInfo("pt-BR")))
                {
                    var records = csvContas.GetRecords<ImportacaoContaContabilViewModel>().ToList();

                    foreach (var record in records)
                    {
                        var insertContaContabil = new ContaContabil()
                        {
                            Codigo = record.Codigo,
                            Historico = record.Descricao,
                        };

                        await _contaContabilRepository.Adicionar(insertContaContabil);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public List<RelatorioViewModel> Filtrar(RelatorioViewModel relatorioViewModel)
        {
            try
            {
                var contaContabil = _contaContabilRepository.GetById(relatorioViewModel.ContaContabil);
                //var historicos = _context.OfxDescricoes.Where(h => h.ContaDebitoId == contaContabil.Codigo
                //                                    || h.ContaCreditoId == contaContabil.Codigo);

                var dadosOfx = _ofxLancamentoRepository.GetByDataInicialFinal(relatorioViewModel.DataInicial.Date, relatorioViewModel.DataFinal.Date);
                var contasCorrentes = _ofxContaCorrenteRepository.GetByEmpresaId(relatorioViewModel.Empresa);

                var relatorioDadosViewModel = new List<RelatorioViewModel>();

                foreach (var contaCorrente in contasCorrentes)
                {
                    foreach (var dado in dadosOfx)
                    {
                        relatorioDadosViewModel.Add(new RelatorioViewModel
                        {
                            RazaoEmpresa = contaCorrente.EmpresaId,
                            Date = dado.Data,
                            Type = dado.TipoLancamento,
                            Description = dado.Descricao,
                            TransationValue = dado.ValorOfx,
                        });
                    }
                }

                return relatorioDadosViewModel;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        private static SelectList ConstruirContasContabeisSelectList(IEnumerable<ContaContabil> contasContabeis)
        => new(contasContabeis.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Historico}" }), "Codigo", "Descricao");
        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo", "Razao");


        public async Task<RelatorioViewModel> RelatorioOfx()
        {
            try
            {
                var empresas = _empresaRepository.GetAll();
                var contasContabeis = await _contaContabilRepository.GetAll();
                var relatorioViewModel = new RelatorioViewModel()
                {
                    Empresas = ConstruirEmpresas(empresas),
                    ContasContabeis = ConstruirContasContabeisSelectList(contasContabeis),
                };

                return relatorioViewModel;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
