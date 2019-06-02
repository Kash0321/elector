using System.Collections.Generic;
using System.Linq;

namespace Kash.Elector.Tests
{
    public class ElectorTestFixtureBase
    {
        protected const string ZARAGOZA = "Zaragoza";
        protected const string HUESCA = "Huesca";
        protected const string TERUEL = "Teruel";
        protected const string MORDOR = "Mordor";

        Dictionary<string, District> districts = null;
        protected Dictionary<string, District> PrepareDistricts()
        {
            if (districts == null)
            {
                districts = new Dictionary<string, District>()
                {
                    { ZARAGOZA, new District(1, ZARAGOZA, 7) },
                    { HUESCA, new District(1, HUESCA, 7) },
                    { TERUEL, new District(1, TERUEL, 7) },
                };
            }

            return districts;
        }

        District mordor = null;
        protected District PrepareMordor()
        {
            if (mordor == null)
            {
                mordor = new District(4, MORDOR, 999);
            }
            return mordor;
        }

        Election election = null;
        protected Election PrepareElection()
        {
            if (election == null)
            {
                var districtsList = PrepareDistricts().Values.ToList();
                election = new Election(
                    1,
                    "Elecciones Generales 2019",
                    new List<ElectoralList>()
                    {
                        new ElectoralList(RED_PARTY, districtsList),
                        new ElectoralList(PURPLE_PARTY, districtsList),
                        new ElectoralList(BLUE_PARTY, districtsList),
                        new ElectoralList(ORANGE_PARTY, districtsList),
                        new ElectoralList(SAURON_PARTY, new List<District>() { PrepareMordor() })
                    }
                );
            }

            return election;
        }

        protected const string RED_PARTY = "Partido Rojo";
        protected const string PURPLE_PARTY = "Partido Morado";
        protected const string BLUE_PARTY = "Partido Azul";
        protected const string ORANGE_PARTY = "Partido Naranja";
        protected const string SAURON_PARTY = "Partido Malvado";

        protected Dictionary<string, ElectoralList> PrepareLists()
        {
            var electoralListsList = new List<ElectoralList>(PrepareElection().ElectoralLists);
            return electoralListsList.ToDictionary(e => e.Party);
        }

        public virtual void SetUp()
        {
            election = null;
            mordor = null;
        }
    }
}
