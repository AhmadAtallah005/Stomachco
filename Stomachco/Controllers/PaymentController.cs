using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace Stomachco.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class PaymentController : ControllerBase
        {
            private readonly StripeSettings _stripe;

            public PaymentController(IOptions<StripeSettings> stripeSettings)
            {
                _stripe = stripeSettings.Value;
                StripeConfiguration.ApiKey = _stripe.SecretKey; // إعداد المفتاح السري
            }

            [HttpPost("create-payment-intent")]
            public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request)
            {
                try
                {
                    var options = new PaymentIntentCreateOptions
                    {
                        Amount = request.Amount, // بالمليم (مثلاً 1000 = 10.00 JOD)
                        Currency = request.Currency ?? "jod",
                        PaymentMethodTypes = new List<string> { "card" }
                    };

                    var service = new PaymentIntentService();
                    var paymentIntent = await service.CreateAsync(options);

                    return Ok(new { clientSecret = paymentIntent.ClientSecret });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }
            [HttpPost("webhook")]
            public async Task<IActionResult> StripeWebhook()
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        Request.Headers["Stripe-Signature"],
                        "whsec_your_webhook_secret"
                    );



                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }

        }

        public class PaymentRequest
        {
            public long Amount { get; set; } // 1000 = 10.00
            public string Currency { get; set; }
        }
    }

