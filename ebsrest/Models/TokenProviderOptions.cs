using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/auth";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(180);
        public SigningCredentials SigningCredentials { get; set; }
    }
}