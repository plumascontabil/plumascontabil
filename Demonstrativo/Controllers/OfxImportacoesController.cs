using Demonstrativo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OFXParser;
using OFXParser.Entities;
using OFXSharp;
using Microsoft.EntityFrameworkCore;
using DomainService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Demonstrativo.Controllers
{
    public class OfxImportacoesController : BaseController
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<object> _logger;
        // private readonly OfxImportacoesDomainService _ofxImportacoesDomainService;


        public OfxImportacoesController(Context context,
            IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, ILogger<object> logger) : base(context, roleManager)
        {
            _context = context;
            _appEnvironment = env;
            _logger = logger;
            //_ofxImportacoesDomainService = ofxImportacoesDomainService;
        }

        public IActionResult Index()
        {
            IniT();

            return View();
        }

        private void IniT()
        {

            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();

            if (ViewBag.EmpresaSeleciodaId != null)
            {
                var idEmp = Convert.ToInt32($"{ViewBag.EmpresaSeleciodaId}");
                var comp = Convert.ToDateTime($"{ViewBag.CompetenciasSelecionadaId}");
                var contaCorrente = _context.ContasCorrentes.Include(f => f.BancoOfx).Where(f => f.EmpresaId == idEmp).ToList();
                //contaCorrente.ForEach(el =>
                //{
                //    el.BancoOfx.UrlImagem = el.BancoOfx.UrlImagem != null ? $"~/Bancos/{el.BancoOfx.UrlImagem.Split("\\").LastOrDefault()}" : "";

                //});


                ViewBag.BancosId = contaCorrente;
                ViewBag.Lotes = _context.OfxLoteLancamento.Where(f => f.EmpresaId == idEmp && f.CompetenciaId == comp).ToList().OrderByDescending(f => f.Data).ToList();
            }
            else
            {
                ViewBag.BancosId = new List<OfxContaCorrente>();
                ViewBag.Lotes = new List<OfxLoteLancamento>();
            }



        }

        [HttpPost]
        public async Task<IActionResult> OfxLoteDelete(int LoteLancamentoId)
        {
            IniT();
            var lote = _context.OfxLoteLancamento.Where(f => f.Id == LoteLancamentoId).FirstOrDefault();
            var lancamentos = _context.OfxLancamentos.Where(f => f.LoteLancamentoId == lote.Id).ToList();

            if (lancamentos.Count > 0)
            {
                var cin = lancamentos.FirstOrDefault().ContaCorrenteId;
                var saldo = _context.SaldoMensal.Where(f => f.Competencia == lote.CompetenciaId && f.ContaCorrenteId == cin).FirstOrDefault();
                if (saldo != null)
                    saldo.Saldo -= lote.Valor;
            }


            lancamentos.ForEach(el =>
            {
                _context.OfxLancamentos.Remove(el);
            });
            _context.SaveChanges();
            _context.Remove(lote);
            _context.SaveChanges();
            ViewBag.Sucesso = "Lote Deletado com Sucesso";
            _logger.LogInformation(((int)EEventLog.Post), "Lote Id: {lote} deleted.", lote.Id);

            IniT();
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> OfxImportar(IFormFile ofxArquivo = null, int? ContaCorrenteId = null, string DescricaoLote = null)
        {
            IniT();

            if (ViewBag.EmpresaSeleciodaId == null)
            {

                ViewBag.Message = "Porfavor, Selecione uma empresa, localizada no topo e Filtre!";
                return View("Index");

            }

            if (ViewBag.CompetenciasSelecionadaId == null)
            {
                // IniT();
                ViewBag.Message = "Porfavor, Selecione uma competência, localizada no topo e Filtre!";
                return View("Index");

            }
            //Listas
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            var autoDescricoes = _context.AutoDescricoes;
            var competenciasId = Convert.ToDateTime($"{ViewBag.CompetenciasSelecionadaId}");
            var empresaId = Convert.ToInt32($"{ViewBag.EmpresaSeleciodaId}");
            var lancamentos = _context.OfxLancamentos.ToList();

            int? BancosId = _context.ContasCorrentes.Where(f => f.Id == ContaCorrenteId).FirstOrDefault().BancoOfxId;
            if (ofxArquivo == null)
            {
                // IniT();
                ViewBag.SemArquivo = "É necessário escolher um arquivo para envio do OFX!";
                return View("Index");
            }

            if (!ofxArquivo.FileName.ToLower().Contains("ofx"))
            {
                // IniT();
                ViewBag.SemArquivo = $"Esse arquivo não possui o formato correto! Importe um arquivo OFX.";
                return View("Index");
            }

            if (!ContaCorrenteId.HasValue)
            {
                // Init();
                ViewBag.Message = " Não possui o ID da conta corrente!";
                return View("Index");

            }

            if (!BancosId.HasValue)
            {
                // IniT();
                ViewBag.SemBanco = "É necessário escolher um banco para envio do OFX!";
                return View("Index");
            }

            if (string.IsNullOrEmpty(DescricaoLote))
            {
                ViewBag.Message = "Descrição do Lote é obrigatória!";
                return View("Index");
            }

            var bancoEscolhido = _context.OfxBancos.FirstOrDefault(f => f.Id == BancosId.Value);

            //Views Models
            var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
            var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();
            var saldoMensalViewModel = new SaldoMensalViewModel();
            OfxLoteLancamento loteAchado = null;
            decimal saldo = 0;
            //If Leitura arquivo não nulo
            if (ofxArquivo != null)
            {
                if (!Directory.Exists($"{_appEnvironment.WebRootPath}\\Temp"))
                {
                    Directory.CreateDirectory($"{_appEnvironment.WebRootPath}\\Temp");
                }
                //Caminho para salvar arquivo no servidor
                string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Temp\\{ofxArquivo.FileName}";
                string ofx = string.Empty;
                using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
                {
                    await ofxArquivo.CopyToAsync(stream);
                }

                ofx = System.IO.File.ReadAllText(caminhoDestinoArquivo);




                if (ofx.Contains("<DTSERVER>00000000000000"))
                {
                    ofx = ofx.Replace("<DTSERVER>00000000000000", $"<DTSERVER>{DateTime.Now.ToString("yyyyMMddhhmmss")}");
                }

                if (ofx.Contains("<DTASOF>00000000") || ofx.Contains("<DTASOF>00000000000000"))
                {
                    ofx = ofx.Replace("<DTASOF>00000000000000", $"<DTASOF>{DateTime.Now.ToString("yyyyMMddhhmmss")}");
                    ofx = ofx.Replace("<DTASOF>00000000", $"<DTASOF>{DateTime.Now.ToString("yyyyMMddhhmmss")}");

                }
                var x = ofx.LastIndexOf("<BALAMT>");
                if (ofx.Substring(x).Contains(","))
                {
                    var Texto = ofx.Substring(x).Trim();
                    ofx = ofx.Replace(Texto, Texto.Replace(",", "."));
                }

                if (!ofx.Contains("</SEVERITY>"))
                {
                    var indx = ofx.IndexOf("<SEVERITY>");
                    var count = "<SEVERITY>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("</"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</SEVERITY>"}";
                    // stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";


                    var stringOfx = ofx.Substring(indx + count - 1 + index - 2);
                    if (!stringOfx.Contains("</SEVERITY>"))
                    {
                        var indxx = stringOfx.IndexOf("<SEVERITY>");
                        var countx = "<SEVERITY>".Length;
                        var indexx = 1;
                        while (!stringOfx.Substring(indxx + countx - 1, indexx).Contains("</"))
                        {
                            indexx++;
                        }
                        var stringFf = $"{stringOfx.Substring(0, indxx + countx)}";

                        stringFf += $"{stringOfx.Substring(indxx + countx - 1, indexx - 2)}";
                        stringFf += $"{"</SEVERITY>"}";
                        stringFf += $"{stringOfx.Substring(indxx + countx - 1 + indexx - 2)}";


                        stringF += stringFf;
                    }
                    ofx = stringF;




                }


                if (!ofx.Contains("</LANGUAGE>"))
                {
                    var indx = ofx.IndexOf("<LANGUAGE>");
                    var count = "<LANGUAGE>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("</"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</LANGUAGE>"}";
                    stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";


                    //var stringOfx = ofx.Substring(indx + count - 1 + index - 2);
                    //if (!stringOfx.Contains("</LANGUAGE>"))
                    //{
                    //    var indxx = stringOfx.IndexOf("<LANGUAGE>");
                    //    var countx = "<LANGUAGE>".Length;
                    //    var indexx = 1;
                    //    while (!stringOfx.Substring(indxx + countx - 1, indexx).Contains("</"))
                    //    {
                    //        indexx++;
                    //    }
                    //    var stringFf = $"{stringOfx.Substring(0, indxx + countx)}";

                    //    stringFf += $"{stringOfx.Substring(indxx + countx - 1, indexx - 2)}";
                    //    stringFf += $"{"</LANGUAGE>"}";
                    //    stringFf += $"{stringOfx.Substring(indxx + countx - 1 + indexx - 2)}";


                    //    stringF += stringFf;
                    //}
                    ofx = stringF;




                }

                if (!ofx.Contains("</CODE>"))
                {
                    var indx = ofx.IndexOf("<CODE>");
                    var count = "<CODE>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("<"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</CODE>"}";
                    // stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";


                    var stringOfx = ofx.Substring(indx + count - 1 + index - 2);
                    if (!stringOfx.Contains("</CODE>"))
                    {
                        var indxx = stringOfx.IndexOf("<CODE>");
                        var countx = "<CODE>".Length;
                        var indexx = 1;
                        while (!stringOfx.Substring(indxx + countx - 1, indexx).Contains("<"))
                        {
                            indexx++;
                        }
                        var stringFf = $"{stringOfx.Substring(0, indxx + countx)}";

                        stringFf += $"{stringOfx.Substring(indxx + countx - 1, indexx - 2)}";
                        stringFf += $"{"</CODE>"}";
                        stringFf += $"{stringOfx.Substring(indxx + countx - 1 + indexx - 2)}";


                        stringF += stringFf;
                    }
                    ofx = stringF;




                }


                if (!ofx.Contains("</DTSERVER>"))
                {
                    var indx = ofx.IndexOf("<DTSERVER>");
                    var count = "<DTSERVER>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("<"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</DTSERVER>"}";
                    stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";


                    //var stringOfx = ofx.Substring(indx + count - 1 + index - 2);
                    //if (!stringOfx.Contains("</DTSERVER>"))
                    //{
                    //    var indxx = stringOfx.IndexOf("<CODE>");
                    //    var countx = "<CODE>".Length;
                    //    var indexx = 1;
                    //    while (!stringOfx.Substring(indxx + countx - 1, indexx).Contains("<"))
                    //    {
                    //        indexx++;
                    //    }
                    //    var stringFf = $"{stringOfx.Substring(0, indxx + countx)}";

                    //    stringFf += $"{stringOfx.Substring(indxx + countx - 1, indexx - 2)}";
                    //    stringFf += $"{"</DTSERVER>"}";
                    //    stringFf += $"{stringOfx.Substring(indxx + countx - 1 + indexx - 2)}";


                    //    stringF += stringFf;
                    //}
                    ofx = stringF;




                }


                if (!ofx.Contains("</BANKID>"))
                {
                    var indx = ofx.IndexOf("<BANKID>");
                    var count = "<BANKID>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("<"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</BANKID>"}";
                    stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";
                    ofx = stringF;
                }

                if (!ofx.Contains("</ACCTID>"))
                {
                    var indx = ofx.IndexOf("<ACCTID>");
                    var count = "<ACCTID>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("<"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</ACCTID>"}";
                    stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";
                    ofx = stringF;
                }
                if (!ofx.Contains("</ACCTTYPE>"))
                {
                    var indx = ofx.IndexOf("<ACCTTYPE>");
                    var count = "<ACCTTYPE>".Length;
                    var index = 1;
                    while (!ofx.Substring(indx + count - 1, index).Contains("</"))
                    {
                        index++;
                    }
                    var stringF = $"{ofx.Substring(0, indx + count)}";

                    stringF += $"{ofx.Substring(indx + count - 1, index - 2)}";
                    stringF += $"{"</ACCTTYPE>"}";
                    stringF += $"{ofx.Substring(indx + count - 1 + index - 2)}";


                    ofx = stringF;




                }








                //Extraindo conteudo do arquivo em um objeto do tipo Extract
                var dados = ofx.Split("\n");

                if (!dados.FirstOrDefault().Contains("<"))
                {
                    var arioi = dados.ToList();

                    for (int i = 0; i < 10; i++)
                    {
                        arioi.RemoveAt(0);
                    }


                    ofx = string.Join("\n", arioi);
                }
                dados = ofx.Split("\n");

                for (int i = 0; i < dados.Length; i++)
                {
                    if (dados[i].Contains("<TRNTYPE>") && !dados[i].Contains("</TRNTYPE>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</TRNTYPE>")}\r";
                    }
                    if (dados[i].Contains("<DTPOSTED>") && !dados[i].Contains("</DTPOSTED>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</DTPOSTED>")}\r";
                    }
                    if (dados[i].Contains("<TRNAMT>") && !dados[i].Contains("</TRNAMT>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</TRNAMT>")}\r";
                    }
                    if (dados[i].Contains("<FITID>") && !dados[i].Contains("</FITID>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</FITID>")}\r";
                    }
                    if (dados[i].Contains("<CHECKNUM>") && !dados[i].Contains("</CHECKNUM>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</CHECKNUM>")}\r";
                    }
                    if (dados[i].Contains("<MEMO>") && !dados[i].Contains("</MEMO>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</MEMO>")}\r";
                    }
                    if (dados[i].Contains("<DTSTART>") && !dados[i].Contains("</DTSTART>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</DTSTART>")}\r";
                    }
                    if (dados[i].Contains("<CURDEF>") && !dados[i].Contains("</CURDEF>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</CURDEF>")}\r";
                    }
                    if (dados[i].Contains("<DTEND>") && !dados[i].Contains("</DTEND>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</DTEND>")}\r";
                    }
                    if (dados[i].Contains("<BALAMT>") && !dados[i].Contains("</BALAMT>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</BALAMT>")}\r";
                    }

                    if (dados[i].Contains("<DTASOF>") && !dados[i].Contains("</DTASOF>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</DTASOF>")}\r";
                    }

                    if (dados[i].Contains("<TRNUID>") && !dados[i].Contains("</TRNUID>"))
                    {
                        dados[i] = $"{dados[i].Replace("\r", "</TRNUID>")}\r";
                    }
                }
                ofx = string.Join("\n", dados.ToList());



                IList<OFXParser.Entities.Transaction> transactions = new List<OFXParser.Entities.Transaction>();
                BankAccount bankAccount = null;
                try
                {
                    try
                    {
                        Extract extratoBancario = Parser.GenerateExtract(caminhoDestinoArquivo);
                        if (extratoBancario != null)
                        {
                            XElement doc = XElement.Parse(ofx);
                            //queryiny the XElement
                            var imps = (from c in doc.Descendants("LEDGERBAL")
                                        select decimal.Parse(c.Element("BALAMT").Value.Replace(",", "."),
                                                                   NumberFormatInfo.InvariantInfo)).FirstOrDefault();
                            //var documento = new OFXDocumentParser();
                            //var dadoDocumento = documento.Import(ofx);
                            saldo = imps;
                            //}
                            bankAccount = extratoBancario.BankAccount;
                            transactions = extratoBancario.Transactions;
                        }
                    }
                    catch (Exception ex)
                    {
                       
                        XElement doc = XElement.Parse(ofx);

                        string formatString = "yyyyMMddHHmmss";
                        transactions = (from c in doc.Descendants("STMTTRN")
                                        select c).Select(x => new OFXParser.Entities.Transaction()
                                        {
                                            Type = x.Element("TRNTYPE").Value,
                                            TransactionValue = double.Parse(x.Element("TRNAMT").Value),
                                            Description = x.Element("MEMO").Value,
                                            Date = DateTime.ParseExact(x.Element("DTPOSTED").Value.Replace("[-3:GMT]", ""), formatString, null),
                                            Id = x.Element("FITID").Value.Trim().Split("-").FirstOrDefault().Trim(),
                                            Checksum = long.Parse(x.Element("FITID").Value.Trim().Split("-").LastOrDefault())

                                        }).ToList();

                        bankAccount = (from c in doc.Descendants("BANKACCTFROM")
                                       select c).Select(x => new OFXParser.Entities.BankAccount()
                                       {
                                           Bank = new OFXParser.Entities.Bank(Convert.ToInt32(x.Element("BANKID").Value), x.Element("BANKID").Value),
                                           Type = x.Element("ACCTTYPE").Value,
                                           AccountCode = x.Element("ACCTID").Value,
                                           AgencyCode = ""
                                       }).FirstOrDefault();

                        //queryiny the XElement
                        var imps = (from c in doc.Descendants("LEDGERBAL")
                                    select decimal.Parse(c.Element("BALAMT").Value.Replace(",", "."),
                                                               NumberFormatInfo.InvariantInfo)).FirstOrDefault();
                        //var documento = new OFXDocumentParser();
                        //var dadoDocumento = documento.Import(ofx);
                        saldo = imps;
                        //}
                    }







                    //if (extratoBancario.
                    //    Transactions.Where(f => (f.Date.Month != competenciasId.Month && f.Date.Year == competenciasId.Year)
                    //                         || (f.Date.Month != competenciasId.Month && f.Date.Year != competenciasId.Year)
                    //                         || (f.Date.Year != competenciasId.Year)).Count() > 0)
                    //{
                    //    ViewBag.Message = "Este OFX tem datas que não correspondem a Competência selecionada!";
                    //    return View("Index");
                    //}
                }
                catch (Exception e)
                {

                    var erro = e.Message == "Unable to parse date" ? "Data do arquivo Inválida" : e.Message;
                    ViewBag.Message = erro;
                    return View("Index");
                }

                var dadosContaCorrente = _context.ContasCorrentes.FirstOrDefault(c =>
                (c.NumeroConta == bankAccount.AccountCode
                || c.Acctid == bankAccount.AccountCode)
                && c.EmpresaId == empresaId);

                if (dadosContaCorrente != null)
                {
                    if (dadosContaCorrente.Id != ContaCorrenteId.Value)
                    {
                        ViewBag.Message = "A conta corrente selecionada não é informada no arquivo OFX!";
                        return View("Index");

                    }

                    saldoMensalViewModel = new SaldoMensalViewModel()
                    {
                        SaldoMensal = saldo,
                        Competencia = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId),
                        ContaCorrenteId = dadosContaCorrente.Id,
                    };
                }
                else
                {
                    // IniT();
                    ViewBag.ContaCorrenteNaoEncontrada = true;
                    return View("Index");
                }
                //extratoBancario.Transactions.ToList().ForEach(el =>
                //{
                //    saldo += Convert.ToDecimal(el.TransactionValue);
                //});

                //saldoMensalViewModel.SaldoMensal = saldo;

                var banco = _context.OfxBancos
                       .FirstOrDefault(b => b.Codigo == Convert.ToInt32(bankAccount.Bank.Code));

                if (banco == null)
                {
                    ViewBag.SemBanco = "Banco Inexistente";
                    return View("Index");
                }

                if (banco.Id != bancoEscolhido.Id)
                {
                    ViewBag.SemBanco = "O Banco do Arquivo não confere com o banco escolhido";
                    return View("Index");
                }
                var bancoViewModel = new OfxBancoViewModel()
                {
                    Id = banco.Id,
                    Codigo = banco.Codigo,
                    Nome = banco.Nome
                };
                //varrendo arquivo e adicionado as ViewsModel

                transactions.ToList().ForEach(dados =>
                {
                    if (lancamentos.Any(c => c.Documento == dados.Id && c.ContaCorrenteId == dadosContaCorrente.Id) == false)
                    {
                        //if (autoDescricoes.FirstOrDefault(a => a.Descricao == dados.Description) == null)
                        //{
                        //    lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                        //    {
                        //        Id = dados.Id,
                        //        TransationValue = Convert.ToDecimal(dados.TransactionValue),
                        //        Description = dados.Description,
                        //        Date = dados.Date,
                        //        CheckSum = dados.Checksum,
                        //        Type = dados.Type,
                        //        SaldoMensal = saldoMensalViewModel,
                        //        LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes)
                        //    });
                        //}
                        //else
                        //{

                        //}
                        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                        {
                            Id = dados.Id,
                            TransationValue = Convert.ToDecimal(dados.TransactionValue),
                            Description = dados.Description,
                            Date = dados.Date,
                            CheckSum = dados.Checksum,
                            Type = dados.Type,
                            SaldoMensal = saldoMensalViewModel,
                            LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                            Dividir = null,
                            Mostrar = ((dados.Date.Month == competenciasId.Month) && (dados.Date.Year == competenciasId.Year))
                            //LancamentoPadraoSelecionado = autoDescricoes.FirstOrDefault(a => a.Descricao == dados.Description).LancamentoPadraoId
                        }); ;

                        contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                        {
                            OfxLancamentos = lancamentoOfxViewModel,
                            NumeroConta = bankAccount.AccountCode,
                        };

                        extratoBancarioViewModel = new ExtratoBancarioViewModel()
                        {
                            Empresas = ConstruirEmpresas(empresas),
                            ContasCorrentes = contaCorrenteViewModel,
                            Banco = bancoViewModel,
                            DescricaoLote = DescricaoLote

                        };
                    }
                    else
                    {
                        var dadoss = _context.OfxLancamentos.Include(f => f.Lote).Where(c => c.Documento == dados.Id && c.ContaCorrenteId == dadosContaCorrente.Id).FirstOrDefault();
                        loteAchado = dadoss.Lote;
                    }
                });
                //foreach (var dados in )
                ////foreach (var dados in extratoBancario.Transactions)
                //{




                //}
                if (ViewBag.SemBanco != null)
                {
                    // IniT();
                    return View("Index");

                }
                extratoBancarioViewModel.EmpresaSelecionada = ViewBag.EmpresaSeleciodaId;

                //Deletando arquivo do servidor
                System.IO.File.Delete(caminhoDestinoArquivo);
                System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");
            }
            try
            {
                _logger.LogInformation(((int)EEventLog.Post), "Import created.");
            }
            catch (Exception ex)
            {

                throw ex;
            }


            if (extratoBancarioViewModel.Banco == null)
            {
                // IniT();
                ViewBag.Message = $"Este arquivo já foi importado, na Competência {loteAchado.Data.ToString("MM/yyyy")} !";
                return View("Index");
            }
            else return View("Contas", extratoBancarioViewModel);
        }

        [HttpPost]
        public IActionResult OfxReimportar(ExtratoBancarioViewModel extratoViewModel = null)
        {
            IniT();
            //var extratoBancarioViewModel = _ofxImportacoesDomainService.OfxReimportar(extratoViewModel);

            //Listas
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            var autoDescricoes = _context.AutoDescricoes;
            var competenciasId = Convert.ToDateTime($"{ViewBag.CompetenciasSelecionadaId}");

            //Views Models
            var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
            var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();
            var Divider = false;
            var idDivider = string.Empty;
            var tipo = 0;
            var Minus = 1;
            var cloneLanc = new OfxLancamentoViewModel();
            extratoViewModel.ContasCorrentes.OfxLancamentos = extratoViewModel.ContasCorrentes.OfxLancamentos.Where(f => !f.Selecionando).ToList();
            foreach (var dados in extratoViewModel.ContasCorrentes.OfxLancamentos)
            {
                if (dados.Dividir.HasValue && dados.Dividir.Value)
                {
                    Minus = (dados.TransationValue < 0 ? -1 : 1);
                    dados.TransationValue = (Math.Abs(dados.TransationValue) - Math.Abs(extratoViewModel.LancamentoManual.Valor)) * Minus;
                    cloneLanc = dados;
                    Divider = true;
                    tipo = Convert.ToInt32(extratoViewModel.LancamentoManual.TipoSelecionado);
                    cloneLanc.LancamentoPadraoSelecionado = tipo;
                }

                lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                {
                    IdBd = dados.IdBd,
                    Id = dados.Id,
                    TransationValue = dados.TransationValue,
                    Description = dados.Description,
                    Date = dados.Date,
                    CheckSum = dados.CheckSum,
                    Type = dados.Type,
                    LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                    Mostrar = ((dados.Date.Month == competenciasId.Month) && (dados.Date.Year == competenciasId.Year)),
                    LancamentoPadraoSelecionado = dados.LancamentoPadraoSelecionado,
                    Dividir = null
                });


                var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoViewModel.Banco.Codigo);

                var bancoViewModel = new OfxBancoViewModel()
                {
                    Id = banco.Id,
                    Codigo = banco.Codigo,
                    Nome = banco.Nome
                };

                contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                {
                    OfxLancamentos = lancamentoOfxViewModel,
                    NumeroConta = extratoViewModel.ContasCorrentes.NumeroConta
                };
                extratoBancarioViewModel = new ExtratoBancarioViewModel()
                {
                    Empresas = ConstruirEmpresas(empresas),
                    ContasCorrentes = contaCorrenteViewModel,
                    Banco = bancoViewModel
                };
            }

            if (extratoViewModel.LancamentoManual != null)
            {
                //var ids = extratoViewModel.ContasCorrentes.OfxLancamentos.Max(x => x.Id) + 1;

                if (Divider)
                {
                    var manu = new OfxLancamentoViewModel()
                    {
                        TransationValue = Math.Abs(extratoViewModel.LancamentoManual.Valor) * Minus,
                        Description = cloneLanc.Description,
                        Date = cloneLanc.Date,
                        CheckSum = cloneLanc.CheckSum,
                        Type = cloneLanc.Type,
                        LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                        SaldoMensal = cloneLanc.SaldoMensal,
                        Mostrar = cloneLanc.Mostrar,
                        Id = cloneLanc.Id,
                        LancamentoPadraoSelecionado = tipo,
                        Dividir = null

                    };

                    var ind = lancamentoOfxViewModel.FindIndex(f => f.Id == cloneLanc.Id);
                    lancamentoOfxViewModel.Insert(ind + 1, manu);
                }
                else
                {
                    lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                    {
                        TransationValue = extratoViewModel.LancamentoManual.Valor * (extratoViewModel.LancamentoManual.TipoSelecionado == "DEBIT" ? -1 : 1),
                        Description = extratoViewModel.LancamentoManual.Descricao,
                        Date = extratoViewModel.LancamentoManual.Data,
                        CheckSum = 1,
                        Type = extratoViewModel.LancamentoManual.TipoSelecionado,
                        LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                        SaldoMensal = new SaldoMensalViewModel(),
                        Mostrar = ((extratoViewModel.LancamentoManual.Data.Month == competenciasId.Month) && (extratoViewModel.LancamentoManual.Data.Year == competenciasId.Year)),
                        Id = Divider ? idDivider : "MANUAL",
                        LancamentoPadraoSelecionado = Divider ? tipo : 0,
                        Dividir = null
                    });

                }

            }
            //decimal saldo = 0;
            //extratoBancarioViewModel.ContasCorrentes.OfxLancamentos.ForEach(el =>
            //{
            //    saldo += el.TransationValue;
            //});

            //extratoBancarioViewModel.ContasCorrentes.OfxLancamentos.ForEach(el =>
            // {
            //     if (el.SaldoMensal != null)
            //     {
            //         el.SaldoMensal.SaldoMensal = saldo;
            //     }
            //     else
            //     {
            //         el.SaldoMensal = new SaldoMensalViewModel()
            //         {
            //             SaldoMensal = saldo
            //         };
            //     }

            // });

            _logger.LogInformation(((int)EEventLog.Post), "ReImport created.");

            extratoBancarioViewModel.DescricaoLote = extratoViewModel.DescricaoLote;

            extratoBancarioViewModel.LancamentoManual = new OfxLancamentoManualViewModel();
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            //
            return View("Contas", extratoBancarioViewModel);
        }

        [HttpPost]
        public IActionResult OfxReimportarDelete(ExtratoBancarioViewModel extratoViewModel = null)
        {
            IniT();
            try
            {
                var lancamentosPadroes = _context.LancamentosPadroes.ToList();

                extratoViewModel.ContasCorrentes.OfxLancamentos = extratoViewModel.ContasCorrentes.OfxLancamentos.Where(f => !f.Selecionando).ToList();

                foreach (var item in extratoViewModel.ContasCorrentes.OfxLancamentos)
                {
                    item.LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes);
                }

                return View("Contas", extratoViewModel);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult OfxReimportarAtivar(ExtratoBancarioViewModel extratoViewModel = null)
        {
            IniT();
            try
            {
                var lancamentosPadroes = _context.LancamentosPadroes.ToList();
                var index = extratoViewModel.ContasCorrentes.OfxLancamentos.FindIndex(f => f.Dividir.HasValue && f.Dividir.Value);
                extratoViewModel.ContasCorrentes.OfxLancamentos[index].Inativar = null;
                extratoViewModel.ContasCorrentes.OfxLancamentos[index].Dividir = null;
                foreach (var item in extratoViewModel.ContasCorrentes.OfxLancamentos)
                {
                    item.LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes);
                }
                return View("Contas", extratoViewModel);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public IActionResult OfxReimportarInativar(ExtratoBancarioViewModel extratoViewModel = null)
        {
            IniT();
            try
            {
                var lancamentosPadroes = _context.LancamentosPadroes.ToList();
                var index = extratoViewModel.ContasCorrentes.OfxLancamentos.FindIndex(f => f.Dividir.HasValue && f.Dividir.Value);
                extratoViewModel.ContasCorrentes.OfxLancamentos[index].Inativar = true;
                extratoViewModel.ContasCorrentes.OfxLancamentos[index].Dividir = null;
                foreach (var item in extratoViewModel.ContasCorrentes.OfxLancamentos)
                {
                    item.LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes);
                }
                return View("Contas", extratoViewModel);

            }
            catch (Exception)
            {

                throw;
            }
        }

        //[HttpPost]
        //public IActionResult OfxReimportEdit(ExtratoBancarioViewModel extratoViewModel = null)
        //{

        //    try
        //    {

        //        AdicionarCompetenciaMesAtual();
        //        CarregarEmpresasCompetencias();
        //        var lanc = extratoViewModel.ContasCorrentes.OfxLancamentos.Where(f => f.Selecionando).FirstOrDefault();
        //        extratoViewModel.LancamentoManual.Data = lanc.Date;
        //        extratoViewModel.LancamentoManual.Descricao = lanc.Description;
        //        extratoViewModel.LancamentoManual.ValorInput = lanc.TransationValue.ToString();
        //        extratoViewModel.LancamentoManual.TipoSelecionado = lanc.Type;                

        //        ViewBag.OpenModal = true;


        //        return View("Contas", extratoViewModel);

        //    }
        //    catch (global::System.Exception)
        //    {

        //        throw;
        //    }
        //}

        [HttpPost]
        public IActionResult OfxReimportarLote(int LoteLancamentoId)
        {
            IniT();
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            var autoDescricoes = _context.AutoDescricoes;
            var competenciasId = Convert.ToDateTime($"{ViewBag.CompetenciasSelecionadaId}");
            var lote = _context.OfxLoteLancamento.Where(f => f.Id == LoteLancamentoId).FirstOrDefault();
            var lancamentos = _context.OfxLancamentos.Include(f => f.ContaCorrente).ThenInclude(f => f.BancoOfx).Where(f => f.LoteLancamentoId == lote.Id).ToList();

            var saldo = _context.SaldoMensal.Where(f => f.ContaCorrenteId == lancamentos.FirstOrDefault().ContaCorrenteId && f.Competencia == competenciasId).FirstOrDefault();
            if (lancamentos.Count == 0)
            {
                ViewBag.Message = "Não tem nenhum lançamento!";
                _context.OfxLoteLancamento.Remove(lote);
                return View("Index");
            }

            ExtratoBancarioViewModel extratoViewModel = new ExtratoBancarioViewModel()
            {
                Empresas = ConstruirEmpresas(empresas),
                Banco = new OfxBancoViewModel()
                {
                    Codigo = lancamentos.FirstOrDefault().ContaCorrente.BancoOfx.Codigo,
                    Id = lancamentos.FirstOrDefault().ContaCorrente.BancoOfx.Id,
                    Nome = lancamentos.FirstOrDefault().ContaCorrente.BancoOfx.Nome
                },
                LancamentoManual = null
            };

            extratoViewModel.DescricaoLote = lote.Descricao;
            extratoViewModel.LoteLancamentoid = lote.Id;
            extratoViewModel.EmpresaSelecionada = lote.EmpresaId;
            extratoViewModel.ContasCorrentes = new OfxContaCorrenteViewModel()
            {
                OfxLancamentos = new List<OfxLancamentoViewModel>(),
                NumeroConta = lancamentos.FirstOrDefault().ContaCorrente.Acctid
            };
            lancamentos.ForEach(el =>
            {
                extratoViewModel.ContasCorrentes.OfxLancamentos.Add(new OfxLancamentoViewModel()
                {
                    IdBd = el.Id,
                    TransationValue = el.ValorOfx,
                    Date = el.Data,
                    Type = el.TipoLancamento,
                    Description = el.Descricao,
                    Inativar = el.Inativar,
                    SaldoMensal = new SaldoMensalViewModel()
                    {
                        SaldoMensal = saldo.Saldo,
                        Competencia = saldo.Competencia,
                        ContaCorrenteId = saldo.ContaCorrenteId,

                    },
                    Id = el.Documento,
                    LancamentoPadraoSelecionado = el.LancamentoPadraoId.HasValue ? el.LancamentoPadrao.Codigo.Value : 0,
                    LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                    Mostrar = ((el.Data.Month == competenciasId.Month) && (el.Data.Year == competenciasId.Year))

                });
            });


            //var extratoBancarioViewModel = _ofxImportacoesDomainService.OfxReimportar(extratoViewModel);

            //Listas


            //Views Models
            var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
            var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();

            foreach (var dados in extratoViewModel.ContasCorrentes.OfxLancamentos)
            {


                lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                {
                    IdBd = dados.IdBd,
                    Id = dados.Id,
                    TransationValue = dados.TransationValue,
                    Description = dados.Description,
                    Date = dados.Date,
                    CheckSum = dados.CheckSum,
                    Type = dados.Type,
                    LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                    LancamentoPadraoSelecionado = dados.LancamentoPadraoSelecionado,
                    Mostrar = ((dados.Date.Month == competenciasId.Month) && (dados.Date.Year == competenciasId.Year)),
                    Inativar = dados.Inativar,
                    SaldoMensal = new SaldoMensalViewModel()
                    {
                        SaldoMensal = saldo.Saldo,
                        Competencia = saldo.Competencia,
                        ContaCorrenteId = saldo.ContaCorrenteId,

                    },

                }); ;

                var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoViewModel.Banco.Codigo);

                var bancoViewModel = new OfxBancoViewModel()
                {
                    Id = banco.Id,
                    Codigo = banco.Codigo,
                    Nome = banco.Nome
                };

                contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                {
                    OfxLancamentos = lancamentoOfxViewModel,
                    NumeroConta = extratoViewModel.ContasCorrentes.NumeroConta
                };
                extratoBancarioViewModel = new ExtratoBancarioViewModel()
                {
                    Empresas = ConstruirEmpresas(empresas),
                    ContasCorrentes = contaCorrenteViewModel,
                    Banco = bancoViewModel
                };
            }

            if (extratoViewModel.LancamentoManual != null)
            {
                //var ids = extratoViewModel.ContasCorrentes.OfxLancamentos.Max(x => x.Id) + 1;
                lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                {
                    TransationValue = extratoViewModel.LancamentoManual.Valor * (extratoViewModel.LancamentoManual.TipoSelecionado == "DEBIT" ? -1 : 1),
                    Description = extratoViewModel.LancamentoManual.Descricao,
                    Date = extratoViewModel.LancamentoManual.Data,
                    CheckSum = 1,
                    Type = extratoViewModel.LancamentoManual.TipoSelecionado,
                    LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                    SaldoMensal = new SaldoMensalViewModel(),
                    Mostrar = ((extratoViewModel.LancamentoManual.Data.Month == competenciasId.Month) && (extratoViewModel.LancamentoManual.Data.Year == competenciasId.Year)),
                    Id = "MANUAL"

                });
            }
            _logger.LogInformation(((int)EEventLog.Post), "ReImport Lote created.");

            extratoBancarioViewModel.DescricaoLote = extratoViewModel.DescricaoLote;
            extratoBancarioViewModel.LoteLancamentoid = lote.Id;
            extratoBancarioViewModel.EmpresaSelecionada = lote.EmpresaId;
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            //
            return View("Contas", extratoBancarioViewModel);
        }


        [HttpPost]
        public IActionResult OfxSalvar(ExtratoBancarioViewModel dados)
        {
            //var extratoBancarioViewModel = _ofxImportacoesDomainService.OfxSalvar(dados);


            //if (dados.ContasCorrentes.OfxLancamentos.Any(x => x.LancamentoPadraoSelecionado == 0))
            //{
            //    ViewBag.LancamentoPadraoSelecionadoNotSelect = "Existe lançamentos sem ser atribuidos a contas contábeis";
            //    return View("Contas", dados);
            //}
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            IniT();
            dados.EmpresaSelecionada = ViewBag.EmpresaSeleciodaId;

            dados.ContasCorrentes.OfxLancamentos.Where(f => f.Mostrar).ToList().ForEach(el =>
            {
                el.LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes);
            });
            if (dados.EmpresaSelecionada == 0)
            {
                ViewBag.LancamentoPadraoSelecionadoNotSelect = "Selecione a empresa";
                return View("Contas", dados);
            }

            if (dados.ContasCorrentes.OfxLancamentos.Where(f => f.Mostrar && !(f.Inativar.HasValue && f.Inativar.Value)).Any(f => f.LancamentoPadraoSelecionado == 0))
            {
                ViewBag.LancamentoPadraoSelecionadoNotSelect = "É necessário realizar todos os lançamentos Contábeis para prossegui!";
                return View("Contas", dados);
            }

            var data = Convert.ToDateTime($"{ViewBag.CompetenciasSelecionadaId}");

            decimal saldoMensalTotal = 0;
            var lote = new OfxLoteLancamento();
            // Fazer Toda a parte do Lote
            if (dados.LoteLancamentoid.HasValue)
            {
                lote = _context.OfxLoteLancamento.Where(f => f.Id == dados.LoteLancamentoid.Value).FirstOrDefault();
                //dados.ContasCorrentes.OfxLancamentos.Where(f => !f.Inativar.HasValue || !f.Inativar.Value).ToList().ForEach(el =>
                //  {
                //      lote.Valor += el.TransationValue;
                //  });
                lote.Valor = dados.ContasCorrentes.OfxLancamentos.FirstOrDefault().SaldoMensal.SaldoMensal;
                //_context.OfxLoteLancamento.Add(lote);
                _context.SaveChanges();
            }
            else
            {
                lote = new OfxLoteLancamento()
                {
                    Data = DateTime.Now,
                    Descricao = dados.DescricaoLote,
                    Valor = dados.ContasCorrentes.OfxLancamentos.FirstOrDefault().SaldoMensal.SaldoMensal,
                    EmpresaId = ViewBag.EmpresaSeleciodaId,
                    CompetenciaId = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId)
                };
                //dados.ContasCorrentes.OfxLancamentos.Where(el => (el.Date.Month == data.Month && el.Date.Year == data.Year)).ToList().ForEach(el =>
                //{

                //    lote.Valor += el.TransationValue;
                //});
                _context.OfxLoteLancamento.Add(lote);
                _context.SaveChanges();
            }
            var contaCorrente = _context.ContasCorrentes.FirstOrDefault(c => c.Acctid == dados.ContasCorrentes.NumeroConta);

            try
            {


                SaldoMensal saldoMensalId = null;
                if (contaCorrente != null)
                {
                    saldoMensalId = _context.SaldoMensal.FirstOrDefault(s => s.Competencia == data
                                                                        && s.ContaCorrenteId == contaCorrente.Id);
                }
                if (saldoMensalId == null)
                {
                    var saldo = new SaldoMensal()
                    {
                        Competencia = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId),// dado.SaldoMensal.Competencia,
                        Saldo = dados.ContasCorrentes.OfxLancamentos.FirstOrDefault().SaldoMensal.SaldoMensal,
                        ContaCorrenteId = contaCorrente.Id
                    };
                    _context.SaldoMensal.Add(saldo);
                }
                else
                {
                    saldoMensalId.Saldo = lote.Valor;
                }

                _context.SaveChanges();
            }
            catch (Exception E)
            {
                // IniT();
                ViewBag.ContaCorrenteNaoEncontrada = true;
                return View("Index");
            }


            foreach (var dado in dados.ContasCorrentes.OfxLancamentos.Where(f => f.Mostrar).ToList())
            {
                var cc = contaCorrente != null;
                var banco = _context.OfxBancos.Any(b => b.Codigo == dados.Banco.Codigo);
                var lancamentoPadrao = _context.LancamentosPadroes.FirstOrDefault(c => c.Codigo == dado.LancamentoPadraoSelecionado);
                if (banco == false)
                {
                    //banco não cadastrado
                }

                if (cc == false)
                {
                    //Todo: adicionar Conta corrente
                    _context.ContasCorrentes.Add(new OfxContaCorrente()
                    {
                        NumeroConta = dados.ContasCorrentes.NumeroConta,
                        EmpresaId = _context.Empresas.FirstOrDefault(e => e.Codigo == dados.EmpresaSelecionada).Codigo,
                        BancoOfxId = _context.OfxBancos.FirstOrDefault(b => b.Codigo == dados.Banco.Codigo).Id,
                    });
                    _context.SaveChanges();
                }

                //var contaCorrente = _context.ContasCorrentes.FirstOrDefault(c => c.NumeroConta == dados.ContasCorrentes.NumeroConta);

                //var saldoMensalId = _context.SaldoMensal.FirstOrDefault(s => s.Competencia == Convert.ToDateTime(dado.Date.ToString("yyyy/MM"))
                //                                                        && s.ContaCorrenteId == contaCorrente.Id);
                //if (saldoMensalId == null)
                //{
                //    _context.SaldoMensal.Add(new SaldoMensal()
                //    {
                //        Competencia = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId),// dado.SaldoMensal.Competencia,
                //        Saldo = dado.SaldoMensal.SaldoMensal == 0 ? lote.Valor : dado.SaldoMensal.SaldoMensal,
                //        ContaCorrenteId = dado.SaldoMensal.ContaCorrenteId
                //    });
                //}
                //else
                //{
                //    //saldoMensalId.Saldo += dado.SaldoMensal.SaldoMensal == 0 ? lote.Valor : dado.SaldoMensal.SaldoMensal;
                //}

                if (dado.IdBd.HasValue)
                {
                    var lanc = _context.OfxLancamentos.Where(f => f.Id == dado.IdBd.Value).FirstOrDefault();
                    lanc.Inativar = dado.Inativar;
                    lanc.Documento = dado.Id;
                    lanc.TipoLancamento = dado.Type;
                    lanc.Descricao = dado.Description;
                    lanc.ValorOfx = dado.TransationValue;
                    lanc.Data = dado.Date;
                    lanc.LoteLancamentoId = lote.Id;
                    lanc.ContaCorrenteId = contaCorrente.Id;
                    lanc.ContaCorrente = contaCorrente;
                    lanc.LancamentoPadraoId = _context.LancamentosPadroes.FirstOrDefault(l => l.Codigo == dado.LancamentoPadraoSelecionado).Id;
                }
                else
                {
                    _context.OfxLancamentos.Add(new OfxLancamento()
                    {
                        Inativar = dado.Inativar,
                        Documento = dado.Id,
                        TipoLancamento = dado.Type,
                        Descricao = dado.Description,
                        ValorOfx = dado.TransationValue,
                        Data = dado.Date,
                        LoteLancamentoId = lote.Id,
                        ContaCorrenteId = contaCorrente.Id,
                        ContaCorrente = contaCorrente,
                        LancamentoPadraoId = _context.LancamentosPadroes.FirstOrDefault(l => l.Codigo == dado.LancamentoPadraoSelecionado).Id
                    });
                }


                _context.SaveChanges();
                var descricao = _context.AutoDescricoes
                    .FirstOrDefault(c => c.Descricao == dado.Description);
                if (descricao == null)
                {
                    _context.AutoDescricoes.Add(new AutoDescricao()
                    {
                        Descricao = dado.Description,
                        LancamentoPadraoId = Convert.ToInt32(lancamentoPadrao.Id)
                    });
                    _context.SaveChanges();
                }
                else if (descricao.LancamentoPadraoId == lancamentoPadrao.Id
                    && descricao.Descricao != dado.Description)
                {
                    _context.AutoDescricoes.Add(new AutoDescricao()
                    {
                        Descricao = dado.Description,
                        LancamentoPadraoId = lancamentoPadrao.Id
                    });
                    _context.SaveChanges();
                }
                else
                {
                    continue;
                }
            }

            _context.SaveChanges();
            ViewBag.Importado = "Arquivo Importado!";
            IniT();
            return View("Index");

        }


        private static SelectList ConstruirLancamentosPadroesSelectList(IEnumerable<LancamentoPadrao> lancamentoPadroes)
            => new(lancamentoPadroes.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Descricao}" }), "Codigo", "Descricao");
        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo ", "Razao ");
    }
}
//else
//{
//    foreach (var dados in extratoView.ContasCorrentes.LancamentosOfxs)
//    {
//        var historico = historicos.FirstOrDefault(h => h.Descricao == extratoView.LancamentoManual.Descricao);

