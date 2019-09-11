using FluentAssertions;
using Kash.CrossCutting.Cache.InMemory;
using Kash.Elector.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Tests.Integration
{
    [TestFixture]
    [Category("IntegrationTests")]
    public class GetDistrictsImpactMeasurement
    {
        [TestCase(5, 2000, 1)]
        [TestCase(1000, 1, 1)]
        [TestCase(5, 2000, 2)]
        [TestCase(1000, 1, 2)]
        [TestCase(5, 2000, 3)]
        [TestCase(1000, 1, 3)]
        public void SimulateVotingImpact(int votesCount, int delay, int repositoryMode)
        {
            var election = new Election(1, "Generales 2019", new List<ElectoralList> { });
            var districtRepositoryMode1 = new DummyDistrictRepository(election, delay);
            var districtRepositoryMode2 = new DummyDistrictRepositoryWithInnerStaticCache(election, delay);
            var districtRepositoryMode3 = new DummyDistrictRepositoryWithCache(election, delay, new InMemoryCacheManager(new Microsoft.Extensions.Caching.Memory.MemoryCache(null)));
            var random = new Random();
            var electorsThatVotes = new List<Elector>();

            //Vote
            var start = DateTime.Now;
            for (int i = 0; i < votesCount; i++)
            {
                switch (repositoryMode)
                {
                    case 1:
                        electorsThatVotes.Add(new Elector(Guid.NewGuid().ToString(), districtRepositoryMode1.Get(election, random.Next(1, 4))));
                        break;
                    case 2:
                        electorsThatVotes.Add(new Elector(Guid.NewGuid().ToString(), districtRepositoryMode2.Get(election, random.Next(1, 4))));
                        break;
                    case 3:
                        electorsThatVotes.Add(new Elector(Guid.NewGuid().ToString(), districtRepositoryMode3.Get(election, random.Next(1, 4))));
                        break;
                    default:
                        break;
                }
            }
            var totalTime = DateTime.Now - start;

            //totalTime.TotalMilliseconds.Should().BeGreaterOrEqualTo(votesCount * delay);
            Console.WriteLine($"{votesCount} votos en {totalTime.TotalSeconds} segundos");
        }
    }
}
