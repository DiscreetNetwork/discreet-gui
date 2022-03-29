using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Start;

namespace WPF.Caches
{
    /// <summary>
    /// A object to hold the neccesary data throughout the process of creating a new wallet
    /// This object is to be registered as a singleton, and then accessed across different views
    /// </summary>
    public class NewWalletCache
    {
        /// <summary>
        /// The desired name of the wallet
        /// </summary>
        public string WalletName { get; set; } = string.Empty;

        /// <summary>
        /// Where on disk to store the wallet
        /// </summary>
        public string WalletLocation { get; set; } = string.Empty;

        /// <summary>
        /// The generated seedphrase that is associated with the new wallet
        /// </summary>
        public List<MnemonicWord> SeedPhrase { get; set; } = null;

        /// <summary>
        /// Password used for the wallet
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The type of network this wallet should be created on
        /// </summary>
        public string NetworkType { get; set; } = string.Empty;

        /// <summary>
        /// Whether or not to be bootstrapped into the network
        /// </summary>
        public bool Bootstrap { get; set; } = false;



        public void Clear()
        {
            WalletName = string.Empty; 
            WalletLocation = string.Empty;
            SeedPhrase = new List<MnemonicWord>();
            Password = string.Empty;
            NetworkType = string.Empty;
            Bootstrap = false;
        }
    }
}
