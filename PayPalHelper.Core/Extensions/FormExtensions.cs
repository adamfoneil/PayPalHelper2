using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PayPalHelper.Core.Extensions
{
    public static class FormExtensions
    {
        /// <summary>
        /// returns strongly-typed T from a form collection
        /// </summary>
        public static T ParseForm<T>(this IFormCollection form) where T : new()
        {
            var fields = form
                .Select(keyPair => new KeyValuePair<string, string>(keyPair.Key, keyPair.Value.First()))
                .ToDictionary(keyPair => keyPair.Key, keyPair => keyPair.Value);

            string json = JsonConvert.SerializeObject(fields);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
