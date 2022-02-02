using System;
using System.Collections.Generic;
using System.Linq;
using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demonstrativo.Models
{
    public class ImportacaoOfxViewModel
    {
        public List<DadosViewModel> Dados { get; set; }
        public SelectList Empresas { get; set; }
        public int EmpresaSelecionada { get; set; }

    }
}

