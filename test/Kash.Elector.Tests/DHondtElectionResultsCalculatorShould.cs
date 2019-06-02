using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Kash.Elector.Tests
{
    [TestFixture]
    [Category("UnitTests")]
    [TestOf(typeof(DHondtElectionResultsCalculator))]
    public class DHondtElectionResultsCalculatorShould : ElectorTestFixtureBase
    {
        [Test]
        [TestCase(340000, 280000, 160000, 60000, 999, 3, 3, 1, 0, 999)] //Ejemplo de la wikipedia: https://es.wikipedia.org/wiki/Sistema_D%27Hondt
        [TestCase(100, 100, 100, 100, 999, 2, 2, 2, 1, 999)] //Empate total
        [TestCase(200, 300, 100, 100, 999, 2, 3, 1, 1, 999)] //Numeros fáciles
        [TestCase(700, 100, 100, 100, 999, 7, 0, 0, 0, 999)] //Numeros fáciles
        public void ShouldCalculateRight(int votesRed, int votesPurple, int votesBlue, int votesOrange, int votesSauron, int seatsRed, int seatsPurple, int seatsBlue, int seatsOrange, int seatsSauron)
        {
            //Arrange
            var voteCounterMock = new Mock<IVoteCounter>();
            voteCounterMock.Setup(m => m.CountVotes(It.IsAny<ElectoralList>(), It.IsAny<District>()))
                .Returns<ElectoralList, District>((el, d) =>
                {
                    if (el.Party == RED_PARTY) return votesRed;
                    if (el.Party == PURPLE_PARTY) return votesPurple;
                    if (el.Party == BLUE_PARTY) return votesBlue;
                    if (el.Party == ORANGE_PARTY) return votesOrange;
                    if (el.Party == SAURON_PARTY) return votesSauron;

                    return 0;
                });
            var target = new DHondtElectionResultsCalculator(PrepareElection(), voteCounterMock.Object);

            //Act
            var result = target.GetResults();

            //Assert
            result.Should().NotBeNull();
            //Zaragoza
            result.DistrictResults[0].ElectoralListsResults[0].Seats.Should().Be(seatsRed);
            result.DistrictResults[0].ElectoralListsResults[1].Seats.Should().Be(seatsPurple);
            result.DistrictResults[0].ElectoralListsResults[2].Seats.Should().Be(seatsBlue);
            result.DistrictResults[0].ElectoralListsResults[3].Seats.Should().Be(seatsOrange);
            //Huesca
            result.DistrictResults[1].ElectoralListsResults[0].Seats.Should().Be(seatsRed);
            result.DistrictResults[1].ElectoralListsResults[1].Seats.Should().Be(seatsPurple);
            result.DistrictResults[1].ElectoralListsResults[2].Seats.Should().Be(seatsBlue);
            result.DistrictResults[1].ElectoralListsResults[3].Seats.Should().Be(seatsOrange);
            //Teruel
            result.DistrictResults[2].ElectoralListsResults[0].Seats.Should().Be(seatsRed);
            result.DistrictResults[2].ElectoralListsResults[1].Seats.Should().Be(seatsPurple);
            result.DistrictResults[2].ElectoralListsResults[2].Seats.Should().Be(seatsBlue);
            result.DistrictResults[2].ElectoralListsResults[3].Seats.Should().Be(seatsOrange);
            //Mordor
            result.DistrictResults[3].ElectoralListsResults[0].Seats.Should().Be(seatsSauron);
        }
    }
}
