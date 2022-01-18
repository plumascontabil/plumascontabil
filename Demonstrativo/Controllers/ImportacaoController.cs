using CsvHelper;
using Demonstrativo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFXParser.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class ImportacaoController : Controller
    {
        Context _context;
        IHostingEnvironment _appEnvironment;

        public ImportacaoController(Context context, IHostingEnvironment env)
        {
            _context = context;
            _appEnvironment = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Importar(IFormFile file)
        {
            var stream = file.OpenReadStream();

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CultureInfo("pt-BR")))
            {
                var records = csv.GetRecords<ImportacaoViewModel>().ToList();
                
                foreach (var record in records)
                {
                    var insertEmpresa = new Empresa();

                    insertEmpresa.Codigo = record.Codigo;
                    insertEmpresa.RazaoSocial = record.Razao;
                    insertEmpresa.Apelido = record.Apelido;
                    insertEmpresa.Cnpj = record.Cnpj ?? "00.000.000/0001-00";
                    insertEmpresa.Situacao = record.Situacao;

                    _context.Empresas.Add(insertEmpresa);
                    _context.SaveChanges();
                }
            }
            return View("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> ImportarOfx(IFormFile fileOfx)
        {
            var caminhoArquivo = Path.GetTempFileName();

            string pasta = "Temp";
            string nomeArquivo = fileOfx.FileName;
            string caminho_WebRoot = _appEnvironment.WebRootPath;
            string caminhoDestinoArquivo = caminho_WebRoot + "\\" + pasta + "\\" + nomeArquivo;

            using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
            {
                await fileOfx.CopyToAsync(stream);
            }

            Extract extratoBancario = OFXParser.Parser.GenerateExtract(caminhoDestinoArquivo);
            var kgomes = new List<ImportacaoOfxViewModel>();

            foreach (var record in extratoBancario.Transactions)
            {
                kgomes.Add(new ImportacaoOfxViewModel()
                {
                    Id = record.Id,
                    TransationValue = record.TransactionValue,
                    Description = record.Description,
                    Date = record.Date,
                    CheckSum = record.Checksum,
                    Type = record.Type,
                });
            }

            System.IO.File.Delete(caminhoDestinoArquivo);
            System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");

            return View("Ofx", kgomes);
        }
    }
}
