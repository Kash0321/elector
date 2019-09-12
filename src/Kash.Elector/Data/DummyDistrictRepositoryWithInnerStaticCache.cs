using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public class DummyDistrictRepositoryWithInnerStaticCache : DummyDistrictRepository
    {
        public DummyDistrictRepositoryWithInnerStaticCache() : base()
        {
            Cache = new Dictionary<int, Dictionary<int, District>>();
        }

        Dictionary<int, Dictionary<int, District>> Cache { get; set; }

        protected override Dictionary<int, District> InternalGetByElection(Election election)
        {
            //Implementación del patrón "Cache aside"

            if (!Cache.TryGetValue(election.Id, out Dictionary<int, District> result))
            {
                result = base.InternalGetByElection(election);
                Cache.Add(election.Id, result);
            }

            return result;
        }
    }
}
