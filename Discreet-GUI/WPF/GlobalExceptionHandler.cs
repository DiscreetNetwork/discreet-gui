using ReactiveUI;
using Services.Testnet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WPF
{
    /// <summary>
    /// React to any uncaught exception and log them accordingly
    /// </summary>
    public class GlobalExceptionHandler
    {
        public const string CRASHLOG_ENDPOINT = "https://issues.discreet.net/crashdump";

        public static void UnhandledExceptionHandler(object s, UnhandledExceptionEventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            var payload = new
            {
                message = ((Exception)e.ExceptionObject).ToString(),
                platform = 1, // wallet
                exceptionOrigin = Enum.GetName(ExceptionOrigin.AppDomain)
            };

            string json = JsonSerializer.Serialize(payload);

            httpClient.PostAsync(CRASHLOG_ENDPOINT, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public static void UnobservedTaskExceptionHandler(object s, UnobservedTaskExceptionEventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            var payload = new
            {
                message = e.Exception.ToString(),
                platform = 1, // wallet
                exceptionOrigin = Enum.GetName(ExceptionOrigin.TaskScheduler)
            };

            string json = JsonSerializer.Serialize(payload);

            httpClient.PostAsync(CRASHLOG_ENDPOINT, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public static ReactiveObserverExceptionHandler GetReactiveObserverExceptionHandler() => new ReactiveObserverExceptionHandler();
    }

    public class ReactiveObserverExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();

            HttpClient httpClient = new HttpClient();

            var payload = new
            {
                message = error.ToString(),
                platform = 1, // wallet
                exceptionOrigin = Enum.GetName(ExceptionOrigin.RxApp)
            };

            string json = JsonSerializer.Serialize(payload);

            httpClient.PostAsync(GlobalExceptionHandler.CRASHLOG_ENDPOINT, new StringContent(json, Encoding.UTF8, "application/json"));

            RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }

        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();

            HttpClient httpClient = new HttpClient();

            var payload = new
            {
                message = value.ToString(),
                platform = 1, // wallet
                exceptionOrigin = Enum.GetName(ExceptionOrigin.RxApp)
            };

            string json = JsonSerializer.Serialize(payload);

            httpClient.PostAsync(GlobalExceptionHandler.CRASHLOG_ENDPOINT, new StringContent(json, Encoding.UTF8, "application/json"));

            RxApp.MainThreadScheduler.Schedule(() => { throw value; });
        }
    }

    public enum ExceptionOrigin
    {
        AppDomain,
        TaskScheduler,
        RxApp,
    }
}
