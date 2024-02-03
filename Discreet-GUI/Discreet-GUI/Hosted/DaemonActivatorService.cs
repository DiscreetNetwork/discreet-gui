using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Services.Caches;
using System;
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
        private readonly DaemonLogCache _daemonLogCache;
        private readonly DaemonCache _daemonCache;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private Process _daemonProcess = null;

        public DaemonActivatorService(IConfiguration configuration, NotificationService notificationService, DaemonLogCache daemonLogCache, DaemonCache daemonCache, NavigationServiceFactory navigationServiceFactory)
        {
            _configuration = configuration;
            _notificationService = notificationService;
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

            // Check if the discreet daemon is already running
            _daemonProcess = Process.GetProcessesByName(executableName).FirstOrDefault();

            // If it is, start watching that process
            if (_daemonProcess is not null)
            {
                _daemonProcess.EnableRaisingEvents = true;
                _daemonProcess.Exited += DaemonProcessExited;
                _daemonCache.DaemonStarted = true;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_daemonProcess is not null) { await Task.Delay(1000); continue; }   // Dont do anything if the daemon is running
                if (!useDaemonActivator) { await Task.Delay(1000); continue; }          // Dont do anything if the activiator setting is disabled

                // One additional check in case the discreet daemon was suddenly opened in a non expected way
                _daemonProcess = Process.GetProcessesByName(executableName).FirstOrDefault();
                if (_daemonProcess is not null)
                {
                    _notificationService.DisplayInformation("Found a running instance of the discreet daemon.");
                    _daemonProcess.EnableRaisingEvents = true;
                    _daemonProcess.Exited += DaemonProcessExited;
                    _daemonCache.DaemonStarted = true;
                    await Task.Delay(1000);
                    continue;
                }

                // Create a new daemon process
                if (!File.Exists(executablePath))
                {
                    var altPath = GetAlternativeExecutablePath();
                    if (!string.IsNullOrWhiteSpace(altPath))
                    {
                        _notificationService.DisplayInformation($"Found a non-default path to the discreet executable");
                        executablePath = altPath;
                    }
                    else
                    {
                        _notificationService.DisplayInformation($"Could not find the discreet executable on the file system. Please update your configuration file with the correct path to the discreet daemon executable.");
                        executablePath = null;
                        await Task.Delay(5000);
                        continue;
                    }
                }

                StringBuilder outputBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                _daemonProcess = new Process();
                _daemonProcess.StartInfo.FileName = executablePath;
                _daemonProcess.StartInfo.UseShellExecute = false;
                _daemonProcess.StartInfo.CreateNoWindow = redirectOutput;

                if (redirectOutput)
                {
                    _daemonProcess.StartInfo.RedirectStandardError = true;
                    _daemonProcess.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data is null)
                        {
                            if (errorBuilder.Length == 0) return;

                            _notificationService.DisplayError("An error occured in the daemon, please check your logs.");
                            errorBuilder.Clear();
                            return;
                        }

                        errorBuilder.AppendLine(e.Data);
                    };

                    _daemonProcess.StartInfo.RedirectStandardOutput = true;
                    _daemonProcess.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data is null)
                        {
                            if (outputBuilder.Length == 0) return;

                            _notificationService.DisplayInformation(outputBuilder.ToString());
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
                catch (Exception)
                {
                    throw;
                }

                if (redirectOutput)
                {
                    _daemonProcess.BeginErrorReadLine();
                    _daemonProcess.BeginOutputReadLine();
                }

                await Task.Delay(1000);
            }
        }

        private void DaemonProcessExited(object sender, EventArgs e)
        {
            _notificationService.DisplayInformation("The daemon process has been closed.");
            _daemonCache.DaemonStarted = false;
            _navigationServiceFactory.Create<Views.Start.StartViewModel>().Navigate();
            _navigationServiceFactory.SetDaemonStartupModal();
            _daemonProcess = null;
        }

        private string? GetAlternativeExecutablePath()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) // Windows
            {
                FileInfo defaultWindowsPath = new FileInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMFILES%"), "Discreet Daemon", "Discreet.exe"));
                FileInfo windowsAltPath = new FileInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMFILES%"), "Discreet", "Discreet Daemon", "Discreet.exe"));
                FileInfo windowsPreviewPath = new FileInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMFILES%"), "Discreet Daemon - Preview", "Discreet.exe"));

                if (defaultWindowsPath.Exists)
                {
                    return defaultWindowsPath.FullName;
                }
                else if (windowsAltPath.Exists)
                {
                    return windowsAltPath.FullName;
                }
                else if (windowsPreviewPath.Exists)
                {
                    return windowsPreviewPath.FullName;
                }
            }
            else // Linux, MacOS
            {
                FileInfo defaultLinuxPath = new FileInfo("/usr/lib/discreet/Discreet");
                FileInfo linuxPreviewPath = new FileInfo("/usr/lib/discreet-preview/Discreet");

                if (defaultLinuxPath.Exists)
                {
                    return defaultLinuxPath.FullName;
                }
                else if (linuxPreviewPath.Exists)
                {
                    return linuxPreviewPath.FullName;
                }
            }

            return null;
        }
    }
}
