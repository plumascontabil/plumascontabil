using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class LogEvento
    {
        public int Id { get; set; }
        public int? EventId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LogLevel { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
