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

        readonly IList<District> _districts = null;
        public IReadOnlyList<District> Districts
        {
            get
            {
                return new ReadOnlyCollection<District>(_districts);
            }
        }

        protected ElectoralList() { }

        public ElectoralList(string party, IList<District> districts)
        {
            Check.NotEmpty(party, nameof(party));
            Check.NotNull(districts, nameof(districts));

            Party = party;
            _districts = districts;
        }

        public override string ToString()
        {
            return $"{Party}";
        }
    }
}
