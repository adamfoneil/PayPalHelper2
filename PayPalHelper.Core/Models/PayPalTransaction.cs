using Newtonsoft.Json;

namespace PayPalHelper.Core.Models
{
    /// <summary>
    /// based on info here: https://developer.paypal.com/docs/ipn/integration-guide/IPNandPDTVariables/
    /// </summary>
    public class PayPalTransaction
    {
        [JsonProperty("txn_type")]
        public string TransactionType { get; set; }

        [JsonProperty("txn_id")]
        public string TransactionId { get; set; }

        [JsonProperty("parent_txn_id")]
        public string ParentTransactionId { get; set; }

        [JsonProperty("business")]
        public string Business { get; set; }

        [JsonProperty("address_country")]
        public string AddressCountry { get; set; }

        [JsonProperty("address_city")]
        public string AddressCity { get; set; }

        [JsonProperty("address_country_code")]
        public string AddressCountryCode { get; set; }

        [JsonProperty("address_name")]
        public string AddressName { get; set; }

        [JsonProperty("address_state")]
        public string AddressState { get; set; }

        [JsonProperty("address_street")]
        public string AddressStreet { get; set; }

        [JsonProperty("address_zip")]
        public string AddressZip { get; set; }

        [JsonProperty("contact_phone")]
        public string ContactPhone { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("payer_business_name")]
        public string PayerBusinessName { get; set; }

        [JsonProperty("payer_email")]
        public string PayerEmail { get; set; }

        [JsonProperty("payer_id")]
        public string PayerId { get; set; }

        [JsonProperty("item_number")]
        public string ItemNumber { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("mc_fee")]
        public decimal Fee { get; set; }

        [JsonProperty("mc_gross")]
        public decimal Gross { get; set; }

        public decimal Net { get { return Gross - Fee; } }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }
    }
}
