using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Services.Caches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF.Services;

namespace WPF.Hosted
{
    public class DaemonActivatorService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly NotificationService _notificationService;
        private readonly WalletCache _walletCache;
        private readonly DaemonLogCache _daemonLogCache;

        public DaemonActivatorService(IConfiguration configuration, NotificationService notificationService, WalletCache walletCache, DaemonLogCache daemonLogCache)
        {
            _configuration = configuration;
            _notificationService = notificationService;
            _walletCache = walletCache;
            _daemonLogCache = daemonLogCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            bool useDaemonActivator = _configuration.GetValue<bool>("DaemonSettings:UseActivator");
            if (!useDaemonActivator) return;

            
            if(Process.GetProcessesByName(_configuration.GetValue<string>("DaemonSettings:ExecutableName")).FirstOrDefault() != null)
            {
                _walletCache.VisorStartupComplete = true;
                return;
            }

            var executablePath = _configuration.GetValue<string>("DaemonSettings:ExecutablePath");
            while(!stoppingToken.IsCancellationRequested && !File.Exists(executablePath))
            {
                _notificationService.Display("Could not find daemon executable.");
                await Task.Delay(3000);
                executablePath = _configuration.GetValue<string>("DaemonSettings:ExecutablePath");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var daemonExecutableName = _configuration.GetValue<string>("DaemonSettings:ExecutableName");
                var redirectOutput = _configuration.GetValue<bool>("DaemonSettings:RedirectOutput");
                var processToStart = Process.GetProcessesByName(daemonExecutableName).FirstOrDefault();

                StringBuilder outputBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                if(processToStart is null)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = executablePath;
                    p.StartInfo.UseShellExecute = false;

                    p.StartInfo.CreateNoWindow = redirectOutput;

                    if(redirectOutput)
                    {
                        p.StartInfo.RedirectStandardError = true;
                        p.ErrorDataReceived += (s, e) =>
                        {
                            if (e.Data is null)
                            {
                                if (errorBuilder.Length == 0) return;

                                _notificationService.Display(errorBuilder.ToString());
                                errorBuilder.Clear();
                                return;
                            }

                            errorBuilder.AppendLine(e.Data);
                        };

                        p.StartInfo.RedirectStandardOutput = true;
                        p.OutputDataReceived += (s, e) =>
                        {
                            if(e.Data is null)
                            {
                                if (outputBuilder.Length == 0) return;

                                _notificationService.Display(outputBuilder.ToString());
                                outputBuilder.Clear();
                                return;
                            }

                            _daemonLogCache.Add(e.Data);
                            outputBuilder.AppendLine(e.Data);
                        };


                        p.EnableRaisingEvents = true;
                        p.Exited += (s, e) =>
                        {
                            _notificationService.Display("Daemon process exited");
                        };
                    }
                    

                    try
                    {
                        p.Start();
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                    if(redirectOutput)
                    {
                        p.BeginErrorReadLine();
                        p.BeginOutputReadLine();
                    }
                }

                await Task.Delay(1000);
            }
        }

        private void P_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
