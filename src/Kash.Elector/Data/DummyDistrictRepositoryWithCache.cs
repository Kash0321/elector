using Kash.CrossCutting.Cache;
using System.Collections.Generic;

namespace Kash.Elector.Data
{
    public class DummyDistrictRepositoryWithCache : DummyDistrictRepository
    {
        public ICacheManager CacheManager { get; set; }

        public DummyDistrictRepositoryWithCache(ICacheManager cacheManager) : base()
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
