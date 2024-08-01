using Microsoft.AspNetCore.Mvc;
using Protyo.WebService.Jobs.Contracts;
using Stripe;
using System.IO;

namespace Protyo.WebService.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private ISubscriptionJob SubscriptionJob;
        public WebhookController(ISubscriptionJob subscriptionJob)
        {
            this.SubscriptionJob = subscriptionJob;
        }

        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                var json = new StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], SubscriptionJob.Options.Value.WebhookSecret, throwOnApiVersionMismatch: false);

                dynamic intent = null;

                switch (stripeEvent.Type)
                {
                    case "customer.subscription.created":

                        break;
                    case "invoice.paid":
                        intent = (Invoice)stripeEvent.Data.Object;
                        this.SubscriptionJob.
                                UpdateSubscription(intent);
                        break;
                    case "customer.subscription.deleted":
                        break;

                    case "customer.subscription.paused":
                        break;

                    case "customer.subscription.resumed":
                        break;

                    case "customer.subscription.trial_will_end":
                        break;
                    case "customer.subscription.updated":
                        break;
                    case "payment_intent.succeeded":
                        intent = (PaymentIntent)stripeEvent.Data.Object;

                        // Fulfil the customer's purchase

                        break;
                    case "payment_intent.payment_failed":
                        intent = (PaymentIntent)stripeEvent.Data.Object;

                        // Notify the customer that payment failed

                        break;
                    default:
                        // Handle other event types

                        break;
                }
                return new EmptyResult();

            }
            catch (StripeException e)
            {
                // Invalid Signature
                return BadRequest();
            }
        }
    }
}
