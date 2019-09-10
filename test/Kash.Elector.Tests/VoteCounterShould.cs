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
    public class VoteCounterShould : ElectorTestFixtureBase
    {
        public Election Election { get; set; }

        public VoteCounterShould()
        {
            Election = PrepareElection();
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
                    .Setup(m => m.Add(It.IsAny<Elector>(), It.IsAny<ElectoralList>()))
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
                    .Setup(m => m.Get(It.IsAny<Elector>()))
                    .Returns<Elector>((e) =>
                    {
                        var result = votes.Where(v => v.Elector == e).SingleOrDefault().ElectoralList;
                        return result;
                    });
            }

            return voteRepositoryMock;
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            voteRepositoryMock = null;
            votes.Clear();
        }

        [Test]
        public void CountOneVote()
        {
            //Arrange
            var elector = new Elector("soyyo", PrepareDistricts(Election)[ZARAGOZA]);
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            var result = target.Vote(elector, PrepareLists()[RED_PARTY]);

            //Assert
            result.Should().BeTrue();
            target.CountVotes(PrepareLists()[RED_PARTY], PrepareDistricts(Election)[ZARAGOZA]).Should().Be(1);
        }

        [Test]
        public void CountSomeVotesAtSameDistrict()
        {
            //Arrange
            var elector1 = new Elector("soyyo1",PrepareDistricts(Election)[ZARAGOZA]);
            var elector2 = new Elector("soyyo2",PrepareDistricts(Election)[ZARAGOZA]);
            var elector3 = new Elector("soyyo3",PrepareDistricts(Election)[ZARAGOZA]);
            var elector4 = new Elector("soyyo4",PrepareDistricts(Election)[ZARAGOZA]);
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            var result = target.Vote(elector1, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector2, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector3, PrepareLists()[PURPLE_PARTY]);
            result = target.Vote(elector4, PrepareLists()[BLUE_PARTY]);

            //Assert
            target.CountVotes(PrepareLists()[RED_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(2);
            target.CountVotes(PrepareLists()[PURPLE_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(1);
            target.CountVotes(PrepareLists()[BLUE_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(1);
        }

        [Test]
        public void CountSomeVotesAtDifferentDistrict()
        {
            //Arrange
            var elector1 = new Elector("soyyo1",PrepareDistricts(Election)[ZARAGOZA]);
            var elector2 = new Elector("soyyo2",PrepareDistricts(Election)[ZARAGOZA]);
            var elector3 = new Elector("soyyo3",PrepareDistricts(Election)[ZARAGOZA]);
            var elector4 = new Elector("soyyo4",PrepareMordor(Election));
            var target = new VoteCounter(PrepareVoteRepositoryMock().Object);

            //Act
            var result = target.Vote(elector1, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector2, PrepareLists()[RED_PARTY]);
            result = target.Vote(elector3, PrepareLists()[PURPLE_PARTY]);
            result = target.Vote(elector4, PrepareLists()[SAURON_PARTY]);

            //Assert
            target.CountVotes(PrepareLists()[RED_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(2);
            target.CountVotes(PrepareLists()[PURPLE_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(1);
            target.CountVotes(PrepareLists()[BLUE_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(0);
            target.CountVotes(PrepareLists()[ORANGE_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(0);
            target.CountVotes(PrepareLists()[SAURON_PARTY],PrepareDistricts(Election)[ZARAGOZA]).Should().Be(0);

            target.CountVotes(PrepareLists()[RED_PARTY],PrepareMordor(Election)).Should().Be(0);
            target.CountVotes(PrepareLists()[PURPLE_PARTY],PrepareMordor(Election)).Should().Be(0);
            target.CountVotes(PrepareLists()[BLUE_PARTY],PrepareMordor(Election)).Should().Be(0);
            target.CountVotes(PrepareLists()[ORANGE_PARTY],PrepareMordor(Election)).Should().Be(0);
            target.CountVotes(PrepareLists()[SAURON_PARTY],PrepareMordor(Election)).Should().Be(1);
        }

        [Test]
        public void NotAllowDuplicatedVotes()
        {
            //Arrange
            var elector = new Elector("soyyo",PrepareDistricts(Election)[ZARAGOZA]);
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
            var elector = new Elector("soyyo",PrepareDistricts(Election)[ZARAGOZA]);
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
