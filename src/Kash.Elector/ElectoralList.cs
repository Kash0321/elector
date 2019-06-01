using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public class ElectoralList
    {
        public Election Election { get; protected set; }

        public string Party { get; protected set; }

        public int Votes { get; protected set; }

        List<District> Districts { get; set; }

        public IEnumerable<District> GetDistricts()
        {
            return Districts.ToArray();
        }

        public ElectoralList(Election election, string party, IEnumerable<District> districts)
        {
            Check.NotNull(election, nameof(election));
            Check.NotEmpty(party, nameof(party));
            Check.NotNull(districts, nameof(districts));

            Election = election;
            Party = party;
            Districts = new List<District>();
            foreach (var district in districts)
            {
                Districts.Add(district);
            }
            Votes = 0;
        }
    }
}
