using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class ContaCorrente
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string NumeroConta { get; set; }

        [Required]
        [ForeignKey("EmpresaId")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [Required]
        [ForeignKey("BancoOfxId ")]
        public int BancoOfxId { get; set; }
        public BancoOfx BancoOfx { get; set; }

    }
}
