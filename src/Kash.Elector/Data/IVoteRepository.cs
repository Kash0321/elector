using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public interface IVoteRepository
    {
        void AddOrUpdate(Elector elector, ElectoralList list);

        ElectoralList Get(Elector elector, Election election);

        void Remove(Elector elector, Election election);

        int Count(ElectoralList list, District district);
    }
}
