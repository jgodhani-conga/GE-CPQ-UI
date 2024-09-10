using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpq_ui_master.Main
{
    public class AppConfig
    {
        public string baseUrl { get; set; }git in
        public string userName { get; set; }
        public string Password { get; set; }
        public string idpName { get; set; }
        public string tokenURL { get; set; }
        public string userId { get; set; }
        public string tenantURL { get; set; }
        public string tenantId { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string grantType { get; set; }
        public string apiuserId { get; set; }
        public string organizationFriendlyId { get; set; }
        public string organizationId { get; set; }
    }
}