using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_signature_timer.Models
{
    public class ArticleRequest
    {
        public string Account { get; set; }

        public string ArticleUrl { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string TransactionId { get; set; }

    }
}
