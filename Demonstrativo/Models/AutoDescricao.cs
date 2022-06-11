using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class LogEvent
    {
        [Key]
        public int Id { get; set; }
        public string LogLevel { get; set; }

        public int EventId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public void Qualquer() { }
    }
}
