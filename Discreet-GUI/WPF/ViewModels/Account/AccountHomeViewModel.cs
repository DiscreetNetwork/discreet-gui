using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        public ObservableCollection<MockItem> MockItems { get; set; } = new ObservableCollection<MockItem>
        {
            new MockItem
            {
               AccountLabel = "First account name",
               AccountBalance = 1127.50f
            },
            new MockItem
            {
               AccountLabel = "Second account name",
               AccountBalance = 7751.10f
            },
        };
    }




    public class MockItem
    {
        public string AccountLabel { get; set; }
        public float AccountBalance { get; set; }
    }
}
