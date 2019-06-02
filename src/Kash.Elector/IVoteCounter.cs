namespace Kash.Elector
{
    public interface IVoteCounter
    {
        bool Vote(Elector elector, ElectoralList list);

        int CountVotes(ElectoralList list, District district);
    }
}
