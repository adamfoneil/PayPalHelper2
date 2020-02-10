# PayPalHelper2

So far, I'm just sketching this out.... have not run it yet nor any tests.

- [RequestExtensions](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/RequestExtensions.cs) will be the basis of this going forward. An `HttpRequest` extension method [IsVerifiedAsync](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/RequestExtensions.cs#L19) is intended to work on its own or part of a controller. This would be what you'd use in an Azure Function, for example.

- [IpnController](https://github.com/adamosoftware/PayPalHelper2/blob/master/PayPalHelper.Core/IpnController.cs) abstract class will be the controller implementation, using the extension method above. This would be used in a .NET Core project.
