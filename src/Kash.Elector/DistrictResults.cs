using Kash.CrossCutting.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kash.Elector
{
    public class DistrictResults
    {
        public District District { get; set; }

        readonly IList<ElectoralListResults> _electoralListsResults = null;
        public IReadOnlyList<ElectoralListResults> ElectoralListsResults
        {
            get
            {
                return new ReadOnlyCollection<ElectoralListResults>(_electoralListsResults);
            }
        }

        public DistrictResults(District district, IList<ElectoralListResults> listResults)
        {
            Check.NotNull(district, nameof(district));
            Check.NotNull(listResults, nameof(listResults));

            District = district;
            _electoralListsResults = listResults;
        }
    }
}
