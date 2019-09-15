using Kash.CrossCutting.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Kash.Elector
{
    public class DHondtElectionResultsCalculator : IElectionResultsCalculator
    {
        IVoteCounter VoteCounter { get; set; }
        Election Election { get; set; }

        public DHondtElectionResultsCalculator(Election election, IVoteCounter voteCounter)
        {
            Check.NotNull(voteCounter, nameof(voteCounter));
            Check.NotNull(election, nameof(election));

            VoteCounter = voteCounter;
            Election = election;
        }

        public ElectionResults GetResults()
        {
            var result = new ElectionResults(Election);

            foreach (var district in Election.GetDistricts())
            {
                var districtElectoralLists = Election.GetElectoralLists().Where(e => e.Districts.Any(d => d == district));
                var listResultsList = new List<ElectoralListResults>();

                // Recuento de votos
                foreach (var list in districtElectoralLists)
                {
                    var listResults = new ElectoralListResults(list, VoteCounter.CountVotes(list, district));
                    listResultsList.Add(listResults);
                }

                // Asignación de escaños (por el método D'Hondt)
                var orderedElectoralLists = listResultsList.OrderByDescending(l => l.Votes).ToList();
                for (int i = 1; i <= district.Seats; i++)
                {
                    var maxCoefficientElectoralList = orderedElectoralLists[0];
                    foreach (var list in orderedElectoralLists)
                    {
                        if ((list.Votes / (list.Seats + 1)) > (maxCoefficientElectoralList.Votes / (maxCoefficientElectoralList.Seats + 1)))
                        {
                            maxCoefficientElectoralList = list;
                        }
                    }
                    maxCoefficientElectoralList.AssignSeat();
                }

                var districtResults = new DistrictResults(district, listResultsList);
                result.AddResult(districtResults);
            }

            return result;
        }
    }
}
