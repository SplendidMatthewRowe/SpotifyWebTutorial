using SpotifyWebTutorial.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpotifyWebTutorial.Services
{
    public class SpotifyAccountService : ISpotifyAccountService  /* <== Make sure you implement the class by adding this bit! */
    {

        //Declare an HttpClient which will be set when we create an implementation of this class
        private readonly HttpClient _httpClient;

        //This is a "constructor" it is called when we create our implementation, it expects to recieve an "httpClient" which we will save as "_httpClient"
        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;   //Set our internal variable using the parameter (setter)
        }

        public async Task<string> GetToken(string clientID, string clientSecret)
        {
            //Create a "request" of type POST meaning we are sending data out 
            //We are sending data to the url https://accounts.spotify.com/api/token <--see token is added!
            var request = new HttpRequestMessage(HttpMethod.Post, "token"); //posts "token" as a word onto the end of the request (which is a url)

            //Add request Authorisation by sending in the clientID and the clientSecret encoded using UTF8, (basically converted to binary)
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientID}:{clientSecret}")));

            //Add request content which is a very short dictionary
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });

            //Send the request and await the result
            var response = await _httpClient.SendAsync(request);

            //Check if the request was successful, if not an error message will be shown
            response.EnsureSuccessStatusCode();

            //Read the response (sent as a stream of data)
            using var responseStream = await response.Content.ReadAsStreamAsync();
            //Deserialise the response into an object (see AuthResult class), because we haven't built the AuthResult object this will show as a 
            //red underline error!
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);

            //Return the deserialised result
            return authResult.access_token;
        }


    }
}
