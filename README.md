[![Nuget](https://img.shields.io/nuget/v/AO.PayPalHelper)](https://www.nuget.org/packages/AO.PayPalHelper/)

This is a .NET Core overhaul of my old [PayPalHelper](https://github.com/adamosoftware/PayPalHelper) project. The Nuget package is **AO.PayPalHelper**.

The thing to use here is an `HttpRequest` extension method called [VerifyPayPalTransactionAsync](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/Extensions/RequestExtensions.cs#L23). This returns a [VerificationResult](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/Models/VerificationResult.cs).

```csharp
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
```

You can see an example in my little Azure Function [test project](https://github.com/adamosoftware/PayPalHelper2/blob/master/IpnTest/IpnHandler.cs).

There is also an [IpnController](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/IpnController.cs) abstract class you can implement. This is sort of compatible with the approach in my old project, but nowadays I lean toward Azure Functions, and I like how lightweight a single extension method is.

I don't have a unit test project, and I haven't tried this with real PayPal transactions yet. But I have tested with [Ngrok](https://ngrok.com/) and PayPal's own [IPN simulator](https://developer.paypal.com/developer/ipnSimulator/). Microsoft has a great [walkthrough](https://docs.microsoft.com/en-us/azure/azure-functions/functions-debug-event-grid-trigger-local) on using Ngrok to test Azure Functions locally, and that's what I worked from.

Note that I didn't try to implement every single IPN variable on my [PayPalTransaction](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/Models/PayPalTransaction.cs) model class. I just addded the important ones I'm familiar with that jumped out at me from PayPal's [reference](https://developer.paypal.com/docs/ipn/integration-guide/IPNandPDTVariables/).

Here's a video walkthrough that doubles as a crash course into to Azure Functions: [Vimeo](https://player.vimeo.com/video/391312203).

As of 3/12/20, I have a realistic use case in my [CloudLicensing project](https://github.com/adamosoftware/CloudLicensing/blob/master/CloudLicensing.Server/OnPurchased.cs).
