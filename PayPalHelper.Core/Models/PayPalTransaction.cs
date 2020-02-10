using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace PayPalHelper.Core.Models
{
    /// <summary>
    /// based on info here: https://developer.paypal.com/docs/ipn/integration-guide/IPNandPDTVariables/
    /// </summary>
    public class PayPalTransaction
    {
        public static PayPalTransaction FromForm(IFormCollection form)
        {
            var result = new PayPalTransaction();

            var fieldMap = new Dictionary<string, string>()
            {
                { "business", nameof(Business)  },
                { "txn_type", nameof(TransactionType) },
                { "txn_id", nameof(TransactionId) },

            };

            return result;
        }

        public string TransactionType { get; set; }
        public string TransactionId { get; set; }
        public string Business { get; set; }
        public string BuyerEmail { get; set; }
        public string AddressCountry { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountryCode { get; set; }
        public string AddressName { get; set; }
        public string AddressState { get; set; }
        public string AddressStreet { get; set; }
        public string AddressZip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayerBusinessName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerId { get; set; }
        public string ItemNumber { get; set; }
        public decimal AuthAmount { get; set; }
        public DateTime AuthExpiration { get; set; }
        public string AuthId { get; set; }
        public string AuthStatus { get; set; }
        public decimal Discount { get; set; }
        public DateTime ECheckTimeProcessed { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public decimal NetPayment { get; set; }
    }
}
