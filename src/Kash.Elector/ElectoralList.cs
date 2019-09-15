using Kash.CrossCutting.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kash.Elector
{
    public class ElectoralList
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Party { get; protected set; }

        public IList<District> Districts { get; set; }

        protected ElectoralList() { }

        public ElectoralList(string party, IList<District> districts)
        {
            Check.NotEmpty(party, nameof(party));
            Check.NotNull(districts, nameof(districts));

            Party = party;
            Districts = districts;
        }

        public override string ToString()
        {
            return $"{Party}";
        }
    }
}
