using Microsoft.Extensions.Configuration;
using Services.Daemon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.DebugUtility
{
    public class MainDebugWindowViewModel : ViewModelBase
    {
        private WebClient _client = new WebClient();
        private readonly IConfiguration _configuration;

        public MainDebugWindowViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string FaucetTargetAddress { get; set; }
        public decimal FaucetAmount { get; set; }
        private string _faucetStatus = string.Empty;

        public string FaucetStatus { get => _faucetStatus; set { _faucetStatus = value; OnPropertyChanged(nameof(FaucetStatus)); } }
        public void GetCoinsFromFaucet()
        {
            _client = new WebClient();

            var endpoint = _configuration.GetValue<string>("FaucetRemoteNode");

            var req = new DaemonRequest("dbg_faucet_stealth", FaucetTargetAddress, FaucetAmount);

            try
            {
                var resp = _client.UploadString(new Uri(endpoint), req.Serialize());

                if (resp.Contains("Could not fulfill faucet request")) FaucetStatus = "Request failed";
                else FaucetStatus = string.Empty;
            }
            catch (Exception e)
            {
                FaucetStatus = e.Message;
            }
        }
    }
}
