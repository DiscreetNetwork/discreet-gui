using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ExtensionMethods;

namespace WPF.Caches
{
    /// <summary>
    /// Individual delegates being invoked, ensures that ViewModels that subscribe to specific Properties,
    /// only receives what they are asking for, and not the whole WalletCache when anything is updated
    /// </summary>
    public class WalletCache
    {
        public event Action LabelChanged;
        string _label;
        public string Label { get => _label; set { _label = value; LabelChanged?.Invoke(); } }


        //public event Action TotalBalanceChanged;
        //ulong _totalBalance;
        //public ulong TotalBalance { get => _totalBalance; set { _totalBalance = value; TotalBalanceChanged?.Invoke(); } }


        public event Action LastSeenHeightChanged;
        long _lastSeenHeight;
        public long LastSeenHeight { get => _lastSeenHeight; set { _lastSeenHeight = value; LastSeenHeightChanged?.Invoke(); } }


        public event Action SyncedChanged;
        bool _synced;
        public bool Synced { get => _synced; set { _synced = value; SyncedChanged?.Invoke(); } }


        /// <summary>
        /// Not sure what flag to use for the background service, as i have removed the WalletData object
        /// </summary>
        public bool Initialized { get; set; }



        public ObservableCollectionEx<WalletAddress> Accounts { get; set; }

        public void AddAccount(string name, string address, ulong balance)
        {
            Accounts.Add(new WalletAddress { Name = name, Address = address, Balance = balance });
        }




        public class WalletAddress : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public string Name { get; set; }
            public string Address { get; set; }
            public AddressType Type { get; set; }

            public string Display => $"{Name} ({Address.Substring(0, 4)}...{Address.Substring(Address.Length - 4, 4)})";

            private ulong _balance;
            public ulong Balance { get => _balance; set { _balance = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Balance))); } }


            private bool _synced;
            public bool Synced { get => _synced; set { _synced = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Synced))); } }


            private bool _syncer;
            public bool Syncer { get => _syncer; set { _syncer = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Syncer))); } }


            private long _height;
            public long Height { get => _height; set { _height = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height))); } }

            public ObservableCollection<int> UTXOs { get; set; } 
        }

        public enum AddressType
        {
            STEALTH = 0,
            TRANSPARENT = 1
        }
    }
}
