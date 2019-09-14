using Kash.CrossCutting.Cache;
using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kash.Elector.Data
{
    public class EFDistrictRepository : IDistrictRepository
    {
        ICacheManager CacheManager { get; set; }

        ElectorContext DataContext { get; set; }

        public EFDistrictRepository(ICacheManager cacheManager, ElectorContext dataContext)
        {
            Check.NotNull(cacheManager, nameof(cacheManager));
            Check.NotNull(dataContext, nameof(dataContext));

            CacheManager = cacheManager;
            DataContext = dataContext;
        }

        protected virtual Dictionary<int, District> InternalGetByElection(Election election)
        {
            //Implementación del patrón "Cache aside"

            if (!CacheManager.TryGet(election.Id.ToString(), out Dictionary<int, District> result))
            {
                result = DataContext.Districts.Where(d => d.Election.Id == election.Id).ToDictionary(d => d.Id);
                CacheManager.Set(election.Id.ToString(), result);
            }

            return result;
        }

        bool Exists(Election election, int id)
        {
            var electionDistricts = InternalGetByElection(election);
            var result = electionDistricts.TryGetValue(id, out _);

            return result;
        }

        bool Exists(Election election, District district)
        {
            return Exists(election, district.Id);
        }

        public void Add(Election election, District district)
        {
            throw new NotImplementedException();
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
