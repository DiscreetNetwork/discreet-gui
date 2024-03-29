﻿using Services.Caches;
using Services.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.Account
{
    public class AccountReceiveViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;


        public ObservableCollection<MockedItem> MockedItems { get; set; } = new ObservableCollection<MockedItem>
        {
            new MockedItem
            {
                AccountName = "Account name 1",
                AccountAddress = "1Ex7LqDJAvGRc7vGo1GNZjZ5w96AHvi..............Fs7zKWELjcrVArWG9orFUZHAqYG7y",
                AccountBalance = 120.24f
            },
            new MockedItem
            {
                AccountName = "Account name 1",
                AccountAddress = "1Ex7LqDJAvGRc7vGo1GNZjZ5w96AHvi..............Fs7zKWELjcrVArWG9orFUZHAqYG7y",
                AccountBalance = 120.24f
            },
            new MockedItem
            {
                AccountName = "Account name 1",
                AccountAddress = "1Ex7LqDJAvGRc7vGo1GNZjZ5w96AHvi..............Fs7zKWELjcrVArWG9orFUZHAqYG7y",
                AccountBalance = 120.24f
            },
            new MockedItem
            {
                AccountName = "Account name 1",
                AccountAddress = "1Ex7LqDJAvGRc7vGo1GNZjZ5w96AHvi..............Fs7zKWELjcrVArWG9orFUZHAqYG7y",
                AccountBalance = 120.24f
            }
        };

        // QR Code
        Avalonia.Media.Imaging.Bitmap _qrCode = null;

        public Avalonia.Media.Imaging.Bitmap QrCode { get => _qrCode; set { _qrCode = value; OnPropertyChanged(nameof(QrCode)); } }


        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;
        public decimal TotalBalance => Accounts.Sum(x => (decimal)x.Balance);

        public AccountReceiveViewModel() { }

        public AccountReceiveViewModel(WalletCache walletCache)
        {
            _walletCache = walletCache;
            Accounts.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalBalance));


            QrCode = BitmapEx.CreateQRCode("0x57fuyw931209fj90wd");
        }
    }


    public class MockedItem
    {
        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public float AccountBalance { get; set; }
    }
}
