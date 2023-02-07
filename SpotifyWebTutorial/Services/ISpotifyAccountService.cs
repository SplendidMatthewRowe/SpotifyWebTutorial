namespace SpotifyWebTutorial.Services
{
    public interface ISpotifyAccountService
    {
        //GetToken will be a Task in new class that sends the clientID and ClientSecret to Spotify (which we copy into this app later) 
        Task<string> GetToken(string clientID, string clientSecret);
        
    }
}
