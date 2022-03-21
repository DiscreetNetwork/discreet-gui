using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountTransactionsViewModel : ViewModelBase
    {
        public ObservableCollection<MMM> MockItems { get; set; } = new ObservableCollection<MMM>
        {
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
            new MMM { Date = DateTime.UtcNow, ReceivingAccount = "733498scxjchasye73437wsaiuw98478.........", Amount = 1234.54f },
        };
    }



    public class MMM
    {
        public DateTime Date { get; set; }
        public string DateFormatted { get => Date.ToString("dd/MM/yyyy"); }
        public string Time { get => Date.TimeOfDay.ToString("hh':'mm"); }
        public string ReceivingAccount { get; set; }
        public float Amount { get; set; }
    }
}
