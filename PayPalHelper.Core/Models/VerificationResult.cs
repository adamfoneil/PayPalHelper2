using System;

namespace PayPalHelper.Core.Models
{
    public class VerificationResult
    {
        public bool IsVerified { get; set; }
        public PayPalTransaction Transaction { get; set; }
        public Exception Exception { get; set; }
    }
}
