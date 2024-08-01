using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Protyo.Utilities.Models.stripe;
using Protyo.WebService.Jobs.Contracts;
using Stripe.Checkout;

using System.Threading.Tasks;

namespace Protyo.WebService.Controllers
{
    [EnableCors("AllowPublicClient")]
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {

        private ISubscriptionJob subscriptionJob;
        public PaymentsController(ISubscriptionJob subscriptionJob)
        {

            this.subscriptionJob = subscriptionJob;
        }

        [HttpGet("config")]
        public ConfigResponse Setup() => this.subscriptionJob.ReturnConfigResponse();


        [HttpPost("create-subscription-session")]
        public async Task<Session> CreateSubscriptionSession([FromBody] SubscriptionPost request)
            => this.subscriptionJob.CreateSubscriptionSession(request);


        [HttpGet("checkout-session")]
        public async Task<IActionResult> CheckoutSession(string sessionId)
            => Ok(await this.subscriptionJob.CreateSessionService(sessionId).GetAsync(sessionId));


        [HttpPost("customer-portal")]
        public async Task<IActionResult> CustomerPortal(string sessionId)
        {

            var checkoutSession = await this.subscriptionJob
                                                .CreateSessionService(sessionId).GetAsync(sessionId);

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = checkoutSession.CustomerId,
                ReturnUrl = this.subscriptionJob.Options.Value.Domain,
            };

            var session = await this.subscriptionJob
                                        .CreateBillingPortalSessionService().CreateAsync(options);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

    }
}
