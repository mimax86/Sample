using System.Collections.Generic;
using MugenMvvmToolkit;
using Transactions.Interfaces;

namespace Transactions.Logic.Netting
{
    public class NettingWeightCalculator : INettingWeightCalculator
    {
        public INettingWeightResult GetInitialResult(INettingTarget primary)
        {
            return new NettingWeightResult(primary);
        }

        public INettingWeightResult GetNettingResult(INettingTarget primary, IEnumerable<INettingSource> netted)
        {
            var result = new NettingWeightResult(primary);
            netted.ForEach(transfer => result.Net(transfer));
            return result;
        }

        public INettingWeightResult GetUnnettingResult(INettingTarget primary, IEnumerable<INettingSource> unnetted)
        {
            var result = new NettingWeightResult(primary);
            unnetted.ForEach(transfer => result.Unnet(transfer));
            return result;
        }
    }
}