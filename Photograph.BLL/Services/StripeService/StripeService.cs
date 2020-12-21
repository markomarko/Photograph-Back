using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Dtos;
using Stripe;

namespace Photograph.BLL.Services.StripeService
{
	public class StripeService
	{
		public static async Task<string> GetTokenId()
		{
			var myToken = new StripeTokenCreateOptions
			{
				Card = new StripeCreditCardOptions
				{
					Number = "4242424242424242"
				}
			};

			var tokenService = new StripeTokenService()
			{
				ApiKey = GetPublicApiKey()
			};

			var stripeToken = await tokenService.CreateAsync(myToken);

			return stripeToken.Id;
		}

		public static string CreateCustomer(string tokenId, string email)
		{
			var customerOptions = new StripeCustomerCreateOptions
			{
				SourceToken = tokenId,
				Email = email
			};

			var customerService = new StripeCustomerService {ApiKey = GetPrivateApiKey()};
			var customer = customerService.Create(customerOptions);

			return customer.Id;
		}

		public static string SubscribeCustomer(string stripeCustomerId, SubscriptionPlan subscriptionPlan)
		{
			var subscriptionService = new StripeSubscriptionService() {ApiKey = GetPrivateApiKey()};

			var planId = GetPlanId(subscriptionPlan);

			var subscription = subscriptionService.Create(stripeCustomerId, planId);

			return subscription.Id;
		}

		private static string GetPublicApiKey()
		{
			return ConfigurationManager.AppSettings["StripeApiKey"];
		}

		private static string GetPrivateApiKey()
		{
			return ConfigurationManager.AppSettings["StripeApiSecretKey"];
		}

		private static string GetPlanId(SubscriptionPlan subscriptionPlan)
		{
			return ConfigurationManager.AppSettings[subscriptionPlan.ToString()];
		}
	}
}