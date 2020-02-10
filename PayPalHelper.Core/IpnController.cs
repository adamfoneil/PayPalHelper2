using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayPalHelper.Core.Extensions;
using PayPalHelper.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayPalHelper.Core
{
    public abstract class IpnController : Controller
    {
        private readonly ILogger _logger;

        protected abstract PayPalEnvironment Environment { get; }
        protected abstract bool LogUnverifiedTransactions { get; }

        protected abstract Task OnVerifiedAsync(PayPalTransaction transaction);

        protected async virtual Task OnVerifiedExceptionAsync(Exception exception) { await Task.CompletedTask; }

        public IpnController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Listener()
        {
            try
            {
                var result = await Request.VerifyPayPalTransactionAsync(Environment);
                if (result.IsVerified)
                {
                    _logger.LogInformation($"Verified transaction at {DateTime.UtcNow}:\r\n{GetFormText(Request.Form)}");

                    try
                    {
                        await OnVerifiedAsync(result.Transaction);
                    }
                    catch (Exception exc)
                    {
                        _logger.LogError(exc, $"Verified transaction handler failed at {DateTime.UtcNow} because {exc.Message}, form data:\r\n{GetFormText(Request.Form)}");
                        await OnVerifiedExceptionAsync(exc);
                    }
                }
                else
                {
                    if (LogUnverifiedTransactions)
                    {
                        _logger.LogInformation($"Unverified transaction at {DateTime.UtcNow}:\r\n{GetFormText(Request.Form)}");
                    }
                }
            }
            catch (Exception exc)
            {
                _logger.LogError($"Verification check failed at {DateTime.Now}: {exc.Message}");
            }

            // we never want exceptions getting back to PayPal.
            // if there's a problem, we must handle internally.
            return Ok();
        }

        private string GetFormText(IFormCollection form)
        {
            return string.Join("\r\n", form.Select(field => $"{field.Key} = {field.Value}"));
        }
    }
}
