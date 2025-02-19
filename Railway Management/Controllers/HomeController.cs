using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Railway_Management.Models;
using Railway_Management.Services;

namespace Railway_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        
        private const string ENDPOINT = "https://arunk-m5wqdad2-eastus2.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            
        }

        public async Task<IActionResult> Index()
        {
            // string result = await azureOpenAIService.GetChatCompletionTesting("what is c#");
           //string result=await AzureOpenAiService.GetResultAsync("What is C#");
          //  string result2 = await AzureOpenAiService.GetApiResult5("what is C#");
           // Console.WriteLine(result);

            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }



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
