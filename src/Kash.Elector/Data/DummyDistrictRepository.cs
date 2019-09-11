using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Kash.Elector.Data
{
    public class DummyDistrictRepository : IDistrictRepository
    {
        DummyDatabase Database { get; set; } // Nuestro almacenamiento "simulado"

        public int Delay { get; set; } = 0; // Nuestra latencia o demora en la respuesta del servicio de almacenamiento

        public DummyDistrictRepository(Election election, int delay)
        {
            Delay = delay;
            Database = new DummyDatabase(election);
        }

        bool Exists(Election election, int id)
        {
            var electionDistricts = InternalGetByElection(election);
            var result = electionDistricts.TryGetValue(id, out _);

            return result;
        }

        protected virtual Dictionary<int, District> InternalGetByElection(Election election)
        {
            // Simulación de proceso pesado
            Thread.Sleep(Delay);

            if (Database.Districts.TryGetValue(election.Id, out Dictionary<int, District> result))
            {
                return result;
            }

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
