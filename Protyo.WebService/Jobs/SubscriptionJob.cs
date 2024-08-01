
using Microsoft.Extensions.Options;
using Stripe;
using System.Collections.Generic;
using Stripe.Checkout;

using System;
using Newtonsoft.Json;
using Protyo.WebService.Jobs.Contracts;
using Protyo.Utilities.Models.configuration;
using Protyo.Utilities.Models.stripe;
using Protyo.WebService.Configuration;
using Microsoft.Extensions.Logging;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services.Contracts;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Protyo.WebService.Jobs
{
    public class SubscriptionJob : ISubscriptionJob
    {
        public IOptions<StripeOptions> Options { get; set; }
        private readonly ILogger<SubscriptionJob> _logger;
        private IMongoService<UserDataObject> MongoService;

        private readonly IStripeClient Client;
 
        public SubscriptionJob(IOptions<StripeOptions> options, ILogger<SubscriptionJob> logger, IMongoService<UserDataObject> mongoService )
        {
            this.Options = options;
            this.Client = new StripeClient(this.Options.Value.SecretKey);
            MongoService = mongoService;
            _logger = logger;
        }

        public ConfigResponse ReturnConfigResponse() {
            return new ConfigResponse
            {
                PublishableKey = this.Options.Value.PublishableKey,
            };
        }

        public Session CreateSubscriptionSession(Utilities.Models.stripe.SubscriptionPost request) {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price =  "price_1NnBAPGt12k2uLzlpn5urfrg",
                    Quantity = 1,
                  },
                },

                Mode = "subscription",

                SuccessUrl = AppSettings.ApiSuccessRedirectUri,
                CancelUrl = AppSettings.ApiCanceledRedirectUri,
            };

            options.SubscriptionData = new SessionSubscriptionDataOptions();
            options.SubscriptionData.Metadata = new Dictionary<string, string>();

            options.SubscriptionData.Metadata.Add("clientId", request.clientId);
            options.SubscriptionData.Metadata.Add("value", request.value.ToString());
            options.SubscriptionData.Metadata.Add("duration", "monthly");
            options.SubscriptionData.Metadata.Add("type", "subscription");

            var session = new SessionService(this.Client).Create(options);

            return session;
        }

        public SessionService CreateSessionService(string sessionId) => new SessionService(this.Client);

        public Stripe.BillingPortal.SessionService CreateBillingPortalSessionService() => new Stripe.BillingPortal.SessionService(this.Client);

        public void UpdateSubscription(Invoice intent) {

            try {
                var subscriptionService = new SubscriptionService(this.Client);
                var session = subscriptionService.Get(intent.SubscriptionId);

                if (session.Status.Equals("active"))
                    MongoService.Update(Builders<UserDataObject>.Filter.Eq(p => p._Id, new ObjectId(intent.SubscriptionDetails.Metadata.GetValueOrDefault("clientId"))), 
                            Builders<UserDataObject>.Update.Set(p => p.subscription, intent.SubscriptionDetails.Metadata.GetValueOrDefault("subscriptionType"))
                        );
            }
            catch (Exception e) {
                _logger.LogError("{ `Task`: `Protyo.WebService.Jobs.SubscriptionJob`, `Method` : `UpdateSubscription`, `Message`: " +e.Message +" }");
            }
            
        }
        
    }
}
