using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientCalculatorService
{
    public class AuthConfig
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string ResourceId { get; set; }
        public string Authority => $"{Instance}{TenantId}";
    }
}
