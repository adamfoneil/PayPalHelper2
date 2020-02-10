using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PayPalHelper.Core;
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
            var result = await req.IsVerifiedAsync(PayPalEnvironment.Sandbox, log);
            if (result.IsVerified)
            {
                log.LogInformation("yes it's verified!");
            }
            else
            {
                log.LogInformation("no, not verified");
            }

            return new OkResult();
        }
    }
}
