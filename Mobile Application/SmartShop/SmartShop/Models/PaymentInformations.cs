using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class PaymentInformations
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV2 { get; set; }
        public string CVC2 { get; set; }
    }
}
