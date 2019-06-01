namespace Kash.Elector
{
    public interface IElectoralRoll
    {
        Elector GetElector(string credential);
    }
}
