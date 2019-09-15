using Kash.CrossCutting.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kash.Elector
{
    public class ElectionResults
    {
        public Election Election { get; protected set; }

        readonly IList<DistrictResults> _districtResults = null;
        public IReadOnlyList<DistrictResults> DistrictResults
        {
            get
            {
                return new ReadOnlyCollection<DistrictResults>(_districtResults);
            }
        }

        public ElectionResults(Election election)
        {
            Check.NotNull(election, nameof(election));
            Election = election;
            _districtResults = new List<DistrictResults>();
        }

        public void AddResult(DistrictResults result)
        {
            var districtsList = new List<District>(Election.GetDistricts());
            if (districtsList.Contains(result.District))
            {
                _districtResults.Add(result);
            }
        }
    }
}
