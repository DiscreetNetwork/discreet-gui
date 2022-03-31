using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPF.Hosted
{
    public class DaemonActivatorService : BackgroundService
    {
        private const string _daemonName = "cmd";
        private const string _daemonExecutablePath = "cmd.exe";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var processToStart = Process.GetProcessesByName(_daemonName).FirstOrDefault();

                if(processToStart is null)
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = _daemonExecutablePath,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                    };

                    Process.Start(psi);
                }

                await Task.Delay(1000);
            }
        }
    }
}
