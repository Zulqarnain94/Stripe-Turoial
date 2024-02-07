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



  public class PaymentController : ControllerBase
  {
    private readonly StripeSettings _stripeSettings;

    public PaymentController(IOptions<StripeSettings> stripeSettings)
    {
      _stripeSettings = stripeSettings.Value;
      StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    [HttpPost("create-checkout-session")]
    public ActionResult CreateCheckoutSession([FromBody] PaymentRequest request)
    {
      var options = new SessionCreateOptions
      {
        PaymentMethodTypes = new List<string> { "card" },
        LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = request.Amount*100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = request.ProductName, // Use the provided product name
                    },
                },
                Quantity = 1,
            },
        },
        Mode = "payment",
        SuccessUrl = "http://localhost:3000/success",
        CancelUrl = "http://localhost:3000/cancel",
        PaymentIntentData = new SessionPaymentIntentDataOptions
        {
          // for the dynamic email adress
         // ReceiptEmail = request.CustomerEmail,
          ReceiptEmail = "sulqi1994@gmail.com",

        },
      };

      var service = new SessionService();
      Session session = service.Create(options);
      return Ok(new { sessionId = session.Id });
    }


  }
}



// SuccessUrl = "http://localhost:3000/success",
// CancelUrl = "http://localhost:3000/cancel",