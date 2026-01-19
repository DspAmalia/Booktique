using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

[Route("create-checkout-session")]
[ApiController]
public class PaymentsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCheckoutSession([FromForm] string priceId)
    {
        var options = new SessionCreateOptions
        {
            SuccessUrl = "https://localhost:7248/success?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = "https://localhost:7248/cancel",
            Mode = "subscription", // sau "payment" pentru one-time
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1
                }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }
}
