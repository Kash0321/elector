using NUnit.Framework;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Kash.Elector.Resources;
using Moq;
using Kash.Elector.Data;
using System.Linq;

namespace Kash.Elector.Tests
{
    [TestFixture]
    [Category("UnitTests")]
    [TestOf(typeof(VoteCounter))]
    public class VoteCounterShould
    {
        const string ZGZ = "Zaragoza";
        const string HCA = "Huesca";
        const string TRL = "Teruel";
        const string MRD = "Mordor";

        Dictionary<string, District> districts = null;
        Dictionary<string, District> PrepareDistricts()
        {
            if (districts == null)
            {
                districts = new Dictionary<string, District>()
                {
                    { ZGZ, new District(1, ZGZ) },
                    { HCA, new District(1, HCA) },
                    { TRL, new District(1, TRL) },
                };
            }

            return districts;
        }

        District mordor = null;
        District PrepareMordor()
        {
            if (mordor == null)
            {
                mordor = new District(4, MRD);
            }
            return mordor;
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

        const string RED_PARTY = "Partido Rojo";
        const string PURPLE_PARTY = "Partido Morado";
        const string BLUE_PARTY = "Partido Azul";
        const string ORANGE_PARTY = "Partido Naranja";
        const string SAURON_PARTY = "Partido Malvado";

        Dictionary<string, ElectoralList> lists = null;
        Dictionary<string, ElectoralList> PrepareLists()
        {
            if (lists == null)
            {
                lists = new Dictionary<string, ElectoralList>()
                {
                    { RED_PARTY, new ElectoralList(PrepareElection(), RED_PARTY, PrepareDistricts().Values) },
                    { PURPLE_PARTY, new ElectoralList(PrepareElection(), PURPLE_PARTY, PrepareDistricts().Values)},
                    { BLUE_PARTY, new ElectoralList(PrepareElection(), BLUE_PARTY, PrepareDistricts().Values)},
                    { ORANGE_PARTY, new ElectoralList(PrepareElection(), ORANGE_PARTY, PrepareDistricts().Values)},
                    { SAURON_PARTY, new ElectoralList(PrepareElection(), SAURON_PARTY, new List<District>() { PrepareMordor() })}
                };
            }
            return lists;
        }

        List<(Elector Elector, ElectoralList ElectoralList)> votes = 
            new List<(Elector Elector, ElectoralList ElectoralList)>();
        Mock<IVoteRepository> voteRepositoryMock = null;
        Mock<IVoteRepository> PrepareVoteRepositoryMock()
        {
            if (voteRepositoryMock == null)
            {
                voteRepositoryMock = new Mock<IVoteRepository>();

                voteRepositoryMock
                    .Setup(m => m.AddOrUpdate(It.IsAny<Elector>(), It.IsAny<ElectoralList>()))
                    .Callback<Elector, ElectoralList>((e, el) =>
                    {
                        votes.Add((e, el));
                    });

                voteRepositoryMock
                    .Setup(m => m.Count(It.IsAny<ElectoralList>(), It.IsAny<District>()))
                    .Returns<ElectoralList, District>((el, d) =>
                    {
                        var result = votes.Where(v => v.ElectoralList == el && v.Elector.District == d).Count();
                        return result;
                    });

                voteRepositoryMock
                    .Setup(m => m.Get(It.IsAny<Elector>(), It.IsAny<Election>()))
                    .Returns<Elector, Election>((e, et) =>
                    {
                        var result = votes.Where(v => v.Elector == e && v.ElectoralList.Election == et).SingleOrDefault().ElectoralList;
                        return result;
                    });
            }

            return voteRepositoryMock;
        }

        [SetUp]
        public void SetUp()
        {
            election = null;
            lists = null;
            voteRepositoryMock = null;
            mordor = null;
            votes.Clear();
        }

        [Test]
        public void CountOneVote()
        {
            //Arrange
            var elector = new Elector("soyyo", PrepareDistricts()[ZGZ]);
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            var result = target.Vote(elector, PrepareLists()[RED_PARTY]);

            //Assert
            result.Should().BeTrue();
            target.CountVotes(PrepareLists()[RED_PARTY], PrepareDistricts()[ZGZ]).Should().Be(1);
        }

        [Test]
        public void CountSomeVotesAtSameDistrict()
        {
            //Arrange
            var elector1 = new Elector("soyyo1", PrepareDistricts()[ZGZ]);
            var elector2 = new Elector("soyyo2", PrepareDistricts()[ZGZ]);
            var elector3 = new Elector("soyyo3", PrepareDistricts()[ZGZ]);
            var elector4 = new Elector("soyyo4", PrepareDistricts()[ZGZ]);
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            var result = target.Vote(elector1, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector2, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector3, PrepareLists()[PURPLE_PARTY]);
            result = target.Vote(elector4, PrepareLists()[BLUE_PARTY]);

            //Assert
            target.CountVotes(PrepareLists()[RED_PARTY], PrepareDistricts()[ZGZ]).Should().Be(2);
            target.CountVotes(PrepareLists()[PURPLE_PARTY], PrepareDistricts()[ZGZ]).Should().Be(1);
            target.CountVotes(PrepareLists()[BLUE_PARTY], PrepareDistricts()[ZGZ]).Should().Be(1);
        }

        [Test]
        public void CountSomeVotesAtDifferentDistrict()
        {
            //Arrange
            var elector1 = new Elector("soyyo1", PrepareDistricts()[ZGZ]);
            var elector2 = new Elector("soyyo2", PrepareDistricts()[ZGZ]);
            var elector3 = new Elector("soyyo3", PrepareDistricts()[ZGZ]);
            var elector4 = new Elector("soyyo4", PrepareMordor());
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            var result = target.Vote(elector1, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector2, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector3, PrepareLists()[PURPLE_PARTY]);
            result = target.Vote(elector4, PrepareLists()[SAURON_PARTY]);

            //Assert
            target.CountVotes(PrepareLists()[RED_PARTY], PrepareDistricts()[ZGZ]).Should().Be(2);
            target.CountVotes(PrepareLists()[PURPLE_PARTY], PrepareDistricts()[ZGZ]).Should().Be(1);
            target.CountVotes(PrepareLists()[BLUE_PARTY], PrepareDistricts()[ZGZ]).Should().Be(0);
            target.CountVotes(PrepareLists()[ORANGE_PARTY], PrepareDistricts()[ZGZ]).Should().Be(0);
            target.CountVotes(PrepareLists()[SAURON_PARTY], PrepareDistricts()[ZGZ]).Should().Be(0);

            target.CountVotes(PrepareLists()[RED_PARTY], PrepareMordor()).Should().Be(0);
            target.CountVotes(PrepareLists()[PURPLE_PARTY], PrepareMordor()).Should().Be(0);
            target.CountVotes(PrepareLists()[BLUE_PARTY], PrepareMordor()).Should().Be(0);
            target.CountVotes(PrepareLists()[ORANGE_PARTY], PrepareMordor()).Should().Be(0);
            target.CountVotes(PrepareLists()[SAURON_PARTY], PrepareMordor()).Should().Be(1);
        }

        [Test]
        public void NotAllowDuplicatedVotes()
        {
            //Arrange
            var elector = new Elector("soyyo", PrepareDistricts()[ZGZ]);
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            Action action = () =>
            {
                target.Vote(elector, PrepareLists()[RED_PARTY]);
                target.Vote(elector, PrepareLists()[RED_PARTY]);
            };

            //Assert
            action.Should().Throw<ElectorException>().WithMessage(Messages.DuplicatedVote);
        }

        [Test]
        public void NotAllowVoteListsFromOutOfYourDistrict()
        {
            //Arrange
            var elector = new Elector("soyyo", PrepareDistricts()[ZGZ]);
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            Action action = () =>
            {
                target.Vote(elector, PrepareLists()[SAURON_PARTY]);
            };

            //Assert
            action.Should().Throw<ElectorException>().WithMessage(Messages.OutOfDistrictVote);
        }
    }
}
