using Kash.Elector.Data;
using Kash.Elector.Web.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Kash.Elector.Web.IntegrationTests.Specs
{
    public class DummyControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> Factory;

        public DummyControllerShould(CustomWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
        }

        [Fact]
        public async Task SimulateVotingiImpact()
        {
            //Arrange
            DummyDistrictRepository.Delay = 0;
            var votesToDo = 10;
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true });

            var electorsThatVotes = new List<Elector>();

            //Act
            var start = DateTime.Now;
            for (int i = 0; i < votesToDo; i++)
            {
                var response = await client.GetAsync($"/Dummy/{Guid.NewGuid().ToString()}");
                var electorStr = await response.Content.ReadAsStringAsync();
                electorsThatVotes.Add(JsonConvert.DeserializeObject<Elector>(electorStr));
            }
            var totalTime = DateTime.Now - start;

            //Retrieve info
            Console.WriteLine($"{votesToDo} votos en {totalTime.TotalSeconds} segundos");
        }
    }
}