//        if (historico == null)
//        {
//            lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
//            {
//                Id = dados.Id,
//                TransationValue = dados.TransationValue,
//                Description = dados.Description,
//                Date = dados.Date,
//                CheckSum = dados.CheckSum,
//                Type = dados.Type,
//                ContasCredito = ConstruirContasContabeisSelectList(contasContabeis),
//                ContasDebito = ConstruirContasContabeisSelectList(contasContabeis),
//            });
//        }
//        else
//        {
//            lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
//            {
//                TransationValue = extratoView.LancamentoManual.Valor,
//                Description = extratoView.LancamentoManual.Descricao,
//                Date = extratoView.LancamentoManual.Data,
//                CheckSum = 1,
//                Type = extratoView.LancamentoManual.TipoSelecionado,
//                ContaCreditoSelecionada = historico.ContaCreditoId,
//                ContaDebitoSelecionada = historico.ContaDebitoId,
//                HistoricoId = historico.Id,
//                ContasCredito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaCreditoId)),
//                ContasDebito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaDebitoId)),
//            });
//        }

//        var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoView.Banco.Codigo);

//        var bancoViewModel = new OfxBancoViewModel()
//        {
//            Id = banco.Id,
//            Codigo = banco.Codigo,
//            Nome = banco.Nome
//        };

//        contaCorrenteViewModel = new OfxContaCorrenteViewModel()
//        {
//            LancamentosOfxs = lancamentoOfxViewModel,
//            NumeroConta = extratoView.ContasCorrentes.NumeroConta
//        };
//        extratoBancarioViewModel = new ExtratoBancarioViewModel()
//        {
//            Empresas = ConstruirEmpresas(empresas),
//            ContasCorrentes = contaCorrenteViewModel,
//            Banco = bancoViewModel
//        };
//    }

//    if (extratoView.LancamentoManual != null)
//    {
//        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
//        {
//            TransationValue = extratoView.LancamentoManual.Valor,
//            Description = extratoView.LancamentoManual.Descricao,
//            Date = extratoView.LancamentoManual.Data,
//            CheckSum = 1,
//            Type = extratoView.LancamentoManual.TipoSelecionado,
//            ContasCredito = ConstruirContasContabeisSelectList(contasContabeis),
//            ContasDebito = ConstruirContasContabeisSelectList(contasContabeis),
//        });
//    }
//}