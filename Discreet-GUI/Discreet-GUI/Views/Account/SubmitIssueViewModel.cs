using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discreet_GUI.ViewModels.Common;
using Services.Testnet;
using Discreet_GUI.Services;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Discreet_GUI.Views.Account
{
    public class SubmitIssueViewModel : ViewModelBase
    {
        private readonly IssueService _issueService;
        private readonly NotificationService _notificationService;

        public string IssueSummary { get; set; }
        public List<int> SeverityOptions { get; set; } = new List<int>(Enum.GetValues(typeof(IssueSeverity)).Cast<int>().ToList());
        public int SelectedSeverityIndex { get; set; }

        public string IssueDescription { get; set; }

        private string _attachmentPath = string.Empty;
        public string AttachmentPath { get => _attachmentPath; set { _attachmentPath = value; OnPropertyChanged(nameof(AttachmentPath)); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public SubmitIssueViewModel(IssueService issueService, NotificationService notificationService)
        {
            _issueService = issueService;
            _notificationService = notificationService;
        }

        async Task SubmitIssue()
        {
            IsLoading = true;
            var submitResult = await _issueService.SubmitIssue(IssueSummary, (IssueSeverity)SelectedSeverityIndex, IssueDescription, AttachmentPath);
            _notificationService.Display(submitResult.Message);
            IsLoading = false;
        }

        async Task OpenFolderDialogCommand()
        {
            var dlg = new OpenFileDialog();
            dlg.Filters.Add(new FileDialogFilter { Name = "All", Extensions = { "*" } });
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var result = await dlg.ShowAsync(desktop.MainWindow);
                if (result != null)
                {
                    AttachmentPath = result.First();
                }
            }
        }
    }
}
