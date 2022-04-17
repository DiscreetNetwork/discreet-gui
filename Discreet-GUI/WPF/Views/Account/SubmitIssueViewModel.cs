using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Common;

namespace WPF.Views.Account
{
    public class SubmitIssueViewModel : ViewModelBase
    {
        public List<string> SeverityOptions { get; set; } = new List<string>
        {
            "Low", "Medium", "High"
        };

        public int SelectedSeverityIndex { get; set; }
    }
}
