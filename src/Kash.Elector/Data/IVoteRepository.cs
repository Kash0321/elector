using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public interface IVoteRepository
    {
        void SetVote(Elector elector, ElectoralList list);

        bool HasVote(Elector elector, Election election);

        int GetVotes(ElectoralList list);
    }
}
