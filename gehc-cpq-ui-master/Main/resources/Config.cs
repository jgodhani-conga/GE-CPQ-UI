using cpq_ui_master.Main;

using Newtonsoft.Json;

using System;

using System.IO;

namespace AutomationProject1.Main.resources

{
    public class Config

    {
        private static readonly AppConfig appConfig;
        static Config()
        {
            string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "Main", "AppConfig/properties.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                appConfig = JsonConvert.DeserializeObject<AppConfig>(json);
            }
            else
            {
                throw new FileNotFoundException("Configuration file not found.");
            }
        }
        public static string baseURL => appConfig.baseUrl;
        public static string loginUser => appConfig.userName;
        public static string loginPassword => appConfig.Password;
        public static string idpName => appConfig.idpName;
        public static string tokenURL => appConfig.tokenURL;
        public static string userId => appConfig.userId;
        public static string tenantURL => appConfig.tenantURL;
        public static string tenantId => appConfig.tenantId;
        public static string clientId => appConfig.clientId;
        public static string clientSecret => appConfig.clientSecret;
        public static string grantType => appConfig.grantType;
        public static string apiuserId => appConfig.apiuserId;
        public static string organizationFriendlyId => appConfig.organizationFriendlyId;
        public static string organizationId => appConfig.organizationId;

    }

}


