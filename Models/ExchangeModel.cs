using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsERT_Demo_HS.Models
{
    public class ExchangeModel
    {
        public string table { get; set; }
        public string no { get; set; }
        public DateTime effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
    }

    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal mid { get; set; }
    }
}
