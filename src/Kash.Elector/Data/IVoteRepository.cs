using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public interface IVoteRepository
    {
        void Add(Elector elector, ElectoralList list);

        ElectoralList Get(Elector elector);

        void Remove(Elector elector);

        int Count(ElectoralList list, District district);
    }
}
