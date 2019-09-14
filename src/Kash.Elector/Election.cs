using Kash.CrossCutting.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kash.Elector
{
    public class Election
    {
        public int Id { get; protected set; }

        [MaxLength(128)]
        public string Name { get; protected set; }

        protected Election() { }

        public Election(int id, string name, IList<ElectoralList> electoralLists)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(electoralLists, nameof(electoralLists));

            Id = id;
            Name = name;
            _districts = new List<District>();
            foreach (var list in electoralLists)
            {
                foreach (var district in list.Districts)
                {
                    if (!_districts.Contains(district))
                    {
                        _districts.Add(district);
                    }
                }
            }
            _electoralLists = electoralLists;
        }

        readonly IList<District> _districts = null;
        public IReadOnlyList<District> Districts
        {
            get
            {
                return new ReadOnlyCollection<District>(_districts);
            }
        }

        readonly IList<ElectoralList> _electoralLists = null;
        public IReadOnlyList<ElectoralList> ElectoralLists
        {
            get
            {
                return new ReadOnlyCollection<ElectoralList>(_electoralLists);
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
