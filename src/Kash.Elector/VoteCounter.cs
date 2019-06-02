using Kash.CrossCutting.Diagnostics;
using Kash.Elector.Data;
using Kash.Elector.Resources;
using System;

namespace Kash.Elector
{
    public class VoteCounter : IVoteCounter
    {
        IVoteRepository VoteRepository { get; set; }

        public VoteCounter(IVoteRepository voteRepository)
        {
            Check.NotNull(voteRepository, nameof(voteRepository));

            VoteRepository = voteRepository;
        }

        public int CountVotes(ElectoralList list, District district)
        {
            return VoteRepository.Count(list, district);
        }

        public bool Vote(Elector elector, ElectoralList list)
        {
            var previousVote = VoteRepository.Get(elector, list.Election);

            if (!(previousVote is null))
            {
                throw new ElectorException(Messages.DuplicatedVote);
            }

            if (!list.Districts.Contains(elector.District))
            {
                throw new ElectorException(Messages.OutOfDistrictVote);
            }

            VoteRepository.AddOrUpdate(elector, list);

            return true;
        }
    }
}
