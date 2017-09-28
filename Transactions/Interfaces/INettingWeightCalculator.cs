using System.Collections.Generic;

namespace Transactions.Interfaces
{
    public interface INettingWeightCalculator
    {
        INettingWeightResult GetInitialResult(INettingTarget primary);

        INettingWeightResult GetNettingResult(INettingTarget primary, IEnumerable<INettingSource> netted);

        INettingWeightResult GetUnnettingResult(INettingTarget primary, IEnumerable<INettingSource> unnetted);
    }
}