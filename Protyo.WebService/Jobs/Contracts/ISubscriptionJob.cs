using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protyo.Utilities.Models.configuration;
using Protyo.Utilities.Models.stripe;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protyo.WebService.Jobs.Contracts
{
    public interface ISubscriptionJob
    {
        public IOptions<StripeOptions> Options { get; set; }

        public ConfigResponse ReturnConfigResponse();

        public Session CreateSubscriptionSession(Utilities.Models.stripe.Subscription request);

        public SessionService CreateSessionService(string sessionId);

        public Stripe.BillingPortal.SessionService CreateBillingPortalSessionService();

        public void UpdateSubscription(Invoice intent);
    }
}
