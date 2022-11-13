using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discreet_GUI.Caches
{
    public class SubmitIssueCache
    {
        public string Summary { get; set; }
        public int Severity { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
    }
}
