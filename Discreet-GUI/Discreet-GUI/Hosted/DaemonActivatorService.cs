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
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;

namespace Discreet_GUI.Hosted
{
    public class DaemonActivatorService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly NotificationService _notificationService;
        private readonly WalletCache _walletCache;
        private readonly DaemonLogCache _daemonLogCache;
        private readonly DaemonCache _daemonCache;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private Process _daemonProcess = null;

        public DaemonActivatorService(IConfiguration configuration, NotificationService notificationService, WalletCache walletCache, DaemonLogCache daemonLogCache, DaemonCache daemonCache, NavigationServiceFactory navigationServiceFactory)
        {
            _configuration = configuration;
            _notificationService = notificationService;
            _walletCache = walletCache;
            _daemonLogCache = daemonLogCache;
            _daemonCache = daemonCache;
            _navigationServiceFactory = navigationServiceFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            bool useDaemonActivator = _configuration.GetValue<bool>("DaemonSettings:UseActivator");
            var executablePath = _configuration.GetValue<string>("DaemonSettings:ExecutablePath");
            var executableName = _configuration.GetValue<string>("DaemonSettings:ExecutableName");
            var redirectOutput = _configuration.GetValue<bool>("DaemonSettings:RedirectOutput");


            // Check for other possible executable paths before starting
            if(!File.Exists(executablePath) && useDaemonActivator && Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string altPath = Path.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMFILES%"), "Discreet", "Discreet Daemon", "Discreet.exe");
                
                if(!File.Exists(altPath))
                {
                    altPath = Path.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMFILES%"), "Discreet Daemon - Preview", "Discreet Daemon", "Discreet.exe");
                    if(File.Exists(altPath)) 
                    {
                        _notificationService.Display($"Using alternative daemon path: {altPath}");
                    }
                }
                else
                {
                    _notificationService.Display($"Using alternative daemon path: {altPath}");
                }

                executablePath = altPath;
            }
            
            while(!stoppingToken.IsCancellationRequested && !File.Exists(executablePath) && useDaemonActivator)
            {
                _notificationService.Display("Could not find the daemon executable. Please update your 'discreet\\wallet-config\\appsettings.json' file with a full path to the daemon");
                await Task.Delay(3000);
                executablePath = _configuration.GetValue<string>("DaemonSettings:ExecutablePath");
            }

            // Initial check to see if the Daemon was running before the wallet were launched
            _daemonProcess = Process.GetProcessesByName(executableName).FirstOrDefault();
            if(_daemonProcess is not null)
            {
                _daemonProcess.EnableRaisingEvents = true;
                _daemonProcess.Exited += DaemonProcessExited;
                _daemonCache.DaemonStarted = true;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if(this._daemonProcess is not null)
                {
                    await Task.Delay(3000);
                    continue;
                }

                this._daemonProcess = Process.GetProcessesByName(executableName).FirstOrDefault();
                if(this._daemonProcess is not null)
                {
                    this._daemonProcess.EnableRaisingEvents = true;
                    this._daemonProcess.Exited += DaemonProcessExited;
                    continue;
                }

                // At this point the daemon should be started by the wallet, if 'activator' is enabled
                if(!useDaemonActivator)
                {
                    await Task.Delay(3000);
                    continue;
                }

                StringBuilder outputBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();
                
                _daemonProcess = new Process();
                _daemonProcess.StartInfo.FileName = executablePath;
                _daemonProcess.StartInfo.UseShellExecute = false;
                _daemonProcess.StartInfo.CreateNoWindow = redirectOutput;

                if(redirectOutput)
                {
                    _daemonProcess.StartInfo.RedirectStandardError = true;
                    _daemonProcess.ErrorDataReceived += (s, e) =>
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

                    _daemonProcess.StartInfo.RedirectStandardOutput = true;
                    _daemonProcess.OutputDataReceived += (s, e) =>
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
                }

                _daemonProcess.EnableRaisingEvents = true;
                _daemonProcess.Exited += DaemonProcessExited;

                try
                {
                    _daemonProcess.Start();
                }
                catch (Exception e)
                {
                    throw;
                }

                if(redirectOutput)
                {
                    _daemonProcess.BeginErrorReadLine();
                    _daemonProcess.BeginOutputReadLine();
                }

                await Task.Delay(1000);
            }
        }

        private void DaemonProcessExited(object sender, EventArgs e)
        {
            _notificationService.Display("Daemon process exited");
            _daemonCache.DaemonStarted = false;
            _navigationServiceFactory.SetDaemonStartupModal();
            _daemonProcess = null;
        }
    }
}
