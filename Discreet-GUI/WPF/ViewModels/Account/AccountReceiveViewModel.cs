using Avalonia.Controls;
using Avalonia.Threading;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WPF.ExtensionMethods;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    class AccountReceiveViewModel : ViewModelBase
    {
        Avalonia.Media.Imaging.Bitmap _qrCode = null;
        public Avalonia.Media.Imaging.Bitmap QrCode
        {
            get => _qrCode;
            set
            {
                _qrCode = value;
                OnPropertyChanged(nameof(QrCode));
            }
        }

        public AccountReceiveViewModel()
        {
            QrCode = BitmapEx.CreateQRCode("0x57fuyw931209fj90wd");
        }
    }
}
