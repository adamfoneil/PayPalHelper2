using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PayPalHelper.Core
{
    public enum Environment
    {
        Sandbox,
        Live
    }

    public static class RequestExtensions
    {
        private static HttpClient _client = new HttpClient();

        public static async Task<bool> IsVerifiedTransaction(this HttpRequest request, Environment environment)
        {
            var urls = new Dictionary<Environment, Uri>
            {
                { Environment.Live, new Uri("https://ipnpb.paypal.com/") },
                { Environment.Sandbox, new Uri("https://ipnpb.sandbox.paypal.com/") }
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

            return false;

        }
    }
}
