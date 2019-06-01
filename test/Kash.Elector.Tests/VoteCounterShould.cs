using NUnit.Framework;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Kash.Elector.Resources;
using Moq;
using Kash.Elector.Data;

namespace Kash.Elector.Tests
{
    [TestFixture]
    public class VoteCounterShould
    {
        List<District> districts = null;
        List<District> PrepareDistricts()
        {
            if (districts == null)
            {
                var districtZ = new District(1, "Zaragoza");
                var districtH = new District(2, "Huesca");
                var districtT = new District(3, "Teruel");

                districts = new List<District>() { districtZ, districtH, districtT };
            }

            return districts;
        }

        Election election = null;
        Election PrepareElection()
        {
            if (election == null)
            {
                election = new Election(1, "Elecciones Generales 2019");
            }
            return election;
        }

        List<ElectoralList> lists = null;
        List<ElectoralList> PrepareLists()
        {
            if (lists == null)
            {
                var redParty = new ElectoralList(PrepareElection(), "Partido Rojo", PrepareDistricts());
                var purpleParty = new ElectoralList(PrepareElection(), "Partido Morado", PrepareDistricts());
                var blueParty = new ElectoralList(PrepareElection(), "Partido Azul", PrepareDistricts());
                var orangeParty = new ElectoralList(PrepareElection(), "Partido Naranja", PrepareDistricts());

                lists = new List<ElectoralList>() { redParty, purpleParty, blueParty, orangeParty };
            }
            return lists;
        }

        [Test]
        public void CountVote()
        {
            //Arrange
            var elector = new Elector("soyyo", "Kash", PrepareDistricts()[0]);
            var voteRepository = new Mock<IVoteRepository>();
            var target = new VoteCounter(voteRepository.Object);

            //Act
            var result = target.Vote(elector, PrepareLists()[0]);

            //Assert
            result.Should().BeTrue();
            target.GetVotes(PrepareLists()[0]).Should().Be(1);
        }

        [Test]
        public void NotAllowDuplicatedVotes()
        {
            //Arrange
            var elector = new Elector("soyyo", "Kash", PrepareDistricts()[0]);
            var list = new ElectoralList(PrepareElection(), "Partido Rojo", PrepareDistricts());
            var voteRepository = new Mock<IVoteRepository>();
            var target = new VoteCounter(voteRepository.Object);

            //Act

            Action action = () =>
            {
                target.Vote(elector, list);
                target.Vote(elector, list);
            };

            //Assert
            action.Should().Throw<ElectorException>().WithMessage(Messages.DuplicatedVote);
        }
    }
}
