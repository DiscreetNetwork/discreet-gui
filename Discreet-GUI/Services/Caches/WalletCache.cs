using Services.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Services.Caches
{
    /// <summary>
    /// Individual delegates being invoked, ensures that ViewModels that subscribe to specific Properties,
    /// only receives what they are asking for, and not the whole WalletCache when anything is updated
    /// </summary>
    public class WalletCache
    {
        /// <summary>
        /// A flag to indicate if the current wallet has been initialized with data
        /// </summary>
        public bool Initialized { get; set; }


        public event Action VisorStartupCompleteChanged;
        private bool _visorStartupComplete = false;
        public bool VisorStartupComplete { get => _visorStartupComplete; set { _visorStartupComplete = value; VisorStartupCompleteChanged?.Invoke(); } }


        public event Action EntropyHashChanged;
        private string _entropyHash;
        public string EntropyHash { get => _entropyHash; set { _entropyHash = value; EntropyHashChanged?.Invoke(); } }

        public event Action LabelChanged;
        string _label;
        public string Label { get => _label; set { _label = value; LabelChanged?.Invoke(); } }


        public event Action LastSeenHeightChanged;
        long _lastSeenHeight;
        public long LastSeenHeight { get => _lastSeenHeight; set { _lastSeenHeight = value; LastSeenHeightChanged?.Invoke(); } }


        public event Action SyncedChanged;
        bool _synced;
        public bool Synced { get => _synced; set { _synced = value; SyncedChanged?.Invoke(); } }


        public event Action NumberOfConnectionsChanged;
        private int _numberOfConnections;
        public int NumberOfConnections { get => _numberOfConnections; set { _numberOfConnections = value; NumberOfConnectionsChanged?.Invoke(); } }

        public ObservableCollectionEx<WalletAddress> Accounts { get; set; } = new ObservableCollectionEx<WalletAddress>();

        public void AddAccount(string name, string address, ulong balance, AddressType addressType)
        {
            Accounts.Add(new WalletAddress { Name = name, Address = address, Balance = balance, Identicon = JazziconEx.IdenticonToAvaloniaBitmap(160, address), Type = addressType });

        }


        /// <summary>
        /// Invoked when the user clicks on one of the accounts
        /// </summary>
        public event Action SelectedAccountChanged;
        private string _selectedAccount;
        public string SelectedAccount { get => _selectedAccount; set { _selectedAccount = value; SelectedAccountChanged?.Invoke(); } }


        public void ClearCache()
        {
            Label = String.Empty;
            Initialized = false;
            Accounts = new ObservableCollectionEx<WalletAddress>();
            LastSeenHeight = 0;
            Synced = false;
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

            private Avalonia.Media.Imaging.Bitmap _identicon;
            public Avalonia.Media.Imaging.Bitmap Identicon { get => _identicon; set { _identicon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Identicon))); } }

            public ObservableCollection<int> UTXOs { get; set; } 
        }

        public enum AddressType
        {
            STEALTH = 0,
            TRANSPARENT = 1
        }
    }
}
