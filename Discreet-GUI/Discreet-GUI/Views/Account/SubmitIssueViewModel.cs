using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discreet_GUI.ViewModels.Common;
using Services.Testnet;
using Discreet_GUI.Services;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Discreet_GUI.Caches;

namespace Discreet_GUI.Views.Account
{
    public class SubmitIssueViewModel : ViewModelBase
    {
        private readonly IssueService _issueService;
        private readonly NotificationService _notificationService;
        private readonly SubmitIssueCache _submitIssueCache;

        private string _issueSummary = string.Empty;
        public string IssueSummary { get => _issueSummary; set { _issueSummary = value; UpdateCache(); OnPropertyChanged(nameof(IssueSummary)); } }

        public List<int> SeverityOptions { get; set; } = new List<int>(Enum.GetValues(typeof(IssueSeverity)).Cast<int>().ToList());

        private int _selectedSeverityIndex = 0;
        public int SelectedSeverityIndex { get => _selectedSeverityIndex; set { _selectedSeverityIndex = value; UpdateCache(); OnPropertyChanged(nameof(SelectedSeverityIndex)); } }


        private string _issueDescription = string.Empty;
        public string IssueDescription { get => _issueDescription; set { _issueDescription = value; UpdateCache(); OnPropertyChanged(nameof(IssueDescription)); } }


        private string _attachmentPath = string.Empty;
        public string AttachmentPath { get => _attachmentPath; set { _attachmentPath = value; UpdateCache(); OnPropertyChanged(nameof(AttachmentPath)); } }


        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public SubmitIssueViewModel(IssueService issueService, NotificationService notificationService, SubmitIssueCache submitIssueCache)
        {
            _issueService = issueService;
            _notificationService = notificationService;
            _submitIssueCache = submitIssueCache;

            _issueSummary = submitIssueCache.Summary;
            _selectedSeverityIndex = submitIssueCache.Severity;
            _issueDescription = submitIssueCache.Description;
            _attachmentPath = submitIssueCache.AttachmentPath;
        }

        async Task SubmitIssue()
        {
            IsLoading = true;
            var submitResult = await _issueService.SubmitIssue(IssueSummary, (IssueSeverity)SelectedSeverityIndex, IssueDescription, AttachmentPath);
            _notificationService.DisplayInformation(submitResult.Message);
            IsLoading = false;
        }

        void UpdateCache()
        {
            _submitIssueCache.Summary = IssueSummary;
            _submitIssueCache.Severity = SelectedSeverityIndex;
            _submitIssueCache.Description = IssueDescription;
            _submitIssueCache.AttachmentPath = AttachmentPath;
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
