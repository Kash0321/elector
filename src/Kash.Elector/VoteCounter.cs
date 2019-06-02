using Kash.CrossCutting.Diagnostics;
using Kash.Elector.Data;
using Kash.Elector.Resources;
using System;
using System.Collections.Generic;

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
            var previousVote = VoteRepository.Get(elector);

            if (!(previousVote is null))
            {
                throw new ElectorException(Messages.DuplicatedVote);
            }

            var districtsList = new List<District>(list.Districts);
            if (!districtsList.Contains(elector.District))
            {
                throw new ElectorException(Messages.OutOfDistrictVote);
            }

            VoteRepository.Add(elector, list);

            return true;
        }
    }
}
