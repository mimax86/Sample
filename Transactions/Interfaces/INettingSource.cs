using Transactions.Statics;

namespace Transactions.Interfaces
{
    public interface INettingSource
    {
        TransferType Type { get; }

        decimal AccountingWeight { get; }

        decimal AdjustmentWeight { get; }

        Unit AccountingUnit { get; }
    }
}