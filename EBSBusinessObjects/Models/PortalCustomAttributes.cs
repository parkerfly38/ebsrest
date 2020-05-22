using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class PortalCustomAttributes
    {
        public int Id { get; set; }

        public string CompId { get; set; }

        public string AttribName { get; set; }

        public string AttribValue { get; set; }

        public string AttribType { get; set; }

        public string Module { get; set; }

        public string Desc { get; set; }

        public string AppName { get; set; }

        public string Version { get; set; }

        public string MaintType { get; set; }

        public int ReadOnly { get; set; }

        public int MayOverride { get; set; }

        public string EventOrAttrib { get; set; }

        public string HelpText { get; set; }

        //we've disabled licensing for now
        //public string En { get; set; }
    }
}