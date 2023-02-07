using SpotifyWebTutorial.Models;

namespace SpotifyWebTutorial.Services
{
    public interface ISpotifyService
    {
       Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken);

    }
}
