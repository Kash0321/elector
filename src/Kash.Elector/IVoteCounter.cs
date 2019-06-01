namespace Kash.Elector
{
    public interface IVoteCounter
    {
        bool Vote(Elector elector, ElectoralList list);

        int GetVotes(ElectoralList list);
    }
}
