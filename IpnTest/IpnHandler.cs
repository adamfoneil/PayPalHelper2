using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PayPalHelper.Core.Extensions;
using System.Threading.Tasks;

namespace IpnTest
{
    public static class IpnHandler
    {
        [FunctionName("IpnHandler")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ILogger log)
        {
            var result = await req.VerifyPayPalTransactionAsync(PayPalEnvironment.Sandbox, log);
            if (result.IsVerified)
            {
                log.LogInformation("yes it's verified!");
                log.LogInformation($"the buyer is {result.Transaction.PayerEmail}, and they paid {result.Transaction.Gross} for item {result.Transaction.ItemNumber}");
            }
            else
            {
                log.LogInformation("no, not verified");
            }

            return new OkResult();
        }
    }
}
