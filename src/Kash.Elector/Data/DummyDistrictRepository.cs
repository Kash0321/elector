using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Kash.Elector.Data
{
    public class DummyDistrictRepository : IDistrictRepository
    {
        const string ZARAGOZA = "Zaragoza";
        const string HUESCA = "Huesca";
        const string TERUEL = "Teruel";
        const string MORDOR = "Mordor";

        Dictionary<int, Dictionary<int, District>> Districts { get; set; } = null;

        public int Delay { get; set; } = 0;

        public DummyDistrictRepository(int delay)
        {
            Districts = new Dictionary<int, Dictionary<int, District>>()
            {
                {
                    1, //Identificador de la elección
                    new Dictionary<int,District>()
                    {
                        { 1, new District(1, ZARAGOZA, 7) },
                        { 2, new District(2, HUESCA, 3) },
                        { 3, new District(3, TERUEL, 3) },
                        { 4, new District(4, MORDOR, 337) }
                    }
                }
            };

            Delay = delay;
        }

        bool Exists(Election election, int id)
        {
            var electionDistricts = InternalGetByElection(election);
            var result = electionDistricts.TryGetValue(id, out _);

            return result;
        }

        IDictionary<int, District> InternalGetByElection(Election election)
        {
            if (Districts.TryGetValue(election.Id, out Dictionary<int, District> result))
            {
                return result;
            }

            // Simulación de proceso pesado
            Thread.Sleep(Delay);

            return new Dictionary<int, District>();
        }

        bool Exists(Election election, District district)
        {
            return Exists(election, district.Id);
        }

        public void Add(Election election, District district)
        {
            var electionDistricts = InternalGetByElection(election);

            if (Exists(election, district))
            {
                throw new Exception($"Previously exists district with this id");
            }

            electionDistricts.Add(district.Id, district);
        }

        public District Get(Election election, int id)
        {
            var electionDistricts = InternalGetByElection(election);

            if (electionDistricts.TryGetValue(id, out District result))
            {
                return result;
            }

            return null;
        }

        public District Get(Election election, string name)
        {
            var result = default(District);

            var electionDistricts = GetByElection(election);

            result = electionDistricts.Where(d => d.Name == name).FirstOrDefault();

            return result;
        }

        public IEnumerable<District> GetByElection(Election election)
        {
            var electionDistricts = InternalGetByElection(election);
            return electionDistricts.Values;
        }

        public void Remove(Election election, District district)
        {
            if (!Exists(election, district))
            {
                throw new Exception($"District not found");
            }

            var electionDistricts = InternalGetByElection(election);
            electionDistricts.Remove(district.Id);
        }

        public void Remove(Election election, int id)
        {
            if (!Exists(election, id))
            {
                throw new Exception($"District not found");
            }

            var electionDistricts = InternalGetByElection(election);
            electionDistricts.Remove(id);
        }

        public void Remove(Election election, string name)
        {
            var previous = Get(election, name);

            if (previous is null)
            {
                throw new Exception($"District not found");
            }

            var electionDistricts = InternalGetByElection(election);
            electionDistricts.Remove(previous.Id);
        }
    }
}
