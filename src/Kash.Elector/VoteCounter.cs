using Kash.CrossCutting.Diagnostics;
using Kash.Elector.Data;
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

        public int GetVotes(ElectoralList list)
        {
            return VoteRepository.GetVotes(list);
        }

        public bool Vote(Elector elector, ElectoralList list)
        {
            VoteRepository.SetVote(elector, list);

            return true;
        }
    }
}
