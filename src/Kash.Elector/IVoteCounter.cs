using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public interface IVoteCounter
    {
        void Vote(Elector elector, ElectoralList list);

        Election Election { get; }
    }
}
