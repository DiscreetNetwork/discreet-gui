using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        public ObservableCollection<MockItem> MockItems { get; set; } = new ObservableCollection<MockItem>(MockItem.Generate(24));

    }


    public class MockItem
    {
        public string AccountLabel { get; set; }
        public float AccountBalance { get; set; }

        public static List<MockItem> Generate(int amount)
        {
            List<MockItem> items = new List<MockItem>();
            for (int i = 0; i < amount; i++)
            {
                items.Add(new MockItem() { AccountLabel = "Account name", AccountBalance = 7777.77f });
            }

            return items;
        }
    }
}
