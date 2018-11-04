using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaypalMVC.Models
{
    public class PaypalConfiguration
    {
        public static string ClientId { get; }
        public static string ClientSecret { get; }

        static PaypalConfiguration ()
        {
            var config = GetConfig();
            ClientId = config["ClientId"];
            ClientSecret = config[ClientSecret];
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            var apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }

    }
}