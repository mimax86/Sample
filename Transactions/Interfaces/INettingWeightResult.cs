
namespace Transactions.Interfaces
{
    public interface INettingWeightResult
    {
        decimal OriginalImbalance { get; set; }

        decimal OriginalWeight { get; set; }

        decimal ResultImbalance { get; set; }

        decimal ResultWeight { get; set; }

        decimal NettedWeight { get; set; }

        decimal NettedAdjustment { get; set; }

        bool DirectionChanging { get; }

        string SelectedWeightAsString { get; }

        string TransferWeightAsString { get; }
    }
}