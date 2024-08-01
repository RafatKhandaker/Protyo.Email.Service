using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Protyo.Utilities.Models.configuration;
using Protyo.Utilities.Services;
using Protyo.WebService.Jobs;
using Protyo.WebService.Jobs.Contracts;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protyo.WebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            StripeConfiguration.AppInfo = new AppInfo
            {
                Name = "stripe-samples/checkout-single-subscription",
                Url = "https://github.com/stripe-samples/checkout-single-subscription",
                Version = "0.0.1",
            };

            services.Configure<StripeOptions>(options =>
            {
                options.PublishableKey = StripeOptionConfiguration.GetInstance().StripeOptions.PublishableKey;
                options.SecretKey = StripeOptionConfiguration.GetInstance().StripeOptions.SecretKey;
                options.WebhookSecret = StripeOptionConfiguration.GetInstance().StripeOptions.WebhookSecret;
                options.Domain = StripeOptionConfiguration.GetInstance().StripeOptions.Domain;
            });

            services.AddScoped<ISubscriptionJob, SubscriptionJob>();

            services.AddCors(options => {
                options.AddPolicy("AllowPublicClient", policy => { policy.AllowAnyOrigin(); policy.AllowAnyHeader(); policy.AllowAnyMethod(); });
                options.AddPolicy("AllowServer2Server", policy => { policy.WithOrigins(CorsConfiguration.GetInstance().CorsConfigurationSettings.Server2Server); policy.AllowAnyHeader(); policy.AllowAnyMethod(); });

            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Protyo.WebService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Protyo.WebService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
