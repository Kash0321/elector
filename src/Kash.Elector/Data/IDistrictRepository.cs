using System.Collections.Generic;

namespace Kash.Elector.Data
{
    public interface IDistrictRepository
    {
        IEnumerable<District> GetByElection(Election election);

        District Get(Election election, int id);

        District Get(Election election, string name);

        void Add(Election election, District district);

        void Remove(Election election, District district);

        void Remove(Election election, int id);

        void Remove(Election election, string name);
    }
}
