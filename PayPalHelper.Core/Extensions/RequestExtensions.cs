using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PayPalHelper.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PayPalHelper.Core.Extensions
{
    public enum PayPalEnvironment
    {
        Sandbox,
        Live
    }

    public static class RequestExtensions
    {
        private static HttpClient _client = new HttpClient();

        private static PayPalEnvironment _env;

        public static async Task<VerificationResult> VerifyPayPalTransactionAsync(this HttpRequest request, PayPalEnvironment environment, ILogger logger = null)
        {
            bool isVerified = false;
            PayPalTransaction transaction = null;

            try
            {
                var urls = new Dictionary<PayPalEnvironment, Uri>
                {
                    { PayPalEnvironment.Live, new Uri("https://ipnpb.paypal.com/") },
                    { PayPalEnvironment.Sandbox, new Uri("https://ipnpb.sandbox.paypal.com/") }
                };
                
                if (_client.BaseAddress == null)
                {
                    _client.BaseAddress = urls[environment];
                    _env = environment;
                }
                else
                {
                    if (environment != _env) throw new Exception("Can't switch environments without restarting your app.");
                }
                
                _client.DefaultRequestHeaders.Accept.Clear();

                var formValues = new Dictionary<string, string>();
                formValues.Add("cmd", "_notify-validate");
                foreach (var field in request.Form) formValues.Add(field.Key, field.Value);
                var content = new FormUrlEncodedContent(formValues);

                var response = await _client.PostAsync("cgi-bin/webscr", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    isVerified = responseText.Equals("VERIFIED");

                    if (isVerified)
                    {
                        transaction = request.Form.ParseForm<PayPalTransaction>();
                    }
                }
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Error in IsVerifiedAsync");
            }

            return new VerificationResult()
            {
                IsVerified = isVerified,
                Transaction = transaction
            };
        }
    }
}
