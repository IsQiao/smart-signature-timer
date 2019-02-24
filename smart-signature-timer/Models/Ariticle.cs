using System;
using System.Collections.Generic;

namespace smart_signature_timer.Models
{
    public partial class Ariticle
    {
        public int Id { get; set; }
        public string EosAccount { get; set; }
        public string ArticleUrl { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? Time { get; set; }
        public string SignId { get; set; }
        public string FissionFactor { get; set; }
        public int? State { get; set; }
        public string TransactionId { get; set; }
    }
}
