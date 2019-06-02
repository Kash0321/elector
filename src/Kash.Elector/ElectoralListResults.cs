using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public class ElectoralListResults 
    {
        public ElectoralListResults(ElectoralList list, int votes)
        {
            Check.NotNull(list, nameof(list));

            ElectoralList = list;
            Votes = votes;
            Seats = 0;
        }

        public ElectoralList ElectoralList { get; protected set; }

        public int Votes { get; protected set; }

        public int Seats { get; protected set; }

        public void AssignSeat()
        {
            Seats++;
        }
    }
}
