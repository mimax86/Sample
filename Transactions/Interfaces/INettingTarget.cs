using Transactions.Statics;

namespace Transactions.Interfaces
{
    public interface INettingTarget
    {
        TransferType Type { get; }

        decimal AccountingWeight { get; }

        decimal AdjustmentWeight { get; }

        Unit AccountingUnit { get; }

        decimal ImbalanceWeight { get; }
    }
}
