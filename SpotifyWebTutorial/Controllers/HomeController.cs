using Microsoft.AspNetCore.Mvc;
using SpotifyWebTutorial.Models;
using SpotifyWebTutorial.Services;
using System.Diagnostics;

namespace SpotifyWebTutorial.Controllers
{
    public class HomeController : Controller
    {
        //Declare global instances, you need these "types" for the params in the constructor
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly IConfiguration _configuration;
        private readonly ISpotifyService _spotifyService; //New line 1 = create a copy of the new class

        //The constructor which sets up the above it reads in a spotifyAccService object and a configuration 
        //Object and stores it here in this script
        public HomeController(ISpotifyAccountService spotifyAccountService, IConfiguration configuration, ISpotifyService spotifyService) //New param at the end here!
        {
            _spotifyAccountService = spotifyAccountService;
            _configuration = configuration;
            _spotifyService = spotifyService; //New line 2 (don't forget the param above though!)
        }

        //This bit below has changed completly!
        public async Task<IActionResult> Index()
        {

            var newReleases = await GetReleases();  //Calls GetReleases ↓↓↓ method below ↓↓↓

            return View(newReleases);  //Returns a "view" which must be handled by the Home/Index.html page
        }

        private async Task<IEnumerable<Release>> GetReleases()
        {
            try
            {
                //Reads in the key and secret from appsettings.json as arguments.
                //Underscore version taken from line 10!
                var token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
                var newReleases = await _spotifyService.GetNewReleases("GB", 20, token); //Send the token and args to the GetNewReleases method
                return newReleases;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return Enumerable.Empty<Release>();
            }

        }

        //From this point on you can leave the code in place...
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}