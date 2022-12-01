namespace Services.Daemon.Wallet.Responses
{
    public class GetWalletHeightResponse
    {
        public long Height { get; set; }
        public bool Synced { get; set; }
    }
}
