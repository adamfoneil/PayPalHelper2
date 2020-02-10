using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PayPalHelper.Core
{
    public enum PayPalEnvironment
    {
        Sandbox,
        Live
    }

    public static class RequestExtensions
    {
        private static HttpClient _client = new HttpClient();

        public static async Task<bool> IsVerifiedAsync(this HttpRequest request, PayPalEnvironment environment, ILogger logger = null)
        {
            try
            {
                var urls = new Dictionary<PayPalEnvironment, Uri>
                {
                    { PayPalEnvironment.Live, new Uri("https://ipnpb.paypal.com/") },
                    { PayPalEnvironment.Sandbox, new Uri("https://ipnpb.sandbox.paypal.com/") }
                };

                _client.BaseAddress = urls[environment];
                _client.DefaultRequestHeaders.Accept.Clear();

                var formValues = new Dictionary<string, string>();
                formValues.Add("cmd", "_notify-validate");
                foreach (var field in request.Form) formValues.Add(field.Key, field.Value);
                var content = new FormUrlEncodedContent(formValues);

                var response = await _client.PostAsync("cgi-bin/webscr", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return (result.Equals("VERIFIED"));
                }
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Error in IsVerifiedAsync");
            }

            return false;
        }
    }
}
