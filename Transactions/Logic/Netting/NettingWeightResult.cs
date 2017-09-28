using System;
using Transactions.Interfaces;
using Transactions.Statics;
using Transactions.Statics.Extensions;

namespace Transactions.Logic.Netting
{
    public class NettingWeightResult : INettingWeightResult
    {
        private readonly INettingTarget _primary;
        private readonly int _primaryDirection;

        public NettingWeightResult(INettingTarget primary)
        {
            _primary = primary;
            _primaryDirection = primary.Type.IsExternalBuy() ? 1 : -1;
            ResultWeight = OriginalWeight = _primaryDirection * primary.AccountingWeight;
            ResultImbalance = OriginalImbalance = primary.ImbalanceWeight;
        }

        public decimal OriginalWeight { get; set; }

        public decimal OriginalImbalance { get; set; }

        public decimal ResultWeight { get; set; }

        public decimal ResultImbalance { get; set; }

        public decimal NettedWeight { get; set; }

        public decimal NettedAdjustment { get; set; }

        public bool DirectionChanging => _primaryDirection * Math.Sign(ResultWeight) < 0;

        public string SelectedWeightAsString => ConvertWeightToString(NettedWeight);

        public string TransferWeightAsString => ConvertWeightToString(ResultWeight);

        public void Net(INettingSource source)
        {
            var weight = source.AccountingWeight;
            var sourceDirection = GetDirection(source.Type);
            var adjustment = source.AdjustmentWeight;
            var netWeight = sourceDirection * weight;
            ResultWeight += netWeight;
            NettedWeight += netWeight;
            ResultImbalance -= _primaryDirection * sourceDirection * weight;
            ResultImbalance += _primaryDirection * adjustment;
            NettedAdjustment += adjustment;
        }

        public void Unnet(INettingSource source)
        {
            var weight = source.AccountingWeight;
            var sourceDirection = GetDirection(source.Type);
            var adjustment = source.AdjustmentWeight;
            var netWeight = sourceDirection * weight;
            ResultWeight -= netWeight;
            NettedWeight += netWeight;
            ResultImbalance += _primaryDirection * sourceDirection * weight;
            ResultImbalance -= _primaryDirection * adjustment;
            NettedAdjustment += adjustment;
        }

        private int GetDirection(TransferType type)
        {
            if (type.IsExternalBuy())
                return 1;
            if (type.IsExternalSell())
                return -1;
            return 0;
        }

        public string ConvertWeightToString(decimal weight)
        {
            var direction = "";
            if (weight > 0)
                direction = " - Buy";
            if (weight < 0)
                direction = " - Sell";

            return $"{Math.Abs(weight):F4} {_primary.AccountingUnit.Name}{direction}";
        }
    }
}