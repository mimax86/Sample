using System.Linq;

namespace Transactions.Statics.Extensions
{
    public static class TransferTypeExtensions
    {
        public static readonly TransferType[] ExternalBuyTypes =
        {
            TransferType.TransferBuyType1,
            TransferType.TransferBuyType2,
        };

        public static readonly TransferType[] ExternalSellTypes =
        {
            TransferType.TransferSellType1,
            TransferType.TransferSellType2,
        };

        public static bool IsExternalBuy(this TransferType transferType)
        {
            return ExternalBuyTypes.Contains(transferType);
        }

        public static bool IsExternalSell(this TransferType transferType)
        {
            return ExternalSellTypes.Contains(transferType);
        }
    }
}