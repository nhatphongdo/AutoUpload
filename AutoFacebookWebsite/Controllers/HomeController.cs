using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AutoFacebookWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigurationRoot _configuration;

        public HomeController(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> FacebookRedirect(string accessToken)
        {
            var fbClient = FacebookClient.CreateInstance(accessToken, _configuration);

//            var result = await fbClient.Post("914879685288040", @"* Check out these 48 cool ‪#‎Chicken‬ design shirts! *
//Note: Not found in stores! (Print on demand shirts!)
//Link: http://bit.ly/Shop-Love-Chickens-Shirts
//*SHARE* with someone you want to buy this for you!
//And don't forget to like and share with your friends!",
//                new[]
//                {
//                    @"C:\Users\nhatp\OneDrive\Pictures\Saved pictures\bc279f7a-8af4-45b5-acce-ce43fe562bc6_jpg_jpg.jpg",
//                    @"C:\Users\nhatp\OneDrive\Pictures\Saved pictures\New Years - Rome.jpg"
//                });

            //            var result = await fbClient.UploadPhoto("308076629246591", new
            //            {
            //                Caption = @"* Check out these 48 cool ‪#‎Chicken‬ design shirts! *
            //Note: Not found in stores! (Print on demand shirts!)
            //Link: http://bit.ly/Shop-Love-Chickens-Shirts
            //*SHARE* with someone you want to buy this for you!
            //And don't forget to like and share with your friends!",
            //FilePath= @"C:\Users\nhatp\OneDrive\Pictures\Saved pictures\bc279f7a-8af4-45b5-acce-ce43fe562bc6_jpg_jpg.jpg",
            //Published = true});

            return Content("");
        }
    }
}
