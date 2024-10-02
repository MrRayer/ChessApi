using ChessApi.Models.RequestObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HiraganaController : ControllerBase
    {

        private readonly IWebHostEnvironment _environment;

        public HiraganaController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet("GetRomanji")]
        public IActionResult GetRomanji()
        {
            var hiraganaJsonPath = Path.Combine(_environment.ContentRootPath, "hiragana.json");
            if (!System.IO.File.Exists(hiraganaJsonPath))
            {
                return NotFound("hiragana.json file not found.");
            }
            var json = System.IO.File.ReadAllText(hiraganaJsonPath);
            var hiraganaWords = JsonConvert.DeserializeObject<List<HiraganaWord>>(json);
            if (hiraganaWords == null || !hiraganaWords.Any())
            {
                return NotFound("No words available in the hiragana.json file.");
            }
            var random = new Random();
            var randomInt = random.Next(hiraganaWords.Count);
            var randomWord = hiraganaWords[randomInt];
            return Ok(randomWord);
        }
    }
}
