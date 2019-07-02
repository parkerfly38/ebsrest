using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ebsrest.Startup))]

namespace ebsrest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConnectionFactory.ConnectionStrings.Add("DefaultConnection", ConfigurationManager.ConnectionStrings[1].ConnectionString);
        }
    }
}
