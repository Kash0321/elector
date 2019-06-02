using Kash.CrossCutting.Diagnostics;
using System;

namespace Kash.Elector
{
    public class ElectionGobernor : IVoteCounter, IElectoralRoll, IElectionResultsCalculator
    {
        public Election Election { get; protected set; }

        IVoteCounter VoteCounter { get; set; }
        IElectoralRoll ElectoralRoll { get; set; }
        IElectionResultsCalculator ElectionResultsCalculator { get; set; }

        public ElectionGobernor(
            Election election,
            IVoteCounter voteCounter,
            IElectoralRoll electoralRoll,
            IElectionResultsCalculator electionResultsCalculator)
        {
            Check.NotNull(election, nameof(election));
            Check.NotNull(voteCounter, nameof(voteCounter));
            Check.NotNull(electoralRoll, nameof(electoralRoll));
            Check.NotNull(electionResultsCalculator, nameof(electionResultsCalculator));

            Election = election;
            VoteCounter = voteCounter;
            ElectoralRoll = electoralRoll;
            ElectionResultsCalculator = electionResultsCalculator;
        }

        public Elector GetElector(string credential)
        {
            return ElectoralRoll.GetElector(credential);
        }

        public bool Vote(Elector elector, ElectoralList list)
        {
            return VoteCounter.Vote(elector, list);
        }

        public ElectionResults GetResults()
        {
            return ElectionResultsCalculator.GetResults();
        }

        public int CountVotes(ElectoralList list, District district)
        {
            return VoteCounter.CountVotes(list, district);
        }
    }
}
