using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout; // Add this for Stripe Checkout related classes
using Models; // Make sure this points to the correct namespace for your models
using System;
using System.Collections.Generic;


namespace Controllers
{
  [ApiController]
  [Route("[controller]")]

 
  
  public class PaymentController : ControllerBase {
    private readonly StripeSettings _stripeSettings;

    public PaymentController(IOptions<StripeSettings> stripeSettings)
    {
      _stripeSettings = stripeSettings.Value;
      StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    [HttpPost("create-checkout-session")]
    public IActionResult CreateCheckoutSession([FromBody] PaymentRequest paymentRequest)
    {
      try
      {
        var currency = "usd";
        var successUrl = "http://localhost:3000/success";
        var cancelUrl = "http://localhost:3000/cancel";

        var options = new SessionCreateOptions
        {
          PaymentMethodTypes = new List<string> { "card" },
          LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = currency,
                                UnitAmount = paymentRequest.Amount * 100,  // Amount in cents
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Product Name",
                                    Description = "Product Description"
                                }
                            },
                            Quantity = 1
                        }
                    },
          Mode = "payment",
          SuccessUrl = successUrl,
          CancelUrl = cancelUrl
        };

        var service = new SessionService();
        var session = service.Create(options);

        return Ok(new { sessionId = session.Id });
      }
      catch (Exception ex)
      {
        // Log the exception for debugging purposes
        Console.WriteLine($"Exception: {ex.Message}");
        return StatusCode(500, "Internal Server Error");
      }
    }
  }
}
