namespace URLShortening.Interfaces
{
    public interface IUrlShorteningService
    {
        string GenerateShortCode(string url);
    }
}