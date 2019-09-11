using Cache.Examples.Infrasctructure.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public class DummyDistrictRepositoryWithCache : DummyDistrictRepository
    {
        public ICacheManager CacheManager { get; set; }

        public DummyDistrictRepositoryWithCache(Election election, int delay, ICacheManager cacheManager) : base(election, delay)
        {
            CacheManager = cacheManager;
        }

        protected override Dictionary<int, District> InternalGetByElection(Election election)
        {
            //Implementación del patrón "Cache aside"

            if (!CacheManager.TryGet(election.Id.ToString(), out Dictionary<int, District> result))
            {
                result = base.InternalGetByElection(election);
                CacheManager.Set(election.Id.ToString(), result);
            }

            return result;
        }
    }
}
